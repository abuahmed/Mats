using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;


namespace MATS.Core.Models
{
    public class RemarkDTO : CommonFieldsA
    {
        
        [DataType(DataType.MultilineText)]
        public string RemarkText
        {
            get { return GetValue(() => RemarkText); }
            set { SetValue(() => RemarkText, value); }
        }//Should Be Rich Text

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public virtual TicketDTO Ticket
        {
            get { return GetValue(() => Ticket); }
            set { SetValue(() => Ticket, value); }
        }
    }
}
