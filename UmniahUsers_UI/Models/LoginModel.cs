using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UmniahUsers_UI.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "Must be between 6 and 12 characters", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
