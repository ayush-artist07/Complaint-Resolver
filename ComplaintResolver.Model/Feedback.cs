using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComplaintResolver.Model
{
    /// <summary>
    /// Feedback form Properties
    /// </summary>
    public class Feedback
    {
        [Display(Name = "Feedback Id")]
        public int FeedbackId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Message Is Required")]
        [MinLength(1)]
        [MaxLength(200, ErrorMessage = "Can have max 200 characters")]
        public string Message { get; set; }
        [Display(Name = "Complaint Id")]
        public int ComplaintId { get; set; }
        
        public virtual ComplaintForm complaints { get; set; }

    }
}

