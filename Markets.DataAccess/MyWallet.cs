using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markets.DataAccess
{
    public class MyWallet
    {
        public static string SatoshiParse(string inputBitcoin)
        {
            decimal result = 0;

            if(decimal.TryParse(inputBitcoin,out result))
            {
                result = result * 100000000;

            }
           
            return result.ToString("0");
        }
        public static string BitcoinParse(string inputSatoshi)
        {
            decimal result = 0;

            if (decimal.TryParse(inputSatoshi, out result))
            {
                result = result / 100000000;

            }

            return result.ToString();
        }
        public static string ValidateAmount(string input)
        {
            decimal result = 0;

            if (decimal.TryParse(input, out result))
            {

            }

            return result.ToString();
        }
    }
}
