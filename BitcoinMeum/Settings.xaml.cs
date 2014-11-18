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
        readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

        public Settings()
        {
            InitializeComponent();
            BuildApplicationBar();

            LoadSettings();
               
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var btnSaveSettings = new ApplicationBarIconButton
            {
                IconUri = new Uri((@"Assets/Pictures/Dark/appbar.save.png"), UriKind.Relative),
                Text = AppResources.Save
            };
            btnSaveSettings.Click += save_Click;
            ApplicationBar.Buttons.Add(btnSaveSettings);
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (!_appSettings.Contains("LiveTileEnabled"))
            {
                _appSettings.Add("LiveTileEnabled", TileSwitch.IsChecked);
            }
            else
            {
                _appSettings["LiveTileEnabled"] = TileSwitch.IsChecked;
            }
            
            if (!_appSettings.Contains("ShowBalance"))
            {
                _appSettings.Add("ShowBalance", CbShowBalance.IsChecked);
            }
            else
            {
                _appSettings["ShowBalance"] = CbShowBalance.IsChecked;
            }
        }

        private void LoadSettings()
        {

            if (!_appSettings.Contains("LiveTileEnabled"))
            {
                TileSwitch.IsChecked = false;
                SpLiveTileSettings.Visibility = Visibility.Collapsed;
            }
            else
            {
                bool livetileEnabled = Boolean.Parse(_appSettings["LiveTileEnabled"].ToString());
                TileSwitch.IsChecked = livetileEnabled;
                if (livetileEnabled == true) {
                    SpLiveTileSettings.Visibility = Visibility.Visible;
                }
                else
                {
                    SpLiveTileSettings.Visibility = Visibility.Collapsed;
                }
               
            }

            if (!_appSettings.Contains("ShowBalance"))
            {
                CbShowBalance.IsChecked = false;
              
            }
            else
            {
                bool livetileEnabled = Boolean.Parse(_appSettings["ShowBalance"].ToString());
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