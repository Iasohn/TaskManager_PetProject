using System.ComponentModel.DataAnnotations;
using TaskManagerPet.Models;

namespace TaskManagerPet.DTO
{
    public class RegistrationDTO 
    {
        public required string UserName {  get; set; }

        [EmailAddress]
        public required string Email {  get; set; }


        [MaxLength(50)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        
        public required string Password { get; set; }
    }
}
