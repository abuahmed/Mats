using GalaSoft.MvvmLight.Messaging;
using MATS.Core.Models;
using MATS.WPF.ViewModel;
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

namespace MATS.WPF.Views
{
    /// <summary>
    /// Interaction logic for Attachments.xaml
    /// </summary>
    public partial class Attachments : Window
    {
        public Attachments()
        {
            AttachmentViewModel.Errors = 0;
            InitializeComponent();
        }
        public Attachments(TicketDTO ticket)
        {
            AttachmentViewModel.Errors = 0;
            InitializeComponent();
            Messenger.Default.Send<TicketDTO>(ticket);
            Messenger.Reset();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) AttachmentViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) AttachmentViewModel.Errors -= 1;
        }
    }
}
