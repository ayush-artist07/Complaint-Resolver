using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComplaintResolver.Model.Enums
{
    /// <summary>
    /// Lists the products we deal with and we have employees from these department as well as complaints concerning them 
    /// </summary>
    public enum EmployeeDepartment
    {
        Mobile,
        TV,
        Laptop,
        AC,
        [Display(Name = "Washing Machine")]
        WashingMachine,
        Refrigrator,
        Others
    }
}
