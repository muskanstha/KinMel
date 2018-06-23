using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KinMel.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [RegularExpression("^[-0-9A-Za-z_]{5,15}$", ErrorMessage = "The username needs to be 5-15 characters long and can only contain numbers and alphabets.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }


        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,128}$", ErrorMessage = "The {0} must be at least 6 aphanumeric characters including at least one uppercase letter, one lowercase letter and one number.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsTrue => true;
        [Required]
        [Display(Name = "I accept the terms and conditions.")]
        [Compare("IsTrue", ErrorMessage = "You have to accept the terms and conditions to sign up!")]
        public bool AcceptedTerms { get; set; }

    }
}
