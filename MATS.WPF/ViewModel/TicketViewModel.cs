using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.Repository.Interfaces;
using MATS.Core;
using MATS.WPF.Views;
using MATS.OA;
using System.Net;
using System.IO;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace MATS.WPF.ViewModel
{
    public class TicketViewModel : ViewModelBase
    {
        #region Fields
        private IUnitOfWork _unitOfWork;
        private IMATSServerDbContextUnitOfWork _mATSUnitOfWork;
        private IEnumerable<TicketDTO> _ticketList;
        private IEnumerable<AttachmentDTO> _attachmentList;
        private ObservableCollection<TicketDTO> _tickets;
        private int _totalNumberOfTickets;
        private TicketDTO _selectedTicket, _selectedTicketForSearch;
        private ClientDTO _selectedClient;
        private ObservableCollection<ClientDTO> _clients;
        private ICommand _addNewTicketCommand, _saveTicketCommand, _deleteTicketCommand;
        private ICommand _refreshListCommand;
        private bool _emptyControlVisibility;
        private TicketStatus _selectedTicketStatusLocal, _selectedTicketStatusServer;
        private string _clientOnlyVisibility, _serverOnlyVisibility;
        private bool _clientOnlyEnability, _serverOnlyEnability;
        private System.Timers.Timer _monitorTimer;
        private string _updatingText;
        private const int _monitorTimerDelay = 30000; //60000-1 minute
        private const int _monitorTimerDelayServer = 10000; //60000-1 minute

        #endregion

        #region Constructor

        public TicketViewModel(IUnitOfWork unitOfWork, IMATSServerDbContextUnitOfWork matsUnitofWork)
        {

            UnitOfWork = unitOfWork;
            MATSUnitOfWork = matsUnitofWork;

            TicketList = new List<TicketDTO>();
            Tickets = new ObservableCollection<TicketDTO>();
            SelectedTicket = new TicketDTO();
            StatusWithTickets = new ObservableCollection<ViewModel.StatusWithTicket>();
            SelectedStatusWithTickets = new ViewModel.StatusWithTicket();

            
            Clients = new ObservableCollection<ClientDTO>();

            FillPeriodCombo();
            SelectedPeriod = FilterPeriods.FirstOrDefault();
            LoadLists();
            GetLiveTickets(false);

            #region Controls Enability

            if (Singleton.Edition == MATSEdition.CompactEdition)
            {
                //_monitorTimerDelay = _monitorTimerDelay * Convert.ToInt32(ConfigurationManager.AppSettings.Get("MonitorObserverDelay"));
                _monitorTimer = new System.Timers.Timer(_monitorTimerDelay);

                ClientOnlyVisibility = "Visible";
                ClientOnlyEnability = true;
                ServerOnlyVisibility = "Collapsed";
                ServerOnlyEnability = false;
            }
            else
            {
                _monitorTimer = new System.Timers.Timer(_monitorTimerDelayServer);
                ClientOnlyVisibility = "Collapsed";
                ClientOnlyEnability = false;
                ServerOnlyVisibility = "Visible";
                ServerOnlyEnability = true;
                GetLiveClients();

                BookCommandEnability = true;
                QueueCommandEnability = true;
                IssueCommandEnability = true;
                VoidCommandEnability = true;
                ConfirmCommandEnability = true;
                WaitCommandEnability = true;
                CancelCommandEnability = true;
                DeleteCommandEnability = true;
            }
            //ClientOnlyEnability = true;
            //ServerOnlyEnability = true;
            #endregion

            _monitorTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnMonitorTimerElapsed);
            _monitorTimer.Enabled = true;
        }

        #endregion

        #region Background Worker
        bool _updatesFound, _noConnection, _refreshed;

        private void OnMonitorTimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            _monitorTimer.Enabled = false;
            _updatesFound = false; _noConnection = false;
            if (_refreshed)
                UpdatingText = "Searching for updates...";
            try
            {

                var worker = new BackgroundWorker();
                if (Singleton.Edition == MATSEdition.CompactEdition)
                {
                    worker.DoWork += UploadDownloadTicketDetail;
                }
                else
                {
                    worker.DoWork += UploadDownloadTicketDetailForServer;
                }
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch
            {
            }

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _monitorTimer.Enabled = true;
            if (_updatesFound)
            {
                UpdatingText = "There exists new updates, refresh to see the updates";
                _refreshed = false;
            }
            else if (_refreshed)
            {
                UpdatingText = _noConnection ? "There is no Internet connection..." : "No updates found...";
            }
        }

        private void UploadDownloadTicketDetail(object sender, DoWorkEventArgs e)
        {
            //try
            //{
            //    //Check for internet connection
            //    var stream = new WebClient().OpenRead("http://localhost:8080/onefes/public/login.php");//http://www.amihanit.com
            //}
            //catch
            //{
            //    _noConnection = true;
            //    return;
            //}
            try
            {
                #region Upload from local
                var ticketlist = _unitOfWork.Repository<TicketDTO>()
                                .GetAll().ToList();
                var ticketList = ticketlist.Where(t => t.LocalPost == false)
                    .OrderBy(s => s.LocalStatus).ToList();

                foreach (var ticket in ticketList)
                {
                    var tkt = MATSUnitOfWork.Tickets.FirstOrDefault(t => 
                        t.ClientId == Singleton.ProductActivation.ClientId &&
                        t.ROWGUID == ticket.ROWGUID);

                    if (tkt == null)
                    {
                        tkt = new Ticket
                        {
                            ServerStatus = (int) TicketStatus.Posted,
                            ServerStatusDate = DateTime.Now,
                            LocalStatus = (int) TicketStatus.Posted,
                            LocalStatusDate = DateTime.Now
                        };

                        ticket.ServerStatus = TicketStatus.Posted;
                        ticket.ServerStatusDate = DateTime.Now;
                        ticket.LocalStatus = TicketStatus.Posted;
                        ticket.LocalStatusDate = DateTime.Now;
                    }

                    tkt.RequestedDate = ticket.RequestedDate;
                    tkt.City = ticket.City;
                    tkt.ROWGUID = ticket.ROWGUID;
                    tkt.PassengerPassportNumber = ticket.PassengerPassportNumber;
                    tkt.PassengerFullName = ticket.PassengerFullName;
                    tkt.ClientId = Singleton.ProductActivation.ClientId;
                    tkt.TypeOfTrip = (int)ticket.TypeOfTrip;
                    tkt.LocalStatus = (int)ticket.LocalStatus;
                    tkt.LocalStatusDate = ticket.LocalStatusDate;
                    tkt.LocalPost = false;//to tell the server system data is changed from the client side
                    //and will apear in the list of tickets updated from the client side

                    MATSUnitOfWork.Add(tkt);
                    MATSUnitOfWork.SaveChanges();

                    ticket.LocalPost = true;
                    _unitOfWork.Repository<TicketDTO>().Update(ticket);

                    _updatesFound = true;
                }
                _unitOfWork.Commit();
                #endregion

                #region Download status from server
                var serverTickets = MATSUnitOfWork.Tickets.Where(t =>
                                   t.ClientId == Singleton.ProductActivation.ClientId && t.ServerPost == false);
                foreach (var serverTicket in serverTickets)
                {
                    var localTicket = ticketlist.FirstOrDefault(lt => lt.ROWGUID == serverTicket.ROWGUID);
                    if (serverTicket.ServerStatus == 12)
                    {
                        _unitOfWork.Repository<TicketDTO>().Delete(localTicket);
                        _unitOfWork.Commit();
                        return;
                    }

                    localTicket.ServerStatus = (TicketStatus)serverTicket.ServerStatus;
                    localTicket.ServerStatusDate = serverTicket.ServerStatusDate;
                    localTicket.TicketNumber = serverTicket.TicketNumber;
                    localTicket.Amount = serverTicket.Amount;
                    localTicket.AirLines = serverTicket.AirLines;
                    localTicket.Route = serverTicket.Route;
                    localTicket.CheckInDate = serverTicket.CheckInDate;
                    localTicket.FlightDate = serverTicket.FlightDate;
                    localTicket.ServerPost = false;//to tell the client system data is changed from the server side
                    //and will apear in the list of tickets updated from the server side

                    _unitOfWork.Repository<TicketDTO>().Update(localTicket);

                    var attachments = serverTicket.Attachments;
                    foreach (var attach in attachments)
                    {
                        var localattach = AttachmentList
                            .FirstOrDefault(at => at.AttachmentName == attach.AttachmentName && at.TicketId == localTicket.Id);
                        if (localattach == null)
                        {
                            localattach = new AttachmentDTO();
                            localattach.AttachmentName = attach.AttachmentName;
                            localattach.AttachmentStatus = AttachmentStatus.Downloading;
                            localattach.TicketId = localTicket.Id;
                            _unitOfWork.Repository<AttachmentDTO>().Insert(localattach);
                        }
                    }

                    _unitOfWork.Commit();

                    serverTicket.ServerPost = true;
                    MATSUnitOfWork.Add(serverTicket);

                    _updatesFound = true;
                }
                MATSUnitOfWork.SaveChanges();
                #endregion
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

            DownloadFilesFromServer("");//put the correct server
        }

        private void UploadDownloadTicketDetailForServer(object sender, DoWorkEventArgs e)
        {
            //try
            //{
            //    //Check for internet connection
            //    //var stream = new WebClient().OpenRead("http://localhost:8080/onefes/public/login.php");//http://www.amihanit.com
            //}
            //catch 
            //{
            //    _noConnection = true;
            //    return;
            //}

            try
            {
                var ticketlist = _unitOfWork.Repository<TicketDTO>()
                    .GetAll().ToList();
                _updatesFound = ticketlist.Any(t => t.LocalPost == false);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public void DownloadFilesFromServer(string ftpServer)//for eg. "ftp://ftpserver.com/matsfiles/clientId"
        {
            try
            {
                var destDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\MATS_Download_Files" + "\\";
                if (!Directory.Exists(destDirectory))
                    Directory.CreateDirectory(destDirectory);

                var client = new WebClient
                {
                    Credentials = new NetworkCredential("username", "password")
                };

                if (!Directory.Exists(ftpServer))
                    return;

                IEnumerable<FileInfo> fileList = new DirectoryInfo(ftpServer).GetFiles();

                foreach (var fi in fileList)
                {
                    try
                    {
                        var localfi = new FileInfo(Path.Combine(destDirectory, fi.Name));
                        if (localfi.Exists)
                            localfi.Delete();

                        client.DownloadFile(fi.Name, localfi.FullName);
                        fi.MoveTo(Path.Combine("ftp://ftpserver.com/matsfilesmoved/", fi.Name));


                        #region Change Attachment Status to completed
                        try
                        {
                            var attachment = AttachmentList.FirstOrDefault(a =>
                                                       a.AttachmentName == fi.Name.Substring(fi.Name.LastIndexOf("_" + 1)) &&
                                                       a.Ticket.ROWGUID == fi.Name.Substring(0, fi.Name.LastIndexOf("_" + 1)));
                            if (attachment != null)
                            {
                                attachment.AttachmentStatus = AttachmentStatus.Completed;
                                _unitOfWork.Repository<AttachmentDTO>().Update(attachment);
                                _unitOfWork.Commit();
                            }
                        }
                        catch { }

                        #endregion
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(DateTime.Now.ToLongDateString() + " - " + ex.Message);
            }
        }

        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
            set
            {
                _unitOfWork = value;
                RaisePropertyChanged<IUnitOfWork>(() => this.UnitOfWork);
            }
        }
        public IMATSServerDbContextUnitOfWork MATSUnitOfWork
        {
            get { return _mATSUnitOfWork; }
            set
            {
                _mATSUnitOfWork = value;
                RaisePropertyChanged<IMATSServerDbContextUnitOfWork>(() => this.MATSUnitOfWork);
            }
        }
        public bool EmptyControlVisibility
        {
            get { return _emptyControlVisibility; }
            set
            {
                _emptyControlVisibility = value;
                RaisePropertyChanged<bool>(() => this.EmptyControlVisibility);
            }
        }
        public string UpdatingText
        {
            get { return _updatingText; }
            set
            {
                _updatingText = value;
                RaisePropertyChanged<string>(() => this.UpdatingText);
            }
        }
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

        public IEnumerable<TicketDTO> TicketList
        {
            get { return _ticketList; }
            set
            {
                _ticketList = value;
                RaisePropertyChanged<IEnumerable<TicketDTO>>(() => this.TicketList);
            }
        }
        public IEnumerable<AttachmentDTO> AttachmentList
        {
            get { return _attachmentList; }
            set
            {
                _attachmentList = value;
                RaisePropertyChanged<IEnumerable<AttachmentDTO>>(() => this.AttachmentList);
            }
        }
        public ObservableCollection<TicketDTO> Tickets
        {
            get { return _tickets; }
            set
            {
                _tickets = value;
                RaisePropertyChanged<ObservableCollection<TicketDTO>>(() => this.Tickets);
                if (Tickets.Any())
                {
                    SelectedTicket = Tickets[0];
                    EmptyControlVisibility = true;
                }
                else
                {
                    EmptyControlVisibility = false;
                }
                TotalNumberOfTickets = Tickets.Count;
            }
        }
        public int TotalNumberOfTickets
        {
            get { return _totalNumberOfTickets; }
            set
            {
                _totalNumberOfTickets = value;
                RaisePropertyChanged<int>(() => this.TotalNumberOfTickets);
            }
        }
        public TicketDTO SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                RaisePropertyChanged<TicketDTO>(() => this.SelectedTicket);

                if (SelectedTicket != null && SelectedTicket.Id != 0)
                {
                    #region Buttons Enability
                    if (Singleton.Edition == MATSEdition.CompactEdition)
                    {
                        #region Compact
                        if (SelectedTicket.ServerStatus == TicketStatus.Pending)
                        {
                            SaveCommandEnability = true;
                            CancelRequestCommandEnability = false;
                            DeleteRequestCommandEnability = false;
                        }
                        else
                        {
                            SaveCommandEnability = false;
                            CancelRequestCommandEnability = true;
                            DeleteRequestCommandEnability = true;
                        }

                        DownloadRequestCommandEnability = SelectedTicket.Attachments.Count > 0;

                        #endregion
                    }
                    else
                    {
                        #region Server
                        if (SelectedTicket.ServerStatus == TicketStatus.DeleteRequest)
                            DeleteCommandEnability = true;
                        else
                            DeleteCommandEnability = false;
                        #endregion
                    }
                    #endregion


                    if (!string.IsNullOrEmpty(SelectedTicket.City))
                        SelectedCity = Cities.FirstOrDefault(c => c.Display.Equals(SelectedTicket.City));
                    if (!string.IsNullOrEmpty(SelectedTicket.AirLines))
                        SelectedAirLine = AirLines.FirstOrDefault(c => c.Display.Equals(SelectedTicket.AirLines));
                    if (!string.IsNullOrEmpty(SelectedTicket.Route))
                        SelectedRoute = Routes.FirstOrDefault(c => c.Display.Equals(SelectedTicket.Route));
                }
            }
        }
        public TicketDTO SelectedTicketForSearch
        {
            get { return _selectedTicketForSearch; }
            set
            {
                _selectedTicketForSearch = value;
                RaisePropertyChanged<TicketDTO>(() => this.SelectedTicketForSearch);
                if (SelectedTicketForSearch != null && !string.IsNullOrEmpty(SelectedTicketForSearch.TicketDetail))
                {
                    SelectedTicket = SelectedTicketForSearch;
                }

            }
        }
        public TicketStatus SelectedTicketStatusLocal
        {
            get { return _selectedTicketStatusLocal; }
            set
            {
                _selectedTicketStatusLocal = value;
                RaisePropertyChanged<TicketStatus>(() => this.SelectedTicketStatusLocal);

            }
        }
        public TicketStatus SelectedTicketStatusServer
        {
            get { return _selectedTicketStatusServer; }
            set
            {
                _selectedTicketStatusServer = value;
                RaisePropertyChanged<TicketStatus>(() => this.SelectedTicketStatusServer);

            }
        }

        public ClientDTO SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                RaisePropertyChanged<ClientDTO>(() => this.SelectedClient);

                if (SelectedClient != null)
                {
                    GetLiveTickets(false);
                }

            }
        }
        public ObservableCollection<ClientDTO> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                RaisePropertyChanged<ObservableCollection<ClientDTO>>(() => this.Clients);
            }
        }

        #region Client Buttons Enability
        private bool _saveCommandEnability,
            _postCommandEnability,
            _cancelRequestCommandEnability,
            _downloadRequestCommandEnability,
            _deleteRequestCommandEnability;
        public bool SaveCommandEnability
        {
            get { return _saveCommandEnability; }
            set
            {
                _saveCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.SaveCommandEnability);
            }
        }
        public bool CancelRequestCommandEnability
        {
            get { return _cancelRequestCommandEnability; }
            set
            {
                _cancelRequestCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.CancelRequestCommandEnability);
            }
        }
        public bool DownloadRequestCommandEnability
        {
            get { return _downloadRequestCommandEnability; }
            set
            {
                _downloadRequestCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.DownloadRequestCommandEnability);
            }
        }
        public bool DeleteRequestCommandEnability
        {
            get { return _deleteRequestCommandEnability; }
            set
            {
                _deleteRequestCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.DeleteRequestCommandEnability);
            }
        }
        #endregion

        #region Server Buttons Enability
        private bool _bookCommandEnability,
                       _queueCommandEnability,
                       _issueCommandEnability,
                       _voidCommandEnability,
                       _confirmCommandEnability,
                       _waitCommandEnability,
                       _cancelCommandEnability,
                       _deleteCommandEnability;
        public bool BookCommandEnability
        {
            get { return _bookCommandEnability; }
            set
            {
                _bookCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.BookCommandEnability);
            }
        }
        public bool QueueCommandEnability
        {
            get { return _queueCommandEnability; }
            set
            {
                _queueCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.QueueCommandEnability);
            }
        }
        public bool IssueCommandEnability
        {
            get { return _issueCommandEnability; }
            set
            {
                _issueCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.IssueCommandEnability);
            }
        }
        public bool VoidCommandEnability
        {
            get { return _voidCommandEnability; }
            set
            {
                _voidCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.VoidCommandEnability);
            }
        }
        public bool ConfirmCommandEnability
        {
            get { return _confirmCommandEnability; }
            set
            {
                _confirmCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.ConfirmCommandEnability);
            }
        }
        public bool WaitCommandEnability
        {
            get { return _waitCommandEnability; }
            set
            {
                _waitCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.WaitCommandEnability);
            }
        }
        public bool CancelCommandEnability
        {
            get { return _cancelCommandEnability; }
            set
            {
                _cancelCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.CancelCommandEnability);
            }
        }
        public bool DeleteCommandEnability
        {
            get { return _deleteCommandEnability; }
            set
            {
                _deleteCommandEnability = value;
                RaisePropertyChanged<bool>(() => this.DeleteCommandEnability);
            }
        }
        #endregion

        #endregion

        #region Commands

        public ICommand AddNewTicketCommand
        {
            get
            {
                return _addNewTicketCommand ?? (_addNewTicketCommand = new RelayCommand(ExcuteAddNewTicketCommand));

            }
        }
        private void ExcuteAddNewTicketCommand()
        {
            SelectedTicket = new TicketDTO
            {
                ClientId = 1,
                LocalStatus = TicketStatus.Pending,
                LocalStatusDate = DateTime.Now,
                ServerStatus = TicketStatus.Pending,
                ServerStatusDate = DateTime.Now,
                TypeOfTrip = TypeOfTrips.OneWay,
                RequestedDate = DateTime.Now,
                ROWGUID = Guid.NewGuid().ToString()

            };
            EmptyControlVisibility = true;
            SaveCommandEnability = true;
        }
        public ICommand SaveTicketCommand
        {
            get
            {
                return _saveTicketCommand ?? (_saveTicketCommand = new RelayCommand<Object>(ExcuteSaveTicketCommand, CanSave));
            }
        }
        private void ExcuteSaveTicketCommand(object button)
        {
            try
            {
                var clickedbutton = button as Button;
                if (clickedbutton == null) return;

                var buttenText = clickedbutton.Tag.ToString();
                var isNew = false;

                #region Buttons Switch

                switch (buttenText)
                {
                    case "Post":// will be done by the background worker program
                        //SelectedTicket.LocalStatus = TicketStatus.Posted;
                        //SelectedTicket.LocalStatusDate = DateTime.Now;
                        //Will Be Posted when web post is DONE
                        //SelectedTicket.ServerStatus = TicketStatus.Posted;
                        //SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Re-Post"://"No need of repost, when cancel confirmed by the server the ticket will be deleted from the local pc"
                        //SelectedTicket.LocalStatus = TicketStatus.Posted;
                        //SelectedTicket.LocalStatusDate = DateTime.Now;
                        break;
                    case "Book":
                        SelectedTicket.ServerStatus = TicketStatus.Booked;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Queue":
                        SelectedTicket.ServerStatus = TicketStatus.Queued;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Issue":
                        SelectedTicket.ServerStatus = TicketStatus.Issued;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Void":
                        SelectedTicket.ServerStatus = TicketStatus.Void;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Confirm":
                        SelectedTicket.ServerStatus = TicketStatus.Confirmed;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Wait":
                        SelectedTicket.ServerStatus = TicketStatus.Waiting;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "CancelRequest":
                        SelectedTicket.LocalStatus = TicketStatus.CancelRequest;
                        SelectedTicket.LocalStatusDate = DateTime.Now;
                        break;
                    case "Cancel":
                        SelectedTicket.ServerStatus = TicketStatus.Canceled;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "DeleteRequest":
                        SelectedTicket.ServerStatus = TicketStatus.DeleteRequest;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                    case "Delete":
                        SelectedTicket.ServerStatus = TicketStatus.Deleted;
                        SelectedTicket.ServerStatusDate = DateTime.Now;
                        break;
                }

                #endregion

                SelectedTicket.City = SelectedCity.Display;
                SelectedTicket.AirLines = SelectedAirLine.Display;
                SelectedTicket.Route = SelectedRoute.Display;

                if (Singleton.Edition == MATSEdition.CompactEdition)//Needs Attention on which buttons clicked
                {
                    SelectedTicket.LocalPost = false;
                    SelectedTicket.ServerPost = true;//to show that the client has done an update after a server has done an add/update 
                    //and will not apear in the list of tickets updated from the client side
                }
                else
                {
                    SelectedTicket.ServerPost = false;
                    SelectedTicket.LocalPost = true;//to show that the server has done an update after a client has done an add/update 
                    //and will not apear in the list of tickets updated from the client side
                }

                if (SelectedTicket.Id == 0)
                {
                    _unitOfWork.Repository<TicketDTO>().Insert(SelectedTicket);
                    isNew = true;
                }
                else
                {
                    _unitOfWork.Repository<TicketDTO>().Update(SelectedTicket);
                }

                _unitOfWork.Commit();

                if (isNew)
                    Tickets.Insert(0, SelectedTicket);
            }
            catch
            {
                MessageBox.Show("Can't save ticket, try again...", "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ICommand DeleteTicketCommand
        {
            get
            {
                return _deleteTicketCommand ?? (_deleteTicketCommand = new RelayCommand(ExcuteDeleteTicketCommand));
            }
        }
        private void ExcuteDeleteTicketCommand()
        {
            try
            {
                _unitOfWork.Repository<TicketDTO>().Delete(SelectedTicket);
                _unitOfWork.Commit();
                GetLiveTickets(false);
            }
            catch
            {
                MessageBox.Show("Can't Delete");
            }
        }

        public ICommand RefreshListCommand
        {
            get
            {
                return _refreshListCommand ?? (_refreshListCommand = new RelayCommand(() => GetLiveTickets(false)));
            }
        }

        public void UploadDownloadTicketDetail()//Work on the background 
        {
            //Get tickets from local
        }
        
        public ICommand UploadTicketCommand
        {
            get
            {
                return _uploadTicketCommand ?? (_uploadTicketCommand = new RelayCommand<Object>(ExcuteUploadTicketCommand, CanSave));
            }
        }
        private void ExcuteUploadTicketCommand(object obj)
        {
            var attachment = new Attachments(SelectedTicket);
            attachment.ShowDialog();
        }
        
        public ICommand DownloadTicketCommand
        {
            get
            {
                return _downloadTicketCommand ?? (_downloadTicketCommand = new RelayCommand<Object>(ExcuteDownloadTicketCommand, CanSave));
            }
        }
        private void ExcuteDownloadTicketCommand(object obj)
        {
            var attachment = new Attachments(SelectedTicket);
            attachment.ShowDialog();
        }
        #endregion

        public void GetLiveTickets(bool showOnlyChanged)
        {
            AttachmentList = _unitOfWork.Repository<AttachmentDTO>().GetAll().ToList();

            TicketList = _unitOfWork.Repository<TicketDTO>()
                .GetAllIncludingChilds(c => c.Client, a => a.Attachments, p => p.Payments)
                .OrderBy(s => s.LocalStatus).ToList();

            #region SHOW only changed list
            if (showOnlyChanged)
            {
                TicketList = Singleton.Edition == MATSEdition.CompactEdition 
                    ? TicketList.Where(t => t.ServerPost == false).ToList() 
                    : TicketList.Where(t => t.LocalPost == false).ToList();
            }
            #endregion

            #region By Client

            if (SelectedClient != null && SelectedClient.Id != -1)
            {
                TicketList = TicketList.Where(pi => pi.ClientId == SelectedClient.Id);
            }

            #endregion

            #region By Date
            TicketList = TicketList.Where(pi => pi.RequestedDate.Date >= FilterStartDate.Date
                    && pi.RequestedDate.Date <= FilterEndDate.Date);
            #endregion

            #region By City
            if (SelectedCityForFilter != null && SelectedCityForFilter.Value != -1)
            {
                TicketList = TicketList.Where(pi => pi.City.ToLower().Contains(SelectedCityForFilter.Display.ToLower()));
            }
            #endregion


            TicketStatus[] statuses = { TicketStatus.Pending,TicketStatus.Posted,TicketStatus.Booked,
                                        TicketStatus.Queued,TicketStatus.Issued,TicketStatus.Void,
                                        TicketStatus.Confirmed,TicketStatus.Canceled, TicketStatus.Waiting,
                                        TicketStatus.CancelRequest,TicketStatus.DeleteRequest,TicketStatus.Deleted};

            StatusWithTickets = new ObservableCollection<StatusWithTicket>();
            foreach (TicketStatus stat in statuses)
            {
                var st = new StatusWithTicket
                {
                    Status = stat,
                    TicketList = TicketList.Where(t => t.ServerStatus == stat).ToList()
                };
                if (st.CountLines > 0)
                    StatusWithTickets.Add(st);
            }
            ShowListOf = "All";
            Tickets = new ObservableCollection<TicketDTO>(TicketList);
            _refreshed = true;
            UpdatingText = "";
        }

        private void GetLiveClients()
        {
            var clientList = _unitOfWork.Repository<ClientDTO>()
                .GetAllIncludingChilds(a => a.Address)
                .ToList()
                .Skip(1);
            Clients = new ObservableCollection<ClientDTO>(clientList);

            Clients.Insert(0, new ClientDTO
            {
                DisplayName = "All", 
                Id = -1
            });
        }

        #region Filter List
        private ICommand _filterEndDateChangedCommand, _filterStartDateChangedCommand;
        private IList<ListDataItem> _filterPeriods;
        private ListDataItem _selectedPeriod;
        private string _filterPeriod;
        private string _filterByCity, _filterByPassenger;
        private DateTime _filterStartDate, _filterEndDate;

        public string FilterPeriod
        {
            get { return _filterPeriod; }
            set
            {
                _filterPeriod = value;
                RaisePropertyChanged<string>(() => this.FilterPeriod);
            }
        }
        public DateTime FilterStartDate
        {
            get { return _filterStartDate; }
            set
            {
                _filterStartDate = value;
                RaisePropertyChanged<DateTime>(() => this.FilterStartDate);
            }
        }
        public DateTime FilterEndDate
        {
            get { return _filterEndDate; }
            set
            {
                _filterEndDate = value;
                RaisePropertyChanged<DateTime>(() => this.FilterEndDate);
            }
        }
        private void FillPeriodCombo()
        {
            FilterPeriods = new List<ListDataItem>
            {
                new ListDataItem {Display = "All", Value = 0},
                new ListDataItem {Display = "Today", Value = 1},
                new ListDataItem {Display = "Yesterday", Value = 2},
                new ListDataItem {Display = "This Week", Value = 3},
                new ListDataItem {Display = "Last Week", Value = 4}
            };
        }
        public IList<ListDataItem> FilterPeriods
        {
            get { return _filterPeriods; }
            set
            {
                _filterPeriods = value;
                RaisePropertyChanged<IList<ListDataItem>>(() => this.FilterPeriods);
            }
        }
        public ListDataItem SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                RaisePropertyChanged<ListDataItem>(() => this.SelectedPeriod);
                switch (SelectedPeriod.Value)
                {
                    case 0:
                        FilterStartDate = DateTime.Now.AddDays(-1);
                        FilterEndDate = DateTime.Now.AddMonths(1);
                        break;
                    case 1:
                        FilterStartDate = DateTime.Now;
                        FilterEndDate = DateTime.Now;
                        break;
                    case 2:
                        FilterStartDate = DateTime.Now.AddDays(-1);
                        FilterEndDate = DateTime.Now.AddDays(-1);
                        break;
                    case 3:
                        FilterStartDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                        FilterEndDate = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek - 1);
                        break;
                    case 4:
                        FilterStartDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 7);
                        FilterEndDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 1);
                        break;
                }
            }
        }
        public ICommand FilterEndDateChangedCommand
        {
            get
            {
                return _filterEndDateChangedCommand ?? (_filterEndDateChangedCommand = new RelayCommand(ExcuteDateChangedCommand));
            }
        }
        public ICommand FilterStartDateChangedCommand
        {
            get
            {
                return _filterStartDateChangedCommand ?? (_filterStartDateChangedCommand = new RelayCommand(ExcuteDateChangedCommand));
            }
        }
        private void ExcuteDateChangedCommand()
        {
            GetLiveTickets(false);
            //Tickets = new ObservableCollection<TicketDTO>(TicketList
            //    .Where(pi => pi.RequestedDate.Date >= FilterStartDate.Date && pi.RequestedDate.Date <= FilterEndDate.Date));

            //if (Tickets.Count > 0)
            //{
            //    SelectedTicket = Tickets[0];
            //}
        }

        public string FilterByCity
        {
            get { return _filterByCity; }
            set
            {
                _filterByCity = value;
                RaisePropertyChanged<string>(() => this.FilterByCity);
                GetLiveTickets(false);
            }
        }
        public string FilterByPassenger
        {
            get { return _filterByPassenger; }
            set
            {
                _filterByPassenger = value;
                RaisePropertyChanged<string>(() => this.FilterByPassenger);
                GetLiveTickets(false);
            }

        }
        #endregion

        #region Open List Commands
        private ICommand _cityListViewCommand,
                         _airLineListViewCommand,
                         _routeListViewCommand;

        public ICommand CityListViewCommand
        {
            get
            {
                if (_cityListViewCommand == null)
                {
                    _cityListViewCommand = new RelayCommand(() =>
                    {
                        var listWindow = new Lists(ListTypes.City);
                        listWindow.ShowDialog();
                        if (listWindow.DialogResult != null && (bool)listWindow.DialogResult)
                        {
                            LoadCities();
                            SelectedCity = Cities.FirstOrDefault(c => c.Display.ToUpper().Equals(listWindow.txtDisplayName.Text.ToUpper()));
                        }
                    });
                }
                return _cityListViewCommand;
            }
        }
        public ICommand AirLineListViewCommand
        {
            get
            {
                if (_airLineListViewCommand == null)
                {
                    _airLineListViewCommand = new RelayCommand(() =>
                    {
                        var listWindow = new Lists(ListTypes.AirLine);
                        listWindow.ShowDialog();
                        if (listWindow.DialogResult != null && (bool)listWindow.DialogResult)
                        {
                            LoadAirLines();
                            SelectedAirLine = AirLines.FirstOrDefault(c => c.Display.Equals(listWindow.txtDisplayName.Text));
                        }
                    });
                }
                return _airLineListViewCommand;
            }
        }
        public ICommand RouteListViewCommand
        {
            get
            {
                if (_routeListViewCommand == null)
                {
                    _routeListViewCommand = new RelayCommand(() =>
                    {
                        var listWindow = new Lists(ListTypes.Route);
                        listWindow.ShowDialog();
                        if (listWindow.DialogResult != null && (bool)listWindow.DialogResult)
                        {
                            LoadRoutes();
                            SelectedRoute = Routes.FirstOrDefault(c => c.Display.Equals(listWindow.txtDisplayName.Text));
                        }
                    });
                }
                return _routeListViewCommand;
            }
        }

        public void LoadLists()
        {
            LoadCities();
            LoadAirLines();
            LoadRoutes();
        }

        private ListDataItem _selectedCity;
        public ListDataItem SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                RaisePropertyChanged<ListDataItem>(() => this.SelectedCity);
            }
        }
        private ListDataItem _selectedCityForFilter;
        public ListDataItem SelectedCityForFilter
        {
            get { return _selectedCityForFilter; }
            set
            {
                _selectedCityForFilter = value;
                RaisePropertyChanged<ListDataItem>(() => this.SelectedCityForFilter);
                if (SelectedCityForFilter != null)
                {
                    GetLiveTickets(false);
                }
            }
        }
        private ObservableCollection<ListDataItem> _cities;
        public ObservableCollection<ListDataItem> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                RaisePropertyChanged<ObservableCollection<ListDataItem>>(() => this.Cities);
            }
        }
        private ObservableCollection<ListDataItem> _citiesForFilter;
        public ObservableCollection<ListDataItem> CitiesForFilter
        {
            get { return _citiesForFilter; }
            set
            {
                _citiesForFilter = value;
                RaisePropertyChanged<ObservableCollection<ListDataItem>>(() => this.CitiesForFilter);
            }
        }
        public void LoadCities()
        {
            Cities = new ObservableCollection<ListDataItem>();
            CitiesForFilter = new ObservableCollection<ListDataItem>();
            SelectedCity = new ListDataItem();

            IEnumerable<string> citiesList = _unitOfWork.Repository<ListDTO>()
                .GetAll()
                .Where(l => l.Type == ListTypes.City)
                .Select(l => l.DisplayName).Distinct().ToList();
            IEnumerable<string> citiesList2 = _unitOfWork.Repository<TicketDTO>()
               .GetAll()
               .Select(l => l.City.ToUpper()).Distinct().ToList();
            citiesList = citiesList.Union(citiesList2).Distinct();

            int i = 0;
            foreach (var item in citiesList)
            {
                var dataItem = new ListDataItem
                {
                    Display = item,
                    Value = i
                };
                Cities.Add(dataItem);
                CitiesForFilter.Add(dataItem);
                i++;
            }
            CitiesForFilter.Insert(0, new ListDataItem { Display = "All Cities", Value = -1 });
        }

        private ListDataItem _selectedAirLine;
        public ListDataItem SelectedAirLine
        {
            get { return _selectedAirLine; }
            set
            {
                _selectedAirLine = value;
                RaisePropertyChanged<ListDataItem>(() => this.SelectedAirLine);
            }
        }
        private ObservableCollection<ListDataItem> _airLines;
        public ObservableCollection<ListDataItem> AirLines
        {
            get { return _airLines; }
            set
            {
                _airLines = value;
                RaisePropertyChanged<ObservableCollection<ListDataItem>>(() => this.AirLines);
            }
        }
        public void LoadAirLines()
        {
            AirLines = new ObservableCollection<ListDataItem>();
            SelectedAirLine = new ListDataItem();

            IEnumerable<string> airLinesList = _unitOfWork.Repository<ListDTO>()
                .GetAll()
                .Where(l => l.Type == ListTypes.AirLine)
                .Select(l => l.DisplayName).Distinct().ToList();
            IEnumerable<string> airLinesList2 = _unitOfWork.Repository<TicketDTO>()
            .GetAll()
            .Select(l => l.AirLines).Distinct().ToList();
            airLinesList = airLinesList.Union(airLinesList2).Distinct();
            int i = 0;
            foreach (var item in airLinesList)
            {
                var dataItem = new ListDataItem
                {
                    Display = item,
                    Value = i
                };
                AirLines.Add(dataItem);
                i++;
            }
        }

        private ListDataItem _selectedRoute;
        public ListDataItem SelectedRoute
        {
            get { return _selectedRoute; }
            set
            {
                _selectedRoute = value;
                RaisePropertyChanged<ListDataItem>(() => this.SelectedRoute);
            }
        }
        private ObservableCollection<ListDataItem> _routes;
        public ObservableCollection<ListDataItem> Routes
        {
            get { return _routes; }
            set
            {
                _routes = value;
                RaisePropertyChanged<ObservableCollection<ListDataItem>>(() => this.Routes);
            }
        }
        public void LoadRoutes()
        {
            Routes = new ObservableCollection<ListDataItem>();
            SelectedRoute = new ListDataItem();

            IEnumerable<string> routesList = _unitOfWork.Repository<ListDTO>()
                .GetAll()
                .Where(l => l.Type == ListTypes.Route)
                .Select(l => l.DisplayName).Distinct().ToList();
            IEnumerable<string> routesList2 = _unitOfWork.Repository<TicketDTO>()
                .GetAll()
                .Select(l => l.Route).Distinct().ToList();
            routesList = routesList.Union(routesList2).Distinct();
            int i = 0;
            foreach (var item in routesList)
            {
                var dataItem = new ListDataItem
                {
                    Display = item,
                    Value = i
                };
                Routes.Add(dataItem);
                i++;
            }
        }


        #endregion

        #region Validation
        public static int Errors { get; set; }
        public bool CanSave(object but)
        {
            return Errors == 0;
        }

        #endregion

        #region Status With Tickets
        private ObservableCollection<StatusWithTicket> _statusWithTickets;
        public ObservableCollection<StatusWithTicket> StatusWithTickets
        {
            get { return _statusWithTickets; }
            set
            {
                _statusWithTickets = value;
                RaisePropertyChanged<ObservableCollection<StatusWithTicket>>(() => this.StatusWithTickets);
            }
        }

        private StatusWithTicket _selectedStatusWithTickets;
        public StatusWithTicket SelectedStatusWithTickets
        {
            get { return _selectedStatusWithTickets; }
            set
            {
                _selectedStatusWithTickets = value;
                RaisePropertyChanged<StatusWithTicket>(() => this.SelectedStatusWithTickets);

                try
                {
                    Tickets = new ObservableCollection<TicketDTO>(SelectedStatusWithTickets.TicketList);
                    if (Tickets.Count > 0)
                    {
                        SelectedTicket = Tickets[0];
                        EmptyControlVisibility = true;
                    }
                    else
                    {
                        EmptyControlVisibility = false;
                    }
                    ShowListOf = SelectedStatusWithTickets.Status.ToString();
                }
                catch
                {
                    MessageBox.Show("Error on Selected Status With Tickets");
                }
            }
        }

        private string _showListOf;
        private ICommand _showAllCommand;
        private string _showAllCommandVisibility;
        private ICommand _uploadTicketCommand;
        private ICommand _downloadTicketCommand;

        public string ShowListOf
        {
            get { return _showListOf; }
            set
            {
                _showListOf = value;
                RaisePropertyChanged<string>(() => this.ShowListOf);
                ShowAllCommandVisibility = ShowListOf == "All" ? "Collapsed" : "Visible";
            }
        }
        public ICommand ShowAllCommand
        {
            get
            {
                return _showAllCommand ?? (_showAllCommand = new RelayCommand(ExcuteShowAllCommand));
            }
        }
        private void ExcuteShowAllCommand()
        {
            GetLiveTickets(false);
        }
        public string ShowAllCommandVisibility
        {
            get { return _showAllCommandVisibility; }
            set
            {
                _showAllCommandVisibility = value;
                RaisePropertyChanged<string>(() => this.ShowAllCommandVisibility);
            }
        }
        
        #endregion
    }

}