using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MATS.Core;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.Repository.Interfaces;
using MATS.WPF.Views;

namespace MATS.WPF.ViewModel
{
    public class MyMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return (object[])value;
        }
    }

    public class LoginViewModel : ViewModelBase
    {
        #region Fields
        private IUnitOfWork _unitOfWork;
        private UserDTO _user;
        private ICommand _loginCommand;
        private ICommand _closeLoginView;
        #endregion

        #region Constructor
        public LoginViewModel(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            User = new UserDTO {UserName = "mycouser"};//matsuser
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
        public UserDTO User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged<UserDTO>(() => User);
            }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand<object>(ExcuteLoginCommand, CanSave));
            }
        }
        private void ExcuteLoginCommand(object obj)
        {
            var values = (object[])obj;
            var psdBox = values[0] as PasswordBox;
            
            //Do Validation if not handled on the UI
            if (psdBox != null && psdBox.Password == "")
            {
                psdBox.Focus();
                return;
            }

            if (psdBox != null)
            {
                var userPassWord = CommonUtility.Encrypt(psdBox.Password);
            
                var user = _unitOfWork.Repository<UserDTO>()
                    .GetAllIncludingChilds(u=>u.Roles).FirstOrDefault(u => u.UserName == User.UserName && u.Password == userPassWord);
                var role = _unitOfWork.Repository<UsersInRoles>()
                    .GetAllIncludingChilds(u => u.User, r => r.Role).ToList();

                if (user == null)
                {
                    MessageBox.Show("Incorrect Username/Password, try again...", "Error Logging", MessageBoxButton.OK, MessageBoxImage.Error);
                    User.Password = "";
                }
                else
                {
                    Singleton.User = user;
                    switch (user.Status)
                    {
                        case UserTypes.Waiting:
                            new ChangePassword().Show();
                            break;
                        case UserTypes.Active:
                            new MainWindow().Show();
                            break;
                        default:
                            MessageBox.Show("Can't Login you may be blocked or deactivated!");
                            break;
                    }

                    CloseWindow(values[1]);
                }
            }
        }

        public ICommand CloseLoginView
        {
            get
            {
                return _closeLoginView ?? (_closeLoginView = new RelayCommand<Object>(CloseWindow));
            }
        }
        private void CloseWindow(object obj)
        {
            if (obj != null)
            {
                var window = obj as Window;
                if (window != null)
                {
                    window.Close();
                }
            }
        }
        #endregion

        #region Validation
        public static int Errors { get; set; }
        public bool CanSave(object obj)
        {
            if (Errors == 0)
                return true;
            return false;
        }

        #endregion
    }
}
