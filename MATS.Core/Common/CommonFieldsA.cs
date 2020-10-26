using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.Core.Common
{
    public class CommonFieldsA : EntityBase
    {
        [Key]
        public int Id { get; set; }

        public int Enabled { get; set; }

        public int? CreatedByUserId { get; set; }

        public virtual DateTime? DateRecordCreated { get; set; }

        public int? ModifiedByUserId { get; set; }

        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]        
        public virtual DateTime? DateLastModified { get; set; }

    }
}
