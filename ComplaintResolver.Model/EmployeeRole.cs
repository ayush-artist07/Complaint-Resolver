using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintResolver.Model.Enums;

namespace ComplaintResolver.Model
{
    /// <summary>
    /// Properties for the role availabe for an employee 
    /// </summary>
    public class EmployeeRole
    {
        public int Employee_Role_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
        public Employee_Role Role { get; set; }

        public virtual EmployeeDetail employee { get; set; }
    }
}
