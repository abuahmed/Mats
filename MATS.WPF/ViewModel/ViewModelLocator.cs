using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace MATS.WPF.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static Bootstrapper _bootStrapper;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            if (_bootStrapper == null)
                _bootStrapper = new Bootstrapper();
        }
        
        public MainViewModel Main
        {
            get
            {
                return _bootStrapper.Container.Resolve<MainViewModel>();
            }
        }
        public TicketViewModel Ticket
        {
            get
            {
                return _bootStrapper.Container.Resolve<TicketViewModel>();
            }
        }
        public ClientViewModel Client
        {
            get
            {
                return _bootStrapper.Container.Resolve<ClientViewModel>();
            }
        }
        public ListViewModel List
        {
            get
            {
                return _bootStrapper.Container.Resolve<ListViewModel>();
            }
        }
        public AttachmentViewModel Attachment
        {
            get
            {
                return _bootStrapper.Container.Resolve<AttachmentViewModel>();
            }
        }
        public SendEmailViewModel SendEmail
        {
            get
            {
                return _bootStrapper.Container.Resolve<SendEmailViewModel>();
            }
        }
        public LoginViewModel Login
        {
            get
            {
                return _bootStrapper.Container.Resolve<LoginViewModel>(Guid.NewGuid().ToString());
            }
        }
        public ChangePasswordViewModel ChangePassword
        {
            get
            {
                return _bootStrapper.Container.Resolve<ChangePasswordViewModel>(Guid.NewGuid().ToString());
            }
        }
        public SplashScreenViewModel Splash
        {
            get
            {
                return _bootStrapper.Container.Resolve<SplashScreenViewModel>(Guid.NewGuid().ToString());
            }
        }
        public ActivationViewModel Activation
        {
            get
            {
                return _bootStrapper.Container.Resolve<ActivationViewModel>(Guid.NewGuid().ToString());
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
        
    }
}