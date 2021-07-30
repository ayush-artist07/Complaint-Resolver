using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComplaintResolver.Model.Enums
{
    /// <summary>
    /// it has values for the various stages a complaint from user will go through
    /// </summary>
    public enum ComplaintStatus
    {
        Assigned,
        Discussed,
        [Display(Name = "Send To Higher Authority")]
        SendToHigherAuthority,
        Solved

    }
}

