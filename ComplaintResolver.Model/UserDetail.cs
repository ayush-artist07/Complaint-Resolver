using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ComplaintResolver.Model.CustomAnntonations;

namespace ComplaintResolver.Model
{
    /// <summary>
    /// All the details needed for the Sign UP of a new User
    /// </summary>
    public class UserDetail
    {
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name Required")]
        [Display(Name = "First Name")]
        [MinLength(1)]
        [MaxLength(100, ErrorMessage = "Can have max 100 characters")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        [MinLength(1)]
        [MaxLength(100, ErrorMessage = "Can have max 100 characters")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [MinLength(1)]
        [MaxLength(120, ErrorMessage = "Can have max 120 characters")]
        [Display(Name = "Email Id")]
        public string EmailId
        {
            get; set;
        }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [IsDateInRange]
        public System.DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Should Match with the password")]
        public string ConfirmPassword { get; set; }

        public sbyte IsEmailVerified { get; set; }

        public string ActivationCode { get; set; }
    }
}
