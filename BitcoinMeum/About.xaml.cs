using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.ApplicationModel;
using System.Reflection;

namespace BitcoinMeum
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            FillAboutInformation();
        }

        private void FillAboutInformation()
        {
            var helper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            Author.Text = "Marek Šíp";
            AuthorEmail.Content = "marek.sip@hotmail.com";
            Version.Text = helper.Version.ToString();
        }

        private void CopyAddressButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(DonateAddressTb.Text);

        }
    }
}