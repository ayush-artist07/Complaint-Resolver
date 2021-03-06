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
    
    public partial class complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public complaint()
        {
            this.feedback = new HashSet<feedback>();
        }
    
        public int Complaint_Id { get; set; }
        public System.DateTime Date_Assigned { get; set; }
        public string ProductType { get; set; }
        public string ComplaintType { get; set; }
        public string ComplaintDescription { get; set; }
        public long PhoneNumber { get; set; }
        public string ReplyComments { get; set; }
        public string Status { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
    
        public virtual users users { get; set; }
        public virtual employee employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<feedback> feedback { get; set; }
    }
}
