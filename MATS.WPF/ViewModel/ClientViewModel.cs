using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.Repository.Interfaces;
using System.Windows.Controls;
using MATS.Core;
using MATS.Core.Common;
using MATS.WPF.Views;

namespace MATS.WPF.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        private ICommand _addNewClientCommand, _saveKeyCommand;
        private IUnitOfWork _unitOfWork;
        private ClientDTO _selectedClient;
        private AddressDTO _selectedAddress;
        private ObservableCollection<ClientDTO> _clients;


        public ClientViewModel(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            SelectedAddress = new AddressDTO();
            SelectedClient = new ClientDTO();
            Clients = new ObservableCollection<ClientDTO>();
            GetLiveClients();
            if (Clients.Any())
                SelectedClient = Clients.FirstOrDefault();
            else
                ExcuteAddNewKeyCommand();
        }

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
        public ClientDTO SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                RaisePropertyChanged<ClientDTO>(() => this.SelectedClient);

                if (SelectedClient != null && SelectedClient.Address != null)
                {
                    SelectedAddress = SelectedClient.Address;
                }
                else
                {
                    SelectedAddress = new AddressDTO
                    {
                        Country = "Ethiopia",
                        City = "Addis Abeba"
                    };

                }
            }
        }
        public AddressDTO SelectedAddress
        {
            get { return _selectedAddress; }
            set
            {
                _selectedAddress = value;
                RaisePropertyChanged<AddressDTO>(() => this.SelectedAddress);
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
        #endregion

        #region Commands
        public ICommand AddNewClientCommand
        {
            get
            {
                return _addNewClientCommand ?? (_addNewClientCommand = new RelayCommand(ExcuteAddNewKeyCommand));
            }
        }
        private void ExcuteAddNewKeyCommand()
        {
            SelectedClient = new ClientDTO()
            {
                ClientCode = "C" + (1000 + Clients.Count + 1).ToString().Substring(1),
                NoOfActivations = 0,
                NoOfAllowedPcs = 1,
                ProductKey = getProductKey(),
                ExpiryDuration = 365
            };
        }
        public string getProductKey()
        {
            string productKey = new Random().Next(11111, 99999).ToString();
            productKey = productKey + "-" + new Random().Next(55555, 66666).ToString();
            productKey = productKey + "-" + new Random().Next(33333, 44444).ToString();
            productKey = productKey + "-" + new Random().Next(77777, 88888).ToString();
            return productKey;
        }

        public ICommand SaveClientCommand
        {
            get
            {
                return _saveKeyCommand ?? (_saveKeyCommand = new RelayCommand(ExcuteSaveKeyCommand, CanSave));
            }
        }
        private void ExcuteSaveKeyCommand()
        {
            try
            {
                if (SelectedClient.Id == 0)
                {
                    SelectedClient.Address = SelectedAddress;
                    _unitOfWork.Repository<ClientDTO>().Insert(SelectedClient);
                }
                else
                {
                    SelectedClient.AddressId = SelectedAddress.Id;
                    _unitOfWork.Repository<ClientDTO>().Update(SelectedClient);
                }

                _unitOfWork.Commit();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }

            GetLiveClients();
        }


        private ICommand _sendKeyCommand;
        public ICommand SendKeyCommand
        {
            get
            {
                return _sendKeyCommand ?? (_sendKeyCommand = new RelayCommand(ExcuteSendKeyCommand, CanSave));
            }
        }
        private void ExcuteSendKeyCommand()
        {
            string bodyMe = "Hi," + Environment.NewLine;
            bodyMe = bodyMe + "This Email contains your product key to Activate MATS Software! Keep is Safe!" + Environment.NewLine + Environment.NewLine;
            bodyMe = bodyMe + "Your Product Key is: " + SelectedClient.ProductKey + Environment.NewLine + Environment.NewLine;
            bodyMe = bodyMe + "Bye!" + Environment.NewLine + Environment.NewLine;
            bodyMe = bodyMe + "Regards," + Environment.NewLine;
            bodyMe = bodyMe + "Myco Ticket Office" + Environment.NewLine;
            //bodyMe = bodyMe + "Address: Behind Commerce, Mezid Plaza 702B" + Environment.NewLine;
            //bodyMe = bodyMe + "Tel:+251-933-88-48-55" + Environment.NewLine;
            //bodyMe = bodyMe + "Email:contact@amihanit.com" + Environment.NewLine;
            //bodyMe = bodyMe + "Web:www.amihanit.com";
            var emailDto = new EmailDTO
            {
                Recepient = SelectedClient.Address.PrimaryEmail,
                RecepientName = SelectedClient.DisplayName,
                Subject = "MATS Key --" + DateTime.Now.ToString("dd-MMM-yyyy"),
                Body = bodyMe,
            };
            var sendEmail = new SendEmail(emailDto);
            sendEmail.ShowDialog();
            var dialogueResult = sendEmail.DialogResult;
            if (dialogueResult != null && (bool)dialogueResult)
            {
                //SelectedClient.SentByEmail = true;
                _unitOfWork.Repository<ClientDTO>().Update(SelectedClient);
                _unitOfWork.Commit();
            }
        }
        #endregion


        private void GetLiveClients()
        {
            var clientList = _unitOfWork.Repository<ClientDTO>().GetAllIncludingChilds(a => a.Address).ToList().Skip(1);
            Clients = new ObservableCollection<ClientDTO>(clientList);
        }

        #region Validation
        public static int Errors { get; set; }
        public bool CanSave()
        {
            if (Errors == 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
