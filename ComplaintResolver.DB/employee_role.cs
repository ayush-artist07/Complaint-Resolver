//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComplaintResolver.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class employee_role
    {
        public int Employee_Role_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
        public string Role { get; set; }
    
        public virtual employee employee { get; set; }
    }
}
