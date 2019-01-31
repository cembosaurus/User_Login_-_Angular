using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Users.DTOs
{
    public class UserToRegister
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8 characters !")]
        public string Password { get; set; }
    }
}
