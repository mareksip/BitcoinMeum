using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Linq;
using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Json;


namespace MarketsTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {

            //TODO: Add code to perform your task in background
            if (task is PeriodicTask)
            {

                string tileTitle = "Bitcoin Meum";
                string liveTileEnabled = "False";
                string showBalanceInstead = "False";

                string publicKey = "";
                string balance = "";

                bool containsKey = appSettings.Contains("LiveTileEnabled");
                if (containsKey == true)
                    liveTileEnabled = appSettings["LiveTileEnabled"].ToString();

                bool containsKeyShowBalance = appSettings.Contains("ShowBalance");
                if (containsKeyShowBalance == true)
                    showBalanceInstead = appSettings["ShowBalance"].ToString();

                bool containsKeyPublic = appSettings.Contains("MWPublicKey");
                if (containsKey)
                    publicKey = appSettings["MWPublicKey"].ToString();


                FlipTileData dt = new FlipTileData();
                if (containsKey == true)
                {
                    if (liveTileEnabled == "True")
                    {

                        WebClient client = new WebClient();

                        client.DownloadStringCompleted += (sender, e) =>
                        {
                            dynamic result = JsonValue.Parse(e.Result);
                            tileTitle = result["last"].ToString().Replace(@"\", "").Replace("\"", ""); ;

                        };

                        client.DownloadStringAsync(new Uri("https://www.bitstamp.net/api/ticker/"));
                        System.Threading.Thread.Sleep(5000);




                        if (showBalanceInstead == "True" && publicKey != "")
                        {

                            publicKey = appSettings["MWPublicKey"].ToString();


                            WebClient client2 = new WebClient();
                            client2.DownloadStringCompleted += (sender, e) =>
                            {
                                balance = Markets.DataAccess.MyWallet.BitcoinParse(e.Result);

                            };

                            client2.DownloadStringAsync(new Uri("https://blockchain.info/q/addressbalance/" + appSettings["MWPublicKey"].ToString() + "?format=json"));
                            System.Threading.Thread.Sleep(5000);

                         }
                    }

                }

                ShellTile appTile = ShellTile.ActiveTiles.First();
                dt.Title = "Last: $" + tileTitle;
                if (showBalanceInstead == "True" && liveTileEnabled == "True")
                {
                    dt.Title = balance + " BTC";
                }
               
                if (appTile != null)
                {
                    appTile.Update(dt);
                }
            }

#if DEBUG
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(30));
#endif

            NotifyComplete();
        }



    }
}