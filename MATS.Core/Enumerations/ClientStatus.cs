using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MATS.Core.Enumerations
{
    public enum ClientStatus
    {
        [Description("Active Client")]
        Active,
        [Description("Block Client")]
        Blocked//,
        //[Description("Remove Client")]
        //Removed
    }
}
