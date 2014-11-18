using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using BitcoinMeum.Resources;


namespace BitcoinMeum
{
    public partial class MyWalletSettings : PhoneApplicationPage
    {
        readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

        public MyWalletSettings()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();

            LoadIss();

        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var btnSaveSettings = new ApplicationBarIconButton
            {
                IconUri = new Uri((@"Assets/Pictures/Dark/appbar.save.png"), UriKind.Relative),
                Text = AppResources.Save
            };
            btnSaveSettings.Click += save_Click;
            ApplicationBar.Buttons.Add(btnSaveSettings);

        }

        private void save_Click(object sender, EventArgs e)
        {
            //save public key
            if (!_appSettings.Contains("MWPublicKey"))
            {
                _appSettings.Add("MWPublicKey", TbPublicKey.Text);
            }
            else
            {
                _appSettings["MWPublicKey"] = TbPublicKey.Text;
            }
            // save private key
            if (!_appSettings.Contains("MWPrivateKey"))
            {
                _appSettings.Add("MWPrivateKey", TbPrivateKey.Text);
            }
            else
            {
                _appSettings["MWPrivateKey"] = TbPrivateKey.Text;
            }

        }

        private void LoadIss()
        {

            if (_appSettings.Contains("MWPublicKey"))
            {
                TbPublicKey.Text = _appSettings["MWPublicKey"].ToString();
            }
            if (_appSettings.Contains("MWPrivateKey"))
            {
                TbPrivateKey.Text = _appSettings["MWPrivateKey"].ToString();
            }


        }

        private void CopyAddressButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(AppResources.DonateAddress);
        }

        private void BtnScanPublic_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/QrScan.xaml?invoker={0}", "walletSettingsPublic"), UriKind.Relative));
        }

        private void BtnScanPrivate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/QrScan.xaml?invoker={0}", "walletSettingsPrivate"), UriKind.Relative));
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string param;
            if (NavigationContext.QueryString.TryGetValue("walletPublic", out param))
            {
                if (!string.IsNullOrEmpty(param))
                {
                    TbPublicKey.Text = param;
                    panorama.DefaultItem = panorama.Items[1];
                }
            }
            if (NavigationContext.QueryString.TryGetValue("walletPrivate", out param))
            {
                if (!string.IsNullOrEmpty(param))
                {
                    TbPrivateKey.Text = param;
                    panorama.DefaultItem = panorama.Items[0];
                }
            }


        }
    }
}