using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MATS.Core.Enumerations
{
    public enum ListTypes
    {
        [Description("Destination City")]
        City=0,
        [Description("Air Lines")]
        AirLine = 1,
        [Description("Routes")]
        Route = 2
    }  
}
