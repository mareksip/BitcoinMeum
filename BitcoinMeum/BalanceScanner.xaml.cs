using System;
using System.Json;
using System.Net;
using Microsoft.Phone.Controls;

namespace BitcoinMeum
{
    public partial class BalanceScanner : PhoneApplicationPage
    {
        private int _retrieved;

        string _msg;
        public BalanceScanner()
        {
            InitializeComponent();
            _retrieved = 0;
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
            //this.Dispatcher.BeginInvoke(() => GetBalance("1HrAm84oEr1eUEojmUwmcYeYdCwnea7QwQ")); //test query

            if (NavigationContext.QueryString.TryGetValue("msg", out _msg))
            {
                this.Dispatcher.BeginInvoke(() => GetBalance(_msg));
            }
            else
            {

                NavigationService.Navigate(new Uri(String.Format("/QrScan.xaml?invoker={0}", "balance"),
                    UriKind.Relative));
            }
        }
      
        public void GetBalance(string publicAdress)
        {
            var client = new WebClient();
            //client.DownloadStringCompleted += TransactionDownload_Completed;
            _retrieved++;
            TbRetrieved.Text = "Retrieved: " + _retrieved.ToString();
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