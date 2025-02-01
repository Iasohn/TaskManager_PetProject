using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly IEmailService _emailSender;
    public AccountController(
        UserManager<User> userManager,
        TokenService tokenService,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailSender)
    {
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
            var account = _userManager.FindByEmailAsync(registerDTO.Email).GetAwaiter().GetResult();
            if(account != null)
            {
                return BadRequest("Please use another email!");
            }
            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
                
            };

            var createUserResult = _userManager.CreateAsync(user, registerDTO.Password).GetAwaiter().GetResult();

            if (createUserResult.Succeeded)
            {
                 _userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _emailSender.SendCode(registerDTO.Email, code);
                Console.WriteLine($"code{code}");
                return Ok(new NewUserDTO
                {
                    Token = _tokenService.CreateToken(user),
                    Email = registerDTO.Email,
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

        // Await the call to FindByEmailAsync
        var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
        
        if (user == null)
            return BadRequest("Errors in email or code");

        var isVerified = _userManager.ConfirmEmailAsync(user, code).GetAwaiter().GetResult();

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

        var user = _userManager.FindByEmailAsync(login.Email.ToLower()).GetAwaiter().GetResult();

        if (user == null)
        {
            return Unauthorized("User not found");
        }

        var result = _signInManager.CheckPasswordSignInAsync(user, login.Password, false).GetAwaiter().GetResult();

        if (!result.Succeeded)
        {
            return Unauthorized("Incorrect password or user");
        }

        return Ok(new NewUserDTO
        {
            
            Email = login.Email,
            Token = _tokenService.CreateToken(user),
        });
    }
}
