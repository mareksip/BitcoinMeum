using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BitcoinMeum.Resources;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Scheduler;

namespace BitcoinMeum
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {


            InitializeComponent();

            BuildLocalizedApplicationBar();

            RegisterAgent();

            TbQuote.Text = Helper.RandomQuote();

        }


        private static void RegisterAgent()
        {
            const string taskName = "MarketsTask";
            try
            {

                if (ScheduledActionService.Find(taskName) != null)
                {
                    //if the agent exists, remove and then add it to ensure
                    //the agent's schedule is updated to avoid expiration
                    ScheduledActionService.Remove(taskName);
                }

                var periodicTask = new PeriodicTask(taskName) {Description = "MarketsTask"};
                ScheduledActionService.Add(periodicTask);
    
                //#if DEBUG

                //ScheduledActionService.LaunchForTest(taskName, TimeSpan.FromSeconds(60));

                //#else

                //#endif

                //zakomentováno error: "BNS Error: API can only be called from deployed app"
                //ScheduledActionService.LaunchForTest(taskName, TimeSpan.FromSeconds(0));
               
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (SchedulerServiceException schedulerException)
            {
                MessageBox.Show(schedulerException.Message);
            }
        }


        private void FirstLaunchCheck()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("firstLaunchx"))
            {
                // first launch

                IsolatedStorageSettings.ApplicationSettings["firstLaunch"] = true;
                IsolatedStorageSettings.ApplicationSettings.Save();
                NavigationService.Navigate(new Uri("/News.xaml", UriKind.Relative));
            }
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            var aboutAppBar =
                new ApplicationBarMenuItem();

            var settingsItem = new ApplicationBarMenuItem {Text = AppResources.Settings};
            settingsItem.Click +=settingsItem_Click;

            aboutAppBar.Text = AppResources.AboutTitle;
            aboutAppBar.Click += aboutAppBar_Click;
            ApplicationBar.MenuItems.Add(aboutAppBar);
            ApplicationBar.MenuItems.Add(settingsItem);
            ApplicationBar.Mode = ApplicationBarMode.Minimized;

        }

        private void settingsItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void aboutAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void BtnBalanceScanner_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BalanceScanner.xaml", UriKind.Relative));
        }

        private void BtnMyWallet_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MyWallet.xaml", UriKind.Relative));
        }

        private void BtnNews_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/News.xaml", UriKind.Relative));
        }

        private void BtnMarkets_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Markets.xaml?msg=" + "test", UriKind.Relative));
        }
    }
}