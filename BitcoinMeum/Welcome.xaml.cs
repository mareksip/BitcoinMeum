using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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