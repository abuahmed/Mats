using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;
using MATS.Core.Enumerations;
using MATS.Validation.CustomValidationAttributes;


namespace MATS.Core.Models
{    
    public class ClientDTO : CommonFieldsA
    {
        public ClientDTO() 
        {
            Tickets = new HashSet<TicketDTO>();
        }

        [MaxLength(50, ErrorMessage = "Client Code exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Client Code contains invalid letters")]        
        public string ClientCode
        {
            get { return GetValue(() => ClientCode); }
            set { SetValue(() => ClientCode, value); }
        }

        [MaxLength(50, ErrorMessage = "Client Name exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Client Name contains invalid letters")]
        [Required] 
        public string DisplayName
        {
            get { return GetValue(() => DisplayName); }
            set { SetValue(() => DisplayName, value); }
        }

        [MaxLength(50, ErrorMessage = "Contact Name exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Client Name contains invalid letters")]         
        public string ContactName
        {
            get { return GetValue(() => ContactName); }
            set { SetValue(() => ContactName, value); }
        }

        [MaxLength(50, ErrorMessage = "Contact Title exceeded 50 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contact Title contains invalid letters")]        
        public string ContactTitle
        {
            get { return GetValue(() => ContactTitle); }
            set { SetValue(() => ContactTitle, value); }
        }

        public decimal? OutStandingDeposit
        {
            get { return GetValue(() => OutStandingDeposit); }
            set { SetValue(() => OutStandingDeposit, value); }
        }

        public ClientStatus ClientStatus
        {
            get { return GetValue(() => ClientStatus); }
            set { SetValue(() => ClientStatus, value); }
        }

        [Required]
        public string ProductKey
        {
            get { return GetValue(() => ProductKey); }
            set { SetValue(() => ProductKey, value); }
        }
                
        [Range(30, 365)]
        public int ExpiryDuration
        {
            get { return GetValue(() => ExpiryDuration); }
            set { SetValue(() => ExpiryDuration, value); }
        }

        public string BIOS_SN
        {
            get { return GetValue(() => BIOS_SN); }
            set { SetValue(() => BIOS_SN, value); }
        }
        
        public int NoOfActivations
        {
            get { return GetValue(() => NoOfActivations); }
            set { SetValue(() => NoOfActivations, value); }
        }

        [Range(1, 5)]
        public int NoOfAllowedPcs
        {
            get { return GetValue(() => NoOfAllowedPcs); }
            set { SetValue(() => NoOfAllowedPcs, value); }
        }

        public DateTime? ActivationDate
        {
            get { return GetValue(() => ActivationDate); }
            set { SetValue(() => ActivationDate, value); }
        }

        public DateTime? ExpirationDate
        {
            get { return GetValue(() => ExpirationDate); }
            set { SetValue(() => ExpirationDate, value); }
        }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public AddressDTO Address
        {
            get { return GetValue(() => Address); }
            set { SetValue(() => Address, value); }
        }
      
        public ICollection<TicketDTO> Tickets { get; set; }        
    }
}
