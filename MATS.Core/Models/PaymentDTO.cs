using System;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;

namespace MATS.Core.Models
{
    public class PaymentDTO :  CommonFieldsA
    {
        public double AmountPaid
        {
            get { return GetValue(() => AmountPaid); }
            set { SetValue(() => AmountPaid, value); }
        }
        public DateTime PaidDate
        {
            get { return GetValue(() => PaidDate); }
            set { SetValue(() => PaidDate, value); }
        }

        public string PaymentStatus
        {
            get { return GetValue(() => PaymentStatus); }
            set { SetValue(() => PaymentStatus, value); }
        }

        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public TicketDTO Ticket
        {
            get { return GetValue(() => Ticket); }
            set { SetValue(() => Ticket, value); }
        }
    }
}
