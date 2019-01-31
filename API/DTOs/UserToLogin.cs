using System.ComponentModel.DataAnnotations;

namespace Users.DTOs
{
    public class UserToLogin
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8 characters !")]
        public string Password { get; set; }
    }
}