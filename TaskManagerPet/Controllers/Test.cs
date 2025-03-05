using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerPet.Data;
using TaskManagerPet.Interfaces;
using TaskManagerPet.Models;

namespace TaskManagerPet.Controllers
{
    [ApiController]
    public class Test : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ProjectContext _context;
        ILogger<AccountController> logger;
        IEmailService _emailSender; 
        public Test(UserManager<User> userManager,ProjectContext context, ILogger<AccountController> logger,IEmailService emailSender)
        {
            
            _userManager = userManager;
            _context = context;
            this.logger = logger;
            _emailSender = emailSender;
        }
        [HttpPost("Test")]
        public async Task<IActionResult> Testing()
        {
            try
            {
                var user = new User { UserName = "testuser" };
                var result = await _userManager.CreateAsync(user, "Test@123");

                if (result.Succeeded)
                    return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Ty gandon {ex.Message}" );
            }

            


            return StatusCode(500, "Failed to create user.");
         
        }


        [HttpGet("Check")]
        [Authorize(Roles = "User")]
        public IActionResult UserCheck()
        {
            var user = HttpContext.User;

            Console.WriteLine(" Логирование клеймов:");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            if (user.Identity is { IsAuthenticated: false })
            {
                return Unauthorized(" Пользователь не аутентифицирован!");
            }

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            return Ok($"Роль пользователя: {userRole}");
        }


        [HttpPost("TestEmail")]
        public async Task<IActionResult> EMAILask(string Email,string code)
        {
            var email = _emailSender.SendCode(Email, code);
            if (email.IsCompleted)
            {
                return Ok("Its working");
            }
            
            
                return BadRequest("Error");
            
        }
        [HttpGet("test-db")]
        public IActionResult TestDatabaseConnection([FromServices] ProjectContext context)
        {
            if (context.TestConnection())
            {
                return Ok("Database connection is working.");
            }
            return StatusCode(500, "Failed to connect to the database.");
        }


        [HttpGet("test-usermanager")]
        public IActionResult TestUserManager()
        {
            if (_userManager == null)
            {
                return StatusCode(500, "UserManager is null.");
            }

            try
            {
                var isDisposed = _userManager.ToString(); 
                return Ok("UserManager is alive.");
            }
            catch (ObjectDisposedException ex)
            {
                return StatusCode(500, $"UserManager is disposed: {ex.Message}");
            }
        }
    }
}
