using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;

namespace MATS.Core.Models
{
    public class ContactDTO : CommonFieldsA
    {
        public string ContactName
        {
            get { return GetValue(() => ContactName); }
            set { SetValue(() => ContactName, value); }
        }
        public string ContactTitle
        {
            get { return GetValue(() => ContactTitle); }
            set { SetValue(() => ContactTitle, value); }
        }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public virtual AddressDTO Address { get; set; }

        
    }
}
