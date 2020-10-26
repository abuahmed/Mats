using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MATS.Core.Common;
using MATS.Core.Models;
using MATS.Repository.Interfaces;

namespace MATS.WPF.ViewModel
{
    public class SendEmailViewModel : ViewModelBase
    {
        private IUnitOfWork _unitOfWork;
        public SendEmailViewModel(IUnitOfWork unitOfWork)
        {
            if (IsInDesignMode) { }
            UnitOfWork = unitOfWork;
            Messenger.Default.Register<EmailDTO>(this, (message) =>
            {
                EmailDetail = message;
            });
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
            set
            {
                _unitOfWork = value;
                RaisePropertyChanged<IUnitOfWork>(() => UnitOfWork);
            }
        }
       
        private EmailDTO _emailDetail;
        public EmailDTO EmailDetail
        {
            get { return _emailDetail; }
            set
            {
                _emailDetail = value;
                RaisePropertyChanged<EmailDTO>(() => EmailDetail);
                if (EmailDetail != null)
                {
                    if (EmailDetail.AttachmentFileName != "")
                    {
                        EmailAttachmentDetail = EmailDetail.AttachmentFileName + ".doc";
                    }
                    else
                    {
                        EmailAttachmentDetail = "No Attachment..";
                    }
                }
            }
        }
        


        private string _emailAttachmentDetail;
        public string EmailAttachmentDetail
        {
            get { return _emailAttachmentDetail; }
            set
            {
                _emailAttachmentDetail = value;
                RaisePropertyChanged<string>(() => EmailAttachmentDetail);
            }
        }
        
        private ICommand _sendEmailCommand;
        public ICommand SendEmailCommand
        {
            get
            {
                return _sendEmailCommand ?? (_sendEmailCommand = new RelayCommand(ExcuteSendEmail, CanSave));
            }
        }

        public void ExcuteSendEmail()
        {
            try
            {
                var localAgency = _unitOfWork.Repository<ClientDTO>().GetAllIncludingChilds(a => a.Address).FirstOrDefault();

                if (localAgency != null)
                {
                    var fromAddress = new MailAddress("agencyonefes@gmail.com", localAgency.DisplayName);
                    const string fromPassword = "Agency1!";

                    var toAddress = new MailAddress(EmailDetail.Recepient, EmailDetail.RecepientName);

                    var addressBcc = new MailAddress(localAgency.Address.PrimaryEmail, localAgency.Address.PrimaryEmail);

                    var message = new MailMessage();

                    message.To.Add(toAddress);


                    //if (ReportToAttach != null)
                    //{
                    //    var oStream = (MemoryStream)ReportToAttach.ExportToStream(ExportFormatType.WordForWindows);
                    //    var at = new Attachment(oStream, EmailDetail.AttachmentFileName + ".doc", "application/doc");
                    //    message.Attachments.Add(at);
                    //}

                    message.Subject = EmailDetail.Subject;
                    message.Body = EmailDetail.Body;
                    message.From = fromAddress;

                    message.CC.Add(addressBcc);

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    smtp.Send(message);
                }

                MessageBox.Show("Email Sent Successfully!!", "Email Sent", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Email Sending Failed, Check your Connection and try again...", "Error Sending Email... ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Validation
        public static int Errors { get; set; }
        public bool CanSave()
        {
            if (Errors == 0)
                return true;
            return false;
        }

        #endregion
    }
}
