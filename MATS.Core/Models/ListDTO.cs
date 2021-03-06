﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;
using MATS.Core.Enumerations;
using MATS.Validation.CustomValidationAttributes;


namespace MATS.Core.Models
{    
    public class ListDTO : EntityBase
    {
        [Key]
        public int Id { get; set; }

        public ListTypes Type
        {
            get { return GetValue(() => Type); }
            set { SetValue(() => Type, value); }
        }
        [Required]
        public string DisplayName
        {
            get { return GetValue(() => DisplayName); }
            set { SetValue(() => DisplayName, value); }
        }
    }
}
