using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComplaintResolver.Model.Enums
{
    /// <summary>
    /// What is the complaint about it offers the list for the same
    /// </summary>
    public enum ComplaintType
    {
        [Display(Name = "Warranty Related")]
        WarrentyRelated,
        [Display(Name = "Not Functioning")]
        NotFunctioning,
        [Display(Name = "Functioninig but not Properly")]
        FunctioningButNotProperly,
        [Display(Name = "Defective Item")]
        DefectiveItem,
    }
}
