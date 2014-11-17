using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;

namespace BitcoinMeum
{
    class RssFeed 
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}
