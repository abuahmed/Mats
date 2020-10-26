using System.Windows.Controls;
using MATS.WPF.ViewModel;

namespace MATS.WPF.Views
{
    /// <summary>
    /// Interaction logic for Tickets.xaml
    /// </summary>
    public partial class Tickets : UserControl
    {
        public Tickets()
        {
            TicketViewModel.Errors = 0;
            InitializeComponent();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) TicketViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) TicketViewModel.Errors -= 1;
        }

        private void LstTicketsAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstTicketsAutoCompleteBox.SearchText = string.Empty;
        }
    }
}
