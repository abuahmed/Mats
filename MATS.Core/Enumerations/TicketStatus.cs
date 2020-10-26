using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MATS.Core.Enumerations
{
    public enum TicketStatus
    {
        [Description("Request Pending")]
        Pending,
        [Description("Posted")]
        Posted,
        [Description("Booked")]
        Booked,
        [Description("Queued")]
        Queued,
        [Description("Issued(OK)")]
        Issued,
        [Description("Void")]
        Void,
        [Description("Confirmed")]
        Confirmed,
        [Description("Canceled")]
        Canceled,
        [Description("Waiting")]
        Waiting,
        [Description("Departured")]
        Departured,
        [Description("Request For Cancel")]//for Canceling confirmed/issued tickets otherwise use delete
        CancelRequest,
        [Description("Request For Delete")]//if >posted otherwise deleted directly
        DeleteRequest,
        [Description("Deleted")]//By Myco
        Deleted
    }
}
