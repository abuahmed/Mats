using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace MATS.WPF.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void wdwSpashScreen_Loaded(object sender, RoutedEventArgs e)
        {            
            Messenger.Default.Send<object>(sender);
            Messenger.Reset();
        }
               
    }
}
