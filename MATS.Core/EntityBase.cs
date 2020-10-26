using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MATS.Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MATS.Core
{
    public abstract class EntityBase: PropertyChangedNotification, IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}
