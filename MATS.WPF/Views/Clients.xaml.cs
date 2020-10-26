using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MATS.WPF.ViewModel;

namespace MATS.WPF.Views
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : Window
    {
        public Clients()
        {
            ClientViewModel.Errors = 0;
            InitializeComponent();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) ClientViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) ClientViewModel.Errors -= 1;
        }
    }
}
