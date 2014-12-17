using System;
using System.Collections.Generic;
using System.Windows;
using BitcoinMeum.Resources;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.Phone.Controls;

namespace BitcoinMeum
{
    public partial class BalanceScanner : PhoneApplicationPage
    {
        private int _retrieved;

        public dynamic TransactionCount { get; set; }

        public string TotalReceived { get; set; }

        public string TotalSent { get; set; }

        public string Balance { get; set; }

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

                TbAddress.Text = _msg;
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
            TbRetrieved.Text = "Retrieved: " + _retrieved;
            client.DownloadStringCompleted += (sender, e) =>
            {
                dynamic result = JObject.Parse(e.Result);

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

                GetBlockchainBitstampRate();
            };
            client.DownloadStringAsync(new Uri("https://blockchain.info/rawaddr/" + publicAdress + "?format=json"));

        }
        private void CountUsdBalance()
        {
            decimal lastUsdRate = 0;
            decimal walletBalance = 0;
            decimal walletUSDBalance = 0;
            if (decimal.TryParse(TbWalletBalance.Text, out lastUsdRate) && decimal.TryParse(TbWalletBalance.Text, out walletBalance))
            {
                walletUSDBalance = lastUsdRate * walletBalance;
                TbWalletBalanceUsd.Text = "$ " + walletUSDBalance.ToString("#.##");

            }
            else
            {

                //cannot parse rate or walletbalance
            }

        }
        private void GetBlockchainBitstampRate()
        {
            try
            {

                var client = new WebClient();
                client.DownloadStringCompleted += client_DownloadStringCompleted;
                client.DownloadStringAsync(new Uri("https://blockchain.info/ticker"));

            }
            catch (KeyNotFoundException)
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show(AppResources.BalanceRefreshFailed);

            }
            catch (Exception)
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show(AppResources.BalanceRefreshFailedConnection);
            }

        }

        private String _lastUsd;
        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            try
            {
                dynamic result = JObject.Parse(e.Result);
                if (result != null)
                {
                    _lastUsd = result["USD"]["last"].ToString();
                    CountUsdBalance();
                }
                ProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show(AppResources.BalanceRefreshFailed);
            }

        }
    }

}