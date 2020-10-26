using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MATS.Core;
using MATS.Core.Enumerations;

namespace MATS.WPF.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (Singleton.Edition == MATSEdition.CompactEdition)
            {
                ClientOnlyVisibility = "Visible";
                ClientOnlyEnability = true;
                ServerOnlyVisibility = "Collapsed";
                ServerOnlyEnability = false;
            }
            else
            {
                ClientOnlyVisibility = "Collapsed";
                ClientOnlyEnability = false;
                ServerOnlyVisibility = "Visible";
                ServerOnlyEnability = true;
            }

            CurrentViewModel = TicketViewModel;
            HeaderText = "Tickets Managment";
            
            TicketViewCommand = new RelayCommand(ExecuteTicketViewCommand); 
        }
        private ViewModelBase _currentViewModel;

        /// <summary>
        /// Static instance of one of the ViewModels.
        /// </summary>        
        readonly static TicketViewModel TicketViewModel = new ViewModelLocator().Ticket;
        /// <summary>
        /// The CurrentView property.  The setter is private since only this 
        /// class can change the view via a command. If the View is changed,
        /// we need to raise a property changed event (via INPC).
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public RelayCommand TicketViewCommand { get; private set; }

        /// <summary>
        /// Set the CurrentViewModel to 'FirstViewModel'
        /// </summary>
        private void ExecuteTicketViewCommand()
        {
            HeaderText = "Ticket Managment";
            CurrentViewModel = TicketViewModel;
        }

        string _headerText;

        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                if (_headerText == value)
                    return;
                _headerText = value;
                RaisePropertyChanged("HeaderText");
            }
        }

        #region Properties

        private string _clientOnlyVisibility, _serverOnlyVisibility;
        public string ClientOnlyVisibility
        {
            get { return _clientOnlyVisibility; }
            set
            {
                _clientOnlyVisibility = value;
                RaisePropertyChanged<string>(() => this.ClientOnlyVisibility);
            }
        }
        public string ServerOnlyVisibility
        {
            get { return _serverOnlyVisibility; }
            set
            {
                _serverOnlyVisibility = value;
                RaisePropertyChanged<string>(() => this.ServerOnlyVisibility);
            }
        }

        private bool _clientOnlyEnability, _serverOnlyEnability;
        public bool ClientOnlyEnability
        {
            get { return _clientOnlyEnability; }
            set
            {
                _clientOnlyEnability = value;
                RaisePropertyChanged<bool>(() => this.ClientOnlyEnability);
            }
        }
        public bool ServerOnlyEnability
        {
            get { return _serverOnlyEnability; }
            set
            {
                _serverOnlyEnability = value;
                RaisePropertyChanged<bool>(() => this.ServerOnlyEnability);
            }
        }
        #endregion       
        
    }
}