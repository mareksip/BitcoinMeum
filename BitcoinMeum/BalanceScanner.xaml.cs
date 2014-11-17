using System;
using System.Json;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using ZXing.Mobile;

namespace BitcoinMeum
{
    public partial class BalanceScanner : PhoneApplicationPage
    {
        private UIElement customOverlayElement = null;
        private MobileBarcodeScanner scanner;
        private int retrieved;
        public BalanceScanner()
        {
            InitializeComponent();
            retrieved = 0;
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
            string msg;
            //this.Dispatcher.BeginInvoke(() => GetBalance("1HrAm84oEr1eUEojmUwmcYeYdCwnea7QwQ ")); //test query
       
            if (!NavigationContext.QueryString.TryGetValue("msg", out msg)) return;
            this.Dispatcher.BeginInvoke(() => GetBalance(msg));
       
            var name = msg;

        }
        private void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                msg = result.Text;
                this.Dispatcher.BeginInvoke(() => GetBalance(msg));

            }
            else
                msg = "Scanning Canceled!";

            this.Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show(msg);

                //Go back to the main page
                if (result != null)
                    NavigationService.Navigate(new Uri(String.Format("/BalanceScanner.xaml?msg={0}",result.Text), UriKind.Relative));

                //Don't allow to navigate back to the scanner with the back button
                NavigationService.RemoveBackEntry();

            });
        }

        public void GetBalance(string publicAdress)
        {
            var client = new WebClient();
            //client.DownloadStringCompleted += TransactionDownload_Completed;
            retrieved++;
            TbRetrieved.Text = "Retrieved: " + retrieved.ToString();
            client.DownloadStringCompleted += (sender, e) =>
            {
                dynamic result = JsonValue.Parse(e.Result);
             
                if (result == null) return;
               
                var tempTransactionCount = result["n_tx"].ToString();
                var tempTotalSent = result["total_sent"].ToString();
                var tempTotalReceived = result["total_received"].ToString();
                var tempFinalBalance = result["final_balance"].ToString();

                float totalSent = float.Parse(tempTotalSent) / 100000000;
                float totalReceived = float.Parse(tempTotalReceived) / 100000000;
                float finalBalance = float.Parse(tempFinalBalance) / 100000000;

                TbTransactionCount.Text = tempTransactionCount.ToString();
                TbTotalReceived.Text = totalReceived.ToString();
                TbTotalSent.Text = totalSent.ToString();

                TbWalletBalance.Text = finalBalance.ToString();

            };
            client.DownloadStringAsync(new Uri("https://blockchain.info/rawaddr/" + publicAdress + "?format=json"));
                
        }

        public dynamic TransactionCount { get; set; }

        public string TotalReceived { get; set; }

        public string TotalSent { get; set; }

        public string Balance { get; set; }
    }
 

  
}