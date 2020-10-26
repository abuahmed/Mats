using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.WPF.ViewModel;

namespace MATS.WPF.Views
{
    /// <summary>
    /// Interaction logic for Lists.xaml
    /// </summary>
    public partial class Lists : Window
    {
        public Lists()
        {
            ListViewModel.Errors = 0;
            InitializeComponent();
        }
        public Lists(ListTypes listType)
        {
            ListViewModel.Errors = 0;
            InitializeComponent();
            Messenger.Default.Send<ListTypes>(listType);
            Messenger.Reset();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) ListViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) ListViewModel.Errors -= 1;
        }
    }
}
