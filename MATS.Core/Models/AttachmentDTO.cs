using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Common;
using MATS.Validation.CustomValidationAttributes;
using MATS.Core.Enumerations;


namespace MATS.Core.Models
{
    public class AttachmentDTO : CommonFieldsA
    {
        [Required]
        public string AttachmentName
        {
            get { return GetValue(() => AttachmentName); }
            set { SetValue(() => AttachmentName, value); }
        }
        public AttachmentStatus AttachmentStatus
        {
            get { return GetValue(() => AttachmentStatus); }
            set { SetValue(() => AttachmentStatus, value); }
        }
        //public byte[] AttachmentFile
        //{
        //    get { return GetValue(() => AttachmentFile); }
        //    set { SetValue(() => AttachmentFile, value); }
        //}
        //[NotMapped]
        //public string Status
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(AttachmentName) && AttachmentFile == null)
        //            return "Uploading/Downloading";
        //        else
        //            return "Upload/Download Completed";
        //    }
        //    set { SetValue(() => Status, value); }
        //}

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public virtual TicketDTO Ticket
        {
            get { return GetValue(() => Ticket); }
            set { SetValue(() => Ticket, value); }
        }
    }
}
