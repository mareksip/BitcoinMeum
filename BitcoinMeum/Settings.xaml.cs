using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using BitcoinMeum.Resources;

namespace BitcoinMeum
{
    public partial class Settings : PhoneApplicationPage
    {
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        public Settings()
        {
            InitializeComponent();
            BuildApplicationBar();

            LoadSettings();
               
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnSaveSettings = new ApplicationBarIconButton
               ();
            btnSaveSettings.IconUri =
                new Uri((@"Assets/Pictures/Dark/appbar.save.png"), UriKind.Relative);
            btnSaveSettings.Text = AppResources.Save;
            btnSaveSettings.Click += save_Click;
            ApplicationBar.Buttons.Add(btnSaveSettings);
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (!appSettings.Contains("LiveTileEnabled"))
            {
                appSettings.Add("LiveTileEnabled", TileSwitch.IsChecked);
            }
            else
            {
                appSettings["LiveTileEnabled"] = TileSwitch.IsChecked;
            }
            
            if (!appSettings.Contains("ShowBalance"))
            {
                appSettings.Add("ShowBalance", CbShowBalance.IsChecked);
            }
            else
            {
                appSettings["ShowBalance"] = CbShowBalance.IsChecked;
            }
        }

        private void LoadSettings()
        {

            if (!appSettings.Contains("LiveTileEnabled"))
            {
                TileSwitch.IsChecked = false;
                SpLiveTileSettings.Visibility = Visibility.Collapsed;
            }
            else
            {
                bool livetileEnabled = Boolean.Parse(appSettings["LiveTileEnabled"].ToString());
                TileSwitch.IsChecked = livetileEnabled;
                if (livetileEnabled == true) {
                    SpLiveTileSettings.Visibility = Visibility.Visible;
                }
                else
                {
                    SpLiveTileSettings.Visibility = Visibility.Collapsed;
                }
               
            }

            if (!appSettings.Contains("ShowBalance"))
            {
                CbShowBalance.IsChecked = false;
              
            }
            else
            {
                bool livetileEnabled = Boolean.Parse(appSettings["ShowBalance"].ToString());
                CbShowBalance.IsChecked = livetileEnabled;
             }
        }

        private void tileSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
                
            SpLiveTileSettings.Visibility = Visibility.Collapsed;
           
           
        }


        private void tileSwitch_Checked(object sender, RoutedEventArgs e)
        {
            SpLiveTileSettings.Visibility = Visibility.Visible;
        }

        private void CbShowBalance_Click(object sender, RoutedEventArgs e)
        {

        }
   
    }
}