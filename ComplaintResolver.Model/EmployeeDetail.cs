using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintResolver.Model.CustomAnntonations;
using ComplaintResolver.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace ComplaintResolver.Model
{
    /// <summary>
    ///Properties for Details needed to register a new Employee in a particular department
    /// </summary>
    public class EmployeeDetail
    {

        [Display(Name ="Employee Id")]
        public int EmployeeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name Required")]
        [MinLength(1)]
        [MaxLength(100, ErrorMessage = "Can have max 100 characters")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [IsOfficialMail]
        [MinLength(1)]
        [MaxLength(120, ErrorMessage = "Can have max 120 characters")]
        [Display(Name = "Email Id")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must be between 8 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]

        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Must contiant 10 digits only")]
        public long Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required")]
        [Display(Name ="Country")]
        public  EmployeeCountry employeecountry{ get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Department is required")]
        public EmployeeDepartment Department { get; set; }
        public Nullable<int> PendingComplaints { get; set; }
        public Nullable<int> CompletedComplaints { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required")]
        [Display(Name = "Role")]
        public EmployeeRole employeerole { get; set; }
    }
}
