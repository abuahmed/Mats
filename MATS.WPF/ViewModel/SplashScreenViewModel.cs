using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MATS.Core;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.Repository.Interfaces;
using MATS.WPF.Views;

namespace MATS.WPF.ViewModel
{
    public class SplashScreenViewModel : ViewModelBase
    {
        #region Fields
        private IUnitOfWork _unitOfWork;
        private object _splashWindow;
        bool _login, _activations;
        private string _licensedTo;
        #endregion

        #region Constructor
        public SplashScreenViewModel(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;


            Messenger.Default.Register<object>(this, (message) =>
            {
                SplashWindow = message;
            });
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
        public object SplashWindow
        {
            get { return _splashWindow; }
            set
            {
                _splashWindow = value;
                RaisePropertyChanged<object>(() => SplashWindow);
                if (SplashWindow != null)
                {
                    if (Singleton.Edition == MATSEdition.CompactEdition)
                        CheckActivation();
                    else
                    {
                        new Login().Show();
                        CloseWindow(SplashWindow);
                    }
                }
            }
        }
        public string LicensedTo
        {
            get { return _licensedTo; }
            set
            {
                _licensedTo = value;
                RaisePropertyChanged<string>(() => LicensedTo);
            }
        }
        #endregion

        #region Actions
        private void CheckActivation()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                var activation = _unitOfWork.Repository<ProductActivationDTO>().GetAll().FirstOrDefault();
                if (activation == null)
                {
                    //new Activations().Show();
                    _activations = true;
                }
                else
                {
                    LicensedTo = activation.LicensedTo;
                    Thread.Sleep(1000);//To show the License to whom it belongs
                    if (activation.RegisteredBIOS_SN.Contains(new ProductActivationDTO().BIOS_SN))
                    {
                        //new Login().Show();
                        Singleton.ProductActivation = activation;
                        _login = true;
                    }
                    else
                    {

                        //if (MessageBox.Show(
                        //            "The Product has already been activated on another computer, Do you want to reset the Key",
                        //            "Activation Key Problem", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                        //{
                        //    _unitOfWork.Repository<ProductActivationDTO>().Delete(activation);
                        //    _unitOfWork.Commit();
                        //    //new Activations().Show();
                        //    _activations = true;
                        //}
                        _activations = true;
                    }
                }

            }
            catch
            {
                if (Singleton.Edition == MATS.Core.Enumerations.MATSEdition.ServerEdition)
                    MessageBox.Show("Problem opening oneface, may be the server computer or the network not working properly! try again later..", "Error Opening", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Problem opening oneface! try again later..", "Error Opening", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWindow(SplashWindow);
            }
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_login)
                new Login().Show();
            else if (_activations)
                new Activations().Show();
            CloseWindow(SplashWindow);
        }

        private void CloseWindow(object obj)
        {
            if (obj == null) return;
            var window = obj as Window;
            if (window == null) return;
            //window.DialogResult = true;
            window.Close();
        }
        #endregion

    }
}
