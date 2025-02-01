using System.ComponentModel.DataAnnotations;

namespace TaskManagerPet.DTO
{
    public class LoginDTO
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
