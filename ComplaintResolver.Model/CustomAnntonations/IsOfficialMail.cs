using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintResolver.Model.CustomAnntonations
{
    public class IsOfficialMail : ValidationAttribute
    {
        /// <summary>
        /// checks whether Employee email is a valid Email or Not 
        /// </summary>
        /// <param name="value">The value entered by the user in the corresponding property</param>
        /// <param name="validationContext"></param>
        /// <returns>If email is not valid as office standards returns Error Message</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = value.ToString();

            if (email.Contains("@lsq.com"))
            {
                return ValidationResult.Success;
            }

            ErrorMessage = ErrorMessage ?? validationContext.DisplayName + " official mail must have @lsq.com"; //if error message is not set from the modal than it is NULL tham the string wtitten here is assigned

            return new ValidationResult(ErrorMessage);

        }
    }
}
