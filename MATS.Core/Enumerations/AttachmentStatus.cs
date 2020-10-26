using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MATS.Core.Enumerations
{  
    public enum AttachmentStatus    
    {
        [Description("Downloading")]
        Downloading = 0,
        [Description("Uploading")]
        Uploading = 1,
        [Description("Completed")]
        Completed = 2
    }
}
