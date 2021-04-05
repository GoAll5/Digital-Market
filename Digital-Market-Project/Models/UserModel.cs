using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Market.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "Your username:")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Your password:")]
        // ReSharper disable once Mvc.TemplateNotResolved
        [UIHint("password")]
        public string Password { get; set; }
        
        [Display(Name = "Your full name:")]
        public string FullName { get; set; }

        [Display(Name = "Your age:")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Your EMail:")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
    }
}
