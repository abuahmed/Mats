using System;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MATS.OA;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MATS.Core;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.Repository.Interfaces;
using MATS.WPF.Views;


namespace MATS.WPF.ViewModel
{
    public class ActivationViewModel : ViewModelBase
    {
        #region Fields
        //readonly MATSServerDbContext _dbContext;
        private IUnitOfWork _unitOfWork;
        private string _productKey;
        private ICommand _activateCommand;
        private ProductActivationDTO _productActivation;
        //private const string ConnectionStringName = @"Data Source=.;Initial Catalog=MatsDb;User ID=sa;pwd=amihan; Connect Timeout=2000; Pooling='true'; Max Pool Size=200";

        #endregion

        #region Constructor
        public ActivationViewModel(IUnitOfWork unitOfWork,IMATSServerDbContextUnitOfWork matsUnitofWork)
        {
            //_dbContext = new MATSServerDbContext(ConnectionStringName);
            MATSUnitOfWork = matsUnitofWork;
            UnitOfWork = unitOfWork;

            ProductActivation = _unitOfWork.Repository<ProductActivationDTO>().GetAll().FirstOrDefault() ??
                                new ProductActivationDTO();

            ProgressBarVisibility = "Collapsed";
            CommandsEnability = true;
        } 
        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
            set
            {
                _unitOfWork = value;
                RaisePropertyChanged<IUnitOfWork>(() => UnitOfWork);
            }
        }

        private IMATSServerDbContextUnitOfWork _mATSUnitOfWork;
        public IMATSServerDbContextUnitOfWork MATSUnitOfWork
        {
            get { return _mATSUnitOfWork; }
            set
            {
                _mATSUnitOfWork = value;
                RaisePropertyChanged<IMATSServerDbContextUnitOfWork>(() => this.MATSUnitOfWork);
            }
        }
        
        public string ProductKey
        {
            get { return _productKey; }
            set
            {
                _productKey = value;
                RaisePropertyChanged<string>(() => ProductKey);
            }
        }
        public ProductActivationDTO ProductActivation
        {
            get { return _productActivation; }
            set
            {
                _productActivation = value;
                RaisePropertyChanged<ProductActivationDTO>(() => ProductActivation);
            }
        }

        private string _progressBarVisibility;
        public string ProgressBarVisibility
        {
            get { return _progressBarVisibility; }
            set
            {
                _progressBarVisibility = value;
                RaisePropertyChanged<string>(() => ProgressBarVisibility);
            }
        }

        private bool _commandsEnability;
        public bool CommandsEnability
        {
            get { return _commandsEnability; }
            set
            {
                _commandsEnability = value;
                RaisePropertyChanged<bool>(() => this.CommandsEnability);
            }
        } 
        #endregion

        #region Commands
        object _obj; bool _login;
        public ICommand ActivateCommand
        {
            get
            {
                return _activateCommand ?? (_activateCommand = new RelayCommand<Object>(ExcuteActivateCommand));
            }
        }
        private void ExcuteActivateCommand(object windowObject)
        {
            _obj = windowObject;
            ProgressBarVisibility = "Visible";
            CommandsEnability = false;
            var worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarVisibility = "Collapsed";
            CommandsEnability = false;
            if (!_login) return;
            new Login().Show();
            CloseWindow(_obj);
        }
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            ProductActivation.ProductKey = ProductKey;
            var client =MATSUnitOfWork.Clients.FirstOrDefault(a => a.ProductKey == ProductActivation.ProductKey
                && a.ClientStatus == 0 );
            if (client != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(client.BIOS_SN))
                    {
                        client.BIOS_SN = ProductActivation.BIOS_SN;
                        client.ActivationDate = DateTime.Now; //the time will be better if it is the server timer
                        client.ExpirationDate = client.ActivationDate.Value.AddDays(client.ExpiryDuration);
                    }
                    else
                    {
                        if (!client.BIOS_SN.Contains(ProductActivation.BIOS_SN))
                        {
                            if (client.NoOfAllowedPcs == 1)
                            {
                                MessageBox.Show(
                                    "Can't Activate the product, check your product key and try again, or contact mats office!");
                                ProductKey = "";
                                CommandsEnability = true;
                                return;
                            }

                            client.BIOS_SN = client.BIOS_SN + "," + ProductActivation.BIOS_SN;
                            if (client.BIOS_SN.Split(',').Count() > client.NoOfAllowedPcs)
                            {
                                MessageBox.Show(
                                    "Can't Activate the product, check your product key and try again, or contact mats office!");
                                ProductKey = "";
                                CommandsEnability = true;
                                return;
                            }
                        }
                    }
                    client.NoOfActivations = client.NoOfActivations + 1;

                    MATSUnitOfWork.Add(client);
                    MATSUnitOfWork.SaveChanges();

                    ProductActivation.RegisteredBIOS_SN = client.BIOS_SN;
                    ProductActivation.LicensedTo = client.DisplayName;
                    
                    var clt = new ClientDTO
                    {
                        Id = 1,
                        ClientCode = client.ClientCode,
                        ProductKey = client.ProductKey,
                        DisplayName = client.DisplayName,
                        ContactName = client.ContactName,
                        NoOfActivations = client.NoOfActivations,
                        NoOfAllowedPcs = client.NoOfAllowedPcs,
                        ExpiryDuration = client.ExpiryDuration,
                        ContactTitle = client.ContactTitle,
                        ClientStatus = ClientStatus.Active,
                        Address = new AddressDTO
                        {
                            Country = "Ethiopia",
                            City = "Addis Abeba",
                            Mobile = "0911111111",
                            PrimaryEmail = "default@yahoo.com"
                        }
                    };
                    
                    ProductActivation.ClientId = client.Id;

                    ProductActivation.DateLastModified = DateTime.Now;
                    ProductActivation.ModifiedByUserId = 1;
                    ProductActivation.CreatedByUserId = 1;
                    ProductActivation.DateRecordCreated = DateTime.Now;


                    if (ProductActivation.Id == 0)
                    {
                        ProductActivation.ActivatedDate = DateTime.Now;
                        ProductActivation.ExpirationDate = DateTime.Now.AddDays(client.ExpiryDuration);

                        _unitOfWork.Repository<ProductActivationDTO>().Insert(ProductActivation);
                        _unitOfWork.Repository<ClientDTO>().Insert(clt);
                    }
                    else
                    {
                        _unitOfWork.Repository<ProductActivationDTO>().Update(ProductActivation);
                        _unitOfWork.Repository<ClientDTO>().Update(clt);
                    }

                    _unitOfWork.Commit();

                    Singleton.ProductActivation = ProductActivation;
                    //new Login().Show();
                    _login = true;

                }
                //catch (DbEntityValidationException ee)
                //{
                //    foreach (var eve in ee.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
                catch
                {
                    MessageBox.Show("Error:" + Environment.NewLine + " There may be no Internet connection." + Environment.NewLine + "Check your connection and try again.");
                    CommandsEnability = true;
                }
            }
            else
            {
                MessageBox.Show("Can't Activate the product, check your product key and try again, or contact oneface office!");
                ProductKey = "";
                CommandsEnability = true;
            }
        }


        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand<Object>(CloseWindow));
            }
        }

        private void CloseWindow(object obj)
        {
            if (obj == null) return;
            var window = obj as Window;
            if (window != null)
            {
                window.Close();
            }
        } 
        #endregion
        
    }
}
