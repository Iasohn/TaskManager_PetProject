using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TaskManagerPet.Data;
using TaskManagerPet.DTO;
using TaskManagerPet.Interfaces;
using TaskManagerPet.Models;
using TaskManagerPet.Services;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AccountController> _logger;
    private readonly ProjectContext _context;
    private readonly IEmailService _emailSender;
    public AccountController(
        UserManager<User> userManager,
        TokenService tokenService,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailSender,
        ProjectContext context)
    {
        _context = context;
        _emailSender = emailSender;
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _logger = logger;
        _roleManager = roleManager;
    }

    [HttpPost("Registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationDTO registerDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var account = await _userManager.FindByEmailAsync(registerDTO.Email);
            if(account != null)
            {
                return BadRequest("Please use another email!");
            }
            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
                
            };

            var createUserResult = await _userManager.CreateAsync(user, registerDTO.Password);

            if (createUserResult.Succeeded)
            {
                 await _userManager.AddToRoleAsync(user, "User");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var sending = _emailSender.SendCode(registerDTO.Email, code);
                Console.WriteLine($"code{code}");
                return Ok(new NewUserDTO
                {
                    Token = _tokenService.CreateToken(user,"User"),
                    Email = registerDTO.Email,
                    RefreshToken = _tokenService.RefreshToken(),
                    Confirm = "Please confirm your email with code,that you received!"
                }
                );
                
               
            }

            else
            {
                _logger.LogError("Error creating user: {Errors}",
                    string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                return StatusCode(500, "Error creating user");
            }

            
        }
        catch (ObjectDisposedException ex)
        {
            _logger.LogError(ex, "A dependency was disposed unexpectedly during registration.");
            return StatusCode(500, $"Internal server error: A resource was disposed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error during registration");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string email, string code)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            return BadRequest("Errors in email or code");

        var user  = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
            return BadRequest("Errors in email or code");

        var isVerified = await _userManager.ConfirmEmailAsync(user, code);

        if (!isVerified.Succeeded)
        {
            return BadRequest("Error confirming email");
        }

        return Ok("You successfully confirmed email");
    }



    [HttpPost("Login")]
    public async Task<IActionResult> LoginPage(LoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(login.Email.ToLower());
        if (user == null)
        {
            return Unauthorized(new { message = "User not found" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
       

        return Ok(new NewUserDTO
        {
            Email = login.Email,
            Token = _tokenService.CreateToken(user,"User"),
            RefreshToken = _tokenService.RefreshToken()
        });
    }

    [HttpGet("login-Google")]
    public async Task<IActionResult> Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return Content("You authenticated"); 
        }


        var redirectUrl = Url.Action("GoogleResponce", "account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties,GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("Google-Response")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded || result.Principal == null)
        {
            return BadRequest("Ошибка аутентификации.");
        }


        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

        
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            
            user = new User
            {
                UserName = name.Replace(" ", "_"), 
                Email = email
            };


            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
              
                
                return BadRequest($"Ошибка создания пользователя: ");
            }

        }

  
        var jwt = _tokenService.CreateToken(user, "User");

  
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok(new { message = "Succesful", token = jwt });
    }

}




