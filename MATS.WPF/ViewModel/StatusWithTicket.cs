using System.Collections.Generic;
using MATS.Core.Enumerations;
using MATS.Core.Models;

namespace MATS.WPF.ViewModel
{
    public class StatusWithTicket
    {
        public StatusWithTicket()
        {
            TicketList = new List<TicketDTO>();
        }
        public TicketStatus Status { get; set; }

        public ICollection<TicketDTO> TicketList { get; set; }
        
        #region may be redundant
        public int CountLines
        {
            get
            {
                return TicketList.Count;
            }
            set
            {
                //CountLines = value;
            }
        }
        public string BackGroundColor
        {
            get
            {
                return TicketList.Count <= 0 ? "Green" : "White";
            }
            set
            {
                //BackGroundColor = value;
            }
        } 
        #endregion

    }
}