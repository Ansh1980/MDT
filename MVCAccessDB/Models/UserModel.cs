using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVCAccessDB.Models
{
    public class UserModel
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; } // FirstName (length: 50)
        [Required]
        [DisplayName("last Name")]
        public string Lastname { get; set; } // Lastname (length: 50)
        public string UserName { get; set; } // UserName (length: 50)
        public string Password { get; set; } // Password (length: 500)
        public string PasswordSalt { get; set; } // PasswordSalt (length: 500)
        public bool IsAdmin { get; set; } // IsAdmin
        public bool IsActive { get; set; } // IsActive
        public int UserId { get; set; } // IsActive
        public string Error { get; set; } // IsActive
    }
}