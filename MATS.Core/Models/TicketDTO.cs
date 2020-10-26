using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;
using MATS.Core.Enumerations;
using MATS.Validation.CustomValidationAttributes;

namespace MATS.Core.Models
{
    public class TicketDTO : CommonFieldsA
    {
        public TicketDTO()
        {
            Attachments = new HashSet<AttachmentDTO>();
            Remarks = new HashSet<RemarkDTO>();
            Payments = new HashSet<PaymentDTO>();            
        }

        [Required]
        [MaxLength(50, ErrorMessage = "Exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        [RegularExpression("^[A-Z0-9]{9,10}$", ErrorMessage = "Passport Number is invalid")]
        public string PassengerPassportNumber
        {
            get { return GetValue(() => PassengerPassportNumber); }
            set { SetValue(() => PassengerPassportNumber, value); }
        }

        [Required]
        [MaxLength(150, ErrorMessage = "Exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        [RegularExpression("^([a-zA-Z]+\\ )+([a-zA-Z]+\\ )+[a-zA-Z]{1,16}$", ErrorMessage = "Name is invalid, must be full name with spaces")]
        public string PassengerFullName
        {
            get { return GetValue(() => PassengerFullName); }
            set { SetValue(() => PassengerFullName, value); }
        }

        [Required]
        [MaxLength(50, ErrorMessage = "Exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        public string City
        {
            get { return GetValue(() => City); }
            set { SetValue(() => City, value); }
        }

        public string Route
        {
            get { return GetValue(() => Route); }
            set { SetValue(() => Route, value); }
        }

        public string AirLines
        {
            get { return GetValue(() => AirLines); }
            set { SetValue(() => AirLines, value); }
        }

        public double Amount
        {
            get { return GetValue(() => Amount); }
            set { SetValue(() => Amount, value); }
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequestedDate
        {
            get { return GetValue(() => RequestedDate); }
            set { SetValue(() => RequestedDate, value); }
        }
        public string RequestedDateShort
        {
            get { return RequestedDate.ToShortDateString(); }
        }

        [NotMapped]
        public TicketStatus TicketStatus
        {
            get { return GetValue(() => TicketStatus); }
            set { SetValue(() => TicketStatus, value); }
        }

        public TypeOfTrips TypeOfTrip
        {
            get { return GetValue(() => TypeOfTrip); }
            set { SetValue(() => TypeOfTrip, value); }
        }

        [MaxLength(50, ErrorMessage = "Exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        [RegularExpression("^[a-zA-Z0-9]{10,16}$", ErrorMessage = "Ticket Number is invalid")]
        public string TicketNumber
        {
            get { return GetValue(() => TicketNumber); }
            set { SetValue(() => TicketNumber, value); }
        }

        [DataType(DataType.Date)]
        public DateTime? CheckInDate
        {
            get { return GetValue(() => CheckInDate); }
            set { SetValue(() => CheckInDate, value); }
        }

        [DataType(DataType.Date)]
        public DateTime? FlightDate
        {
            get { return GetValue(() => FlightDate); }
            set { SetValue(() => FlightDate, value); }
        }

        [NotMapped]
        public string FlightDateShort
        {
            get
            {
                if (FlightDate != null)
                    return FlightDate.Value.ToShortDateString();
                else
                    return "";
            }
        }

        public float? CommisionPercent
        {
            get { return GetValue(() => CommisionPercent); }
            set { SetValue(() => CommisionPercent, value); }
        } //Better put in the Client Class

        public decimal? FlightCost
        {
            get { return GetValue(() => FlightCost); }
            set { SetValue(() => FlightCost, value); }
        }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public ClientDTO Client
        {
            get { return GetValue(() => Client); }
            set { SetValue(() => Client, value); }
        }

        public TicketStatus LocalStatus
        {
            get { return GetValue(() => LocalStatus); }
            set
            {
                SetValue(() => LocalStatus, value);
                if (LocalStatusDate != null)
                    LocalStatusShort = LocalStatus + " (" + LocalStatusDate.Value.ToShortDateString() + ")";
            }
        }

        [DataType(DataType.Date)]
        public DateTime? LocalStatusDate
        {
            get { return GetValue(() => LocalStatusDate); }
            set { SetValue(() => LocalStatusDate, value); }
        }

        [NotMapped]
        public string LocalStatusShort
        {
            get
            {
                if (LocalStatusDate != null)
                    return LocalStatus + " (" + LocalStatusDate.Value.ToShortDateString() + ")";
                else
                    return "";
            }
            set { SetValue(() => LocalStatusShort, value); }
        }

        public TicketStatus ServerStatus
        {
            get { return GetValue(() => ServerStatus); }
            set
            {
                SetValue(() => ServerStatus, value);
                if (ServerStatusDate != null)
                    ServerStatusShort = ServerStatus + " (" + ServerStatusDate.Value.ToShortDateString() + ")";
            }
        }

        [DataType(DataType.Date)]
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public DateTime? ServerStatusDate
        {
            get { return GetValue(() => ServerStatusDate); }
            set { SetValue(() => ServerStatusDate, value); }
        }
        
        [NotMapped]
        public string ServerStatusShort
        {
            get
            {
                if (ServerStatusDate != null)
                    return ServerStatus + " (" + ServerStatusDate.Value.ToShortDateString() + ")";
                else
                    return "";
            }
            set { SetValue(() => ServerStatusShort, value); }
        }
        
        [NotMapped]
        public string TicketDetail
        {
            get
            {
                string emp = "";
                if (Id != 0)
                    emp = PassengerFullName + " | " + PassengerPassportNumber + " | " + City + " | " + TicketNumber;            

                return emp;
            }
            set { SetValue(() => TicketDetail, value); }
        }

        [NotMapped]
        public bool PaymentCompleted
        {
            get
            {
                double totallyPaid = Payments.Sum(p => p.AmountPaid);
                if (Amount == totallyPaid)
                    return true;

                return false;
            }
            set { SetValue(() => PaymentCompleted, value); }
        }

        public bool LocalPost { get; set; }
        public bool ServerPost { get; set; }

        [Required]
        public string ROWGUID
        {
            get { return GetValue(() => ROWGUID); }
            set { SetValue(() => ROWGUID, value); }
        }
        
        public ICollection<AttachmentDTO> Attachments { get; set; }
        public ICollection<RemarkDTO> Remarks { get; set; }
        public ICollection<PaymentDTO> Payments { get; set; }
    }
}
