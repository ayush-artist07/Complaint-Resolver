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
    
    public partial class feedback
    {
        public int Feedback_Id { get; set; }
        public string Message { get; set; }
        public Nullable<int> Complaint_Id { get; set; }
    
        public virtual complaint complaint { get; set; }
    }
}
