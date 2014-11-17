using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BitcoinMeum.Resources;
using Newtonsoft.Json.Linq;


namespace BitcoinMeum
{
    public partial class WalletGenerator : PhoneApplicationPage
    {
        public WalletGenerator()
        {
            InitializeComponent();
            BuildApplicationBar();
            RefreshAllRates();
          
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void RefreshAllRates()
        {
            try
            {
                //bitstamp
                var client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(bitstampRefresh_Completed);
                client.DownloadStringAsync(new Uri("https://www.bitstamp.net/api/ticker/"));


                //btce
                var btceClient = new WebClient();
                btceClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(btceRefresh_Completed);
                btceClient.DownloadStringAsync(new Uri("https://btc-e.com/api/3/ticker/btc_usd"));

            }
            catch (KeyNotFoundException)
            {

            }
            catch (Exception)
            {

            }


        }

        private void btceRefresh_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            var jo = JObject.Parse(e.Result);

           
            TbBtceHigh.Text = (string)jo["btc_usd"]["high"] + " USD";
            TbBtceLast.Text = (string)jo["btc_usd"]["last"] + " USD";
            TbBtceLow.Text = (string)jo["btc_usd"]["low"] + " USD";
            TbBtceTimestamp.Text = DateTime.Now.ToString();
            TbBtceVolume.Text = (string)jo["btc_usd"]["vol_cur"] + " BTC";
        }

        private void bitstampRefresh_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            dynamic bitstampRates = JObject.Parse(e.Result);

            TbBitstampLast.Text = bitstampRates["last"].ToString() + " USD";
            TbBitstampHigh.Text = bitstampRates["high"].ToString() + " USD";
            TbBitstampLow.Text = bitstampRates["low"].ToString()+ " USD";
            double bitstampVol = 0;
            if(double.TryParse(bitstampRates["volume"].ToString(),out bitstampVol))
            {
                
            }
           
            TbBitstampVolume.Text = bitstampVol.ToString("#.##") + " BTC";
            TbBitstampTimestamp.Text = DateTime.Now.ToString();

        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var btnBalance = new ApplicationBarIconButton
            {
                IconUri = new Uri(@"Assets/Pictures/Dark/appbar.refresh.png", UriKind.Relative),
                Text = AppResources.MWRefreshBalance
            };
            btnBalance.Click += refresh_Click;

            ApplicationBar.Buttons.Add(btnBalance);
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            RefreshAllRates();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
            {

            }

        }

    }
}