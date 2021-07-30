using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintResolver.Model.CustomAnntonations
{
    public class IsDateInRange : ValidationAttribute
    {
        /// <summary>
        /// Isvalid overiden method validates wether the user entering DOB has age greater than 10 years
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns>Error Message if Age is less than or equal to 10 </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = Convert.ToDateTime(value);

            if (value != null)
            {
                if (date.Year >= DateTime.Now.Year - 10)
                {
                    return new ValidationResult("Age must be greater than 10 years");
                }
            }

            return ValidationResult.Success;
        }    
    }
}
