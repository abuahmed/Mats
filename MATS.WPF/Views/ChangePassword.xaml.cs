using System.Windows;
using System.Windows.Controls;
using MATS.WPF.ViewModel;

namespace MATS.WPF.Views
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            ChangePasswordViewModel.Errors = 0;
            InitializeComponent();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) ChangePasswordViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) ChangePasswordViewModel.Errors -= 1;
        }
    }
}
