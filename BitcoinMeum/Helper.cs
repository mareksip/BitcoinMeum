using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using BitcoinMeum.Resources;

namespace BitcoinMeum
{
    public class Helper
    {
        public static string CurrentTheme(string iconPath, Visibility v)
        {
            string theme = (v == Visibility.Visible ? "light" : "dark");

            iconPath = theme == "dark" ? iconPath.Replace("Dark", "Light") : iconPath.Replace("Light", "Dark");

            return iconPath;
        }

        public static string RandomQuote()
        {
            string result = "";
            Random rn = new Random();
            int rnd = rn.Next(1, 7);

            switch (rnd)
            {
                case 1 :
                    result = AppResources.Q1;
                    break;
                case 2: result = AppResources.Q2;
                    break;
                case 3: result = AppResources.Q3;
                    break;
                case 4: result = AppResources.Q4;
                    break;
                case 5: result = AppResources.Q5;
                    break;
                case 6: result = AppResources.Q6;
                    break;
                case 7: result = AppResources.Q7;
                    break;
                case 8: result = AppResources.Q8;
                    break;
                default :
                    result = AppResources.Q1;
                    break;
            }
            return result;
        }
    }
}
