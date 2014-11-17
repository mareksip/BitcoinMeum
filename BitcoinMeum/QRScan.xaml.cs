using System;
using System.Windows;
using Microsoft.Phone.Controls;
using ZXing.Mobile;

namespace BitcoinMeum
{
    public partial class QrScan : PhoneApplicationPage
    {
        private UIElement customOverlayElement = null;
        private MobileBarcodeScanner scanner;
        private int retrieved;

        private string invoker;
        public QrScan()
        {
            InitializeComponent();
            scanner = new MobileBarcodeScanner(this.Dispatcher);


            //Get our UIElement from the MainPage.xaml (this) file 
            // to use as our custom overlay
            if (customOverlayElement == null)
            {
                customOverlayElement = this.customOverlay.Children[0];
                this.customOverlay.Children.RemoveAt(0);
            }

            //Wireup our buttons from the custom overlay


            //Set our custom overlay and enable it
            scanner.CustomOverlay = customOverlayElement;
            scanner.UseCustomOverlay = true;

            //Start scanning
            scanner.Scan().ContinueWith(t =>
            {
                if (t.Result != null)
                    HandleScanResult(t.Result);
            });

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string param;

            if (!NavigationContext.QueryString.TryGetValue("invoker", out param)) return;
            invoker = param;

        }
        private void HandleScanResult(ZXing.Result result)
        {
            if (result == null) return;

            this.Dispatcher.BeginInvoke(() =>
            {
                switch (invoker)
                {
                    case "wallet":
                        NavigationService.Navigate(new Uri(String.Format("/MyWallet.xaml?recipientPublic={0}", result.Text), UriKind.Relative));
                        break;
                    case "walletSettingsPublic":
                        NavigationService.Navigate(new Uri(String.Format("/MyWalletSettings.xaml?walletPublic={0}", result.Text), UriKind.Relative));
                        break;
                    case "walletSettingsPrivate":
                        NavigationService.Navigate(new Uri(String.Format("/MyWalletSettings.xaml?walletPrivate={0}", result.Text), UriKind.Relative));
                        break;
                }

                //Don't allow to navigate back to the scanner with the back button
                NavigationService.RemoveBackEntry();

            });
        }

    }
}