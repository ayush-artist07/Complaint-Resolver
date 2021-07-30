using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintResolver.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace ComplaintResolver.Model
{
    /// <summary>
    /// Properties of data to be obtained from the complaint form and saving to the database
    /// </summary>
    public class ComplaintForm
    {
        [Display(Name = "Complaint ID")]
        public int Complaint_id { get; set; }

        [Display(Name = "Employee Name")]
        public string Employee_Name { get; set; }

        [Display(Name = "Employee Number")]
        public UInt64 Employee_Number { get; set; }
        public int User_Id { get; set; }

        public string User { get; set; }

        [Display(Name = "Date Assigned")]
        public System.DateTime Date_assigned { get; set; }

        [Display(Name = "Product Type")]
        [Required]
        public EmployeeDepartment ProductType { get; set; }

        [Display(Name = "Complaint Type")]
        [Required]
        public ComplaintType ComplaintType { get; set; }


        [Display(Name = "Complaint Description")]
        [Required]
        public string ComplaintDescription { get; set; }

        [Display(Name = "Phone Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Must contiant 10 digits only")]
        public long PhoneNumber { get; set; }

        [Display(Name = "Reply Comments")]
        public string ReplyComments { get; set; }

        [Display(Name = "Employee ID")]
        public int Employee_id { get; set; }

        public ComplaintStatus Status { get; set; }
        public UserDetail users { get; set; }
        public EmployeeDetail employee { get; set; }

    }
}

