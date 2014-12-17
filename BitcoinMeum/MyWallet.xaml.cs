using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BitcoinMeum.Resources;
using System.IO.IsolatedStorage;
using ZXing;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using System.Json;

namespace BitcoinMeum
{
    public partial class MyWallet : PhoneApplicationPage
    {
        readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

        public MyWallet()
        {

            InitializeComponent();
            ProgressBar.Visibility = Visibility.Visible;
            BuildLocalizedApplicationBar();

            ParsePublicKey();

            GetBlockchainBitstampRate();
            BlockchainDetails();

            TbSendTransactionFee.Text = "0.0001";

        }

        private void ParsePublicKey()
        {
            if (!_appSettings.Contains("MWPublicKey")) return;
            
            var publicKey = _appSettings["MWPublicKey"].ToString();
            if(publicKey.Length>25)
            {
                var partOne = publicKey.Substring(0, 5);
                //var partTwo = publicKey.Substring(5, publicKey.Length - 8);
                var partThree = publicKey.Substring(publicKey.Length - 4, 4);
                TbPublicFirstPart.Text = partOne + " . . . " + partThree;
            }
        }

        private void CountUsdBalance()
        {
            decimal lastUsdRate = 0;
            decimal walletBalance = 0;
            decimal walletUSDBalance = 0;
            if (decimal.TryParse(_lastUsd.ToString(), out lastUsdRate) && decimal.TryParse(TbWalletBalance.Text.ToString(), out walletBalance))
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
                client.DownloadStringCompleted += BlockchainExchangeRate_DownloadStringCompleted;
                client.DownloadStringAsync(new Uri("https://blockchain.info/ticker"));

            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show(AppResources.BalanceRefreshFailed);
                ProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.BalanceRefreshFailedConnection);
                ProgressBar.Visibility = Visibility.Collapsed;
            }

        }
        String _lastUsd = "";
        private void BlockchainExchangeRate_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            try
            {
                dynamic result = JObject.Parse(e.Result);
                if (result != null)
                {
                    _lastUsd = result["USD"]["last"].ToString();
                }

            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.BalanceRefreshFailed);
            }
        }


        private void RenderRequestQr(string amount, string message)
        {
            if (_appSettings.Contains("MWPublicKey"))
            {
                ImgRequestQr.Source = GenerateQrCode(_appSettings["MWPublicKey"].ToString(), amount, message);
            }
            else
            {
                MessageBox.Show(AppResources.CantCreateQr);
            }
        }
        private static WriteableBitmap GenerateQrCode(string publicKey, string amount, string message)
        {

            var barcodeImage = new WriteableBitmap(1, 1);
            var writer = new BarcodeWriter
            {
                Renderer = new ZXing.Rendering.WriteableBitmapRenderer()
                {

                    //Background = System.Windows.Media.Color.FromArgb(0, 0, 0, 255)


                },
                Format = BarcodeFormat.QR_CODE,
                Options = { Height = 400, Width = 400, Margin = 1 }
            };


            float valueBtc;
            if (float.TryParse(amount, out valueBtc))
            {
                //float valueSatoshi = valueBtc * 100000000;
                var transactionString = publicKey + "?amount=" + valueBtc + "&message=" + message;

                barcodeImage = writer.Write(transactionString);
            }
            else
            {
                if (amount == "")
                {
                    barcodeImage = writer.Write(publicKey);
                }
                else
                {
                    MessageBox.Show(AppResources.MWInvalidRequestAmount);
                }
            }


            return barcodeImage;
        }

        private void BlockchainDetails()
        {

            if (_appSettings.Contains("MWPublicKey"))
            {
                var client = new WebClient();
                client.DownloadStringCompleted += TransactionDownload_Completed;
                client.DownloadStringAsync(
                    new Uri("https://blockchain.info/rawaddr/" + _appSettings["MWPublicKey"] + "?format=json"));
            }
            else
            {
                 ProgressBar.Visibility = Visibility.Collapsed;
            }
            

        }

        private void TransactionDownload_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                dynamic result = JsonObject.Parse(e.Result);
                if (result != null)
                {
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
                }

                var transactions = new List<Bitcoin.WalletTransactions>();
                //txinfo
                var tx_index = "";
                var block_height = "";
                var time = "";
                var txres = "";

                //tx prevout info
                var prev_addr_tag = "";
                var prev_value = "";
                var prev_addr = "";

                var out_value = "";
                var out_addr = "";

                var unconfirmedText = "";

                var transactionType = "";
                bool confirmed = true;

                foreach (var txs in result["txs"])
                {
                    unconfirmedText = "";
                    tx_index = txs["tx_index"].ToString().Replace("\"", "");
                    if (txs.ContainsKey("block_height"))
                    {
                        //confirmed transaction
                        confirmed = true;
                        block_height = txs["block_height"].ToString().Replace("\"", "");
                    }
                    else if (!txs.ContainsKey("block_height"))
                    {
                        //unconfirmed transaction
                        confirmed = false;

                    }

                    time = txs["time"].ToString().Replace("\"", "");
                    txres = txs["result"].ToString().Replace("\"", "");

                    foreach (var inputs in txs["inputs"])
                    {

                        foreach (var prev_out in inputs["prev_out"])
                        {

                            switch ((string)prev_out.Key)
                            {
                                //ouput transaction tag
                                case "addr_tag":
                                    prev_addr_tag = prev_out.Value.ToString().Replace("\"", "");
                                    break;
                                //output value
                                case "value":
                                    prev_value = prev_out.Value.ToString().Replace("\"", "");
                                    break;
                                //transaction is output if
                                //in_addr = prev_out["addr"].ToString().Replace("\"", "");
                                case "addr":
                                    prev_addr = prev_out.Value.ToString().Replace("\"", "");
                                    if (prev_addr == _appSettings["MWPublicKey"].ToString())
                                    {
                                        //wallet output transaction
                                        transactionType = "out";
                                    }
                                    else
                                    {
                                        //wallet input transaction
                                        transactionType = "in";
                                    }
                                    break;

                            }

                        }

                    }
                    foreach (var outt in txs["out"])
                    {
                        switch (transactionType)
                        {
                            case "in":
                                if (outt["addr"].ToString().Replace("\"", "") == _appSettings["MWPublicKey"].ToString())
                                {
                                    //output value to address without fee
                                    out_value = outt["value"].ToString().Replace("\"", "");
                                    //output address
                                    out_addr = outt["addr"].ToString().Replace("\"", "");
                                    //nový addr tag
                                    if (outt.ContainsKey("addr_tag"))
                                        prev_addr_tag = outt["addr_tag"].ToString().Replace("\"", "");

                                }
                                break;
                            case "out":
                                if (outt["addr"].ToString().Replace("\"", "") != _appSettings["MWPublicKey"].ToString())
                                {
                                    //output value to address without fee
                                    out_value = outt["value"].ToString().Replace("\"", "");
                                    //output address
                                    out_addr = outt["addr"].ToString().Replace("\"", "");
                                }
                                break;
                        }
                    }

                    Int64 _outputTransactionFee = 0;
                    float _finalTransValue = 0;
                    string _finalString = "";
                    string _textColor = "";
                    switch (transactionType)
                    {
                        case "in":
                            {
                                //přijatá transakce
                                if (float.TryParse(out_value.ToString(), out _finalTransValue))
                                {
                                    _finalTransValue = _finalTransValue / 100000000;
                                    _finalString = _finalTransValue.ToString("F99").TrimEnd("0".ToCharArray());
                                }
                                _textColor = "#00a6ea";
                                break;
                            }
                        case "out":
                            {
                                //odeslaná transakce
                                var prValue = Int64.Parse(prev_value);
                                Int64 outValue = Int64.Parse(out_value);
                                _outputTransactionFee = prValue - outValue;

                                _finalTransValue = (outValue * -1);
                                _finalTransValue = _finalTransValue / 100000000;
                                _finalString = _finalTransValue.ToString("F99").TrimEnd("0".ToCharArray());

                                _textColor = "#ff5915";

                            }
                            break;
                    }

                    unconfirmedText = confirmed ? "" : "Unconfirmed transaction!";

                    var addressTemp = "";
                    var iconSource = @"Assets/Pictures/Dark/transaction-in.png";
                    if (transactionType == "out")
                    {
                        addressTemp = out_addr;
                        iconSource = @"Assets/Pictures/Dark/transaction-out.png";
                    }
                    else
                    {
                        addressTemp = prev_addr;
                    }
                    transactions.Add(new Bitcoin.WalletTransactions()
                    {
                        TxIndex = tx_index,
                        BlockHeight = block_height,
                        Time = time,
                        TxRes = txres,
                        PrevAddrTag = prev_addr_tag,
                        PrevValue = prev_value,
                        PrevAddr = prev_addr,
                        OutValue = out_value,
                        OutAddr = out_addr,
                        UnconfirmedText = unconfirmedText,
                        TransactionValue = _finalString,
                        OutputFee = _outputTransactionFee,
                        TextColor = _textColor,
                        Address = addressTemp,
                        IconSource = iconSource

                    });
                    prev_addr_tag = "";
                    transactionType = "";

                }
                TransactionList.ItemsSource = transactions;
                CountUsdBalance();
                ProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void BuildLocalizedApplicationBar()
        {

            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            var btnBalance = new ApplicationBarIconButton
            {
                IconUri = new Uri(@"Assets/Pictures/Dark/appbar.refresh.png", UriKind.Relative),
                Text = AppResources.MWRefreshBalance,

            };
            btnBalance.Click += refresh_Click;

            var walletSettingsAppBar =
                new ApplicationBarMenuItem
                {
                    Text = AppResources.MWSettingsRef
                };

            walletSettingsAppBar.Click += walletSettingsAppBar_Click;
            ApplicationBar.Buttons.Add(btnBalance);
            ApplicationBar.MenuItems.Add(walletSettingsAppBar);


        }

        private void refresh_Click(object sender, EventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;

            GetBlockchainBitstampRate();
            BlockchainDetails();
            CountUsdBalance();
        }

        private void walletSettingsAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MyWalletSettings.xaml", UriKind.Relative));

        }

        private void BtnRefreshRequsetQR_Click(object sender, RoutedEventArgs e)
        {
            RenderRequestQr(TbTransactionAmount.Text, TbTransactionMessage.Text);
        }

        private void TbTransactionPublicHold_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //TODO: copy transaction details on HOLD
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {

            //replace GUID with private key
            //amount in satoshi

            var guid = "";
            if (_appSettings.Contains("MWPrivateKey"))
            {
                guid = _appSettings["MWPrivateKey"].ToString();
            }
            else
            {
                MessageBox.Show(AppResources.MWSendPrivateKeyError);
                return;
            }

            var recipient = TbRecipient.Text;
            if (string.IsNullOrEmpty(recipient))
            {
                MessageBox.Show(AppResources.MWSendRecipientError);
                return;
            }

            var amount = Markets.DataAccess.MyWallet.ValidateAmount(TbSendTransactionValue.Text); //balance 24922
            if (amount == "0")
            {
                MessageBox.Show(AppResources.MWSendAmountError);
                return;
                
            }
            amount = Markets.DataAccess.MyWallet.SatoshiParse(amount);

            string fee = Markets.DataAccess.MyWallet.SatoshiParse(TbSendTransactionFee.Text);
            var note = TbSendTransactionMessage.Text;
            var uri = "https://blockchain.info/merchant/" + guid +
                               "/payment?to=" + recipient + "&amount=" + amount + "&fee=" + fee + "&note=" + note;
           
            var confirmationMsg = "Really send " + TbSendTransactionValue.Text + " BTC" + " to " + TbRecipient.Text +" ? " + " Fee: "+ TbSendTransactionFee.Text + " BTC";
            var mr = MessageBox.Show(confirmationMsg, "Confirmation", MessageBoxButton.OKCancel);

            switch (mr)
            {
                case MessageBoxResult.OK:
                    try
                    {

                        var client = new WebClient();
                        client.DownloadStringCompleted += SendDownload_Completed;
                        client.DownloadStringAsync(new Uri(uri));

                    }
                    catch (KeyNotFoundException)
                    {

                    }
                    catch (Exception)
                    {

                    }
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show(AppResources.MWSendCancel);
                    break;
            }

        }
        private static void SendDownload_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            dynamic result = JObject.Parse(e.Result);
            if (result != null)
            {
                var msg = "";
                if (result.ToString().Contains("error"))
                {
                    msg = result["error"].ToString().Replace("\"", "");
                }
                else
                {
                    msg = AppResources.MWSendBlockchainResponse + @"\n" + result["message"].ToString().Replace("\"", "");
                }
                MessageBox.Show(msg);
            }
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string param;

            if (!NavigationContext.QueryString.TryGetValue("recipientPublic", out param)) return;
            string recipientPublic = param;
            if (!string.IsNullOrEmpty(recipientPublic))
            {
                TbRecipient.Text = param;
                Panorama.DefaultItem = Panorama.Items[2];

            }

        }
        private void BtnScanRecipient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/QrScan.xaml?invoker={0}", "wallet"), UriKind.Relative));                      
       
        }

    }
}