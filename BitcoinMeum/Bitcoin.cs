using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Newtonsoft.Json.Linq;
using System.Net;


namespace BitcoinMeum
{
    public class Bitcoin
    {
        static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;

        public class WalletBalance
        {

            public string Balance { get; set; }
            string BalanceUsd { get; set; }
            string TransactionCount { get; set; }
            string TotalSent { get; set; }
            string TotalReceived { get; set; }

            private void TransactionDownload_Completed(object sender, DownloadStringCompletedEventArgs e)
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

                TransactionCount = tempTransactionCount.ToString();
                TotalReceived = totalReceived.ToString();
                TotalSent = totalSent.ToString();
                Balance = finalBalance.ToString();
            }

            public void Retrieve(string publicAdress)
            {
                try
                {
                    var client = new WebClient();
                    //client.DownloadStringCompleted += TransactionDownload_Completed;

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

                        TransactionCount = tempTransactionCount.ToString();
                        TotalReceived = totalReceived.ToString();
                        TotalSent = totalSent.ToString();
                        Balance = finalBalance.ToString();

                    };
                    client.DownloadStringAsync(new Uri("https://blockchain.info/rawaddr/" + publicAdress + "?format=json"));
                }
                catch (KeyNotFoundException)
                {

                }
                catch (Exception)
                {

                }
            }
        }

        public class WalletGenerator
        {
            //static readonly BitcoinSecret Alice = new BitcoinSecret("KyJTjvFpPF6DDX4fnT56d2eATPfxjdUPXFFUb85psnCdh34iyXRQ");
            //private static readonly BitcoinAddress btad = new BitcoinAddress("aa");
            //static private PubKey recipient = new PubKey("1Meum86kuhVe4nNeSJo3fgjutqEUnc3bpB");
            //private string a = new Key().PubKey.GetAddress(Network.Main).ToString();

            //static readonly Transaction AliceFunding = new Transaction()
            //{
            //    Outputs =
            //            {
            //                new TxOut("0.45", Alice.GetAddress()),
            //            }
            //};

            //static readonly Coin[] AliceCoins = AliceFunding
            //                        .Outputs
            //                        .Select((o, i) => new Coin(new OutPoint(AliceFunding.GetHash(), i), o))
            //                        .ToArray();

            //private static readonly TransactionBuilder TxBuilder = new TransactionBuilder();
            ////private static TransactionBuilder tx = TxBuilder.AddCoins(AliceCoins).AddKeys(Alice.Key).Send(, "1.00").SendFees("0.001").SetChange(Alice.GetAddress()).BuildTransaction(true);
            
            ////Assert(txBuilder.Verify(tx));

        }
        public class WalletTransactions : IEquatable<WalletTransactions>
        {
            //tx
            public string TxIndex { get; set; }
            public string BlockHeight { get; set; }
            public string Time { get; set; }
            public string TxRes { get; set; }
            //prev
            public string PrevAddrTag { get; set; }
            public string PrevValue { get; set; }
            public string PrevAddr { get; set; }
            //out
            public string OutValue { get; set; }
            public string OutAddr { get; set; }

            //vypocet
            public Int64 OutputFee { get; set; }
            public String TransactionValue { get; set; }
            public string TextColor { get; set; }
            public string Address { get; set; }
            public string IconSource { get; set; }
            public string UnconfirmedText { get; set; }

            public bool Equals(WalletTransactions other)
            {
                throw new NotImplementedException();

            }
        }
    }
}
