using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace BitcoinMeum
{
    public partial class Welcome : PhoneApplicationPage
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute)); 
        }

        private void BtnBitcoinGuide_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}