using System;
using System.Linq;
using System.Net;
using Microsoft.Phone.Controls;
using System.Xml.Linq;

namespace BitcoinMeum
{
    public partial class News : PhoneApplicationPage
    {
        public News()
        {
            InitializeComponent();
            LoadCoinDesk();
            LoadReddit();
            
        }

        private void LoadReddit()
        {
            WebClient RSSClient = new WebClient();
            RSSClient.DownloadStringCompleted += RSSClientReddit_DownloadStringCompleted;
            RSSClient.DownloadStringAsync(new Uri("http://www.reddit.com/r/bitcoin/.rss"));
        }

        private void RSSClientReddit_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rssData = from rss in XElement.Parse(e.Result).Descendants("item")
                          select new RssFeed
                          {
                              Title = rss.Element("title").Value,
                              Date = rss.Element("pubDate").Value,
                              Description = rss.Element("description").Value,
                              Link = rss.Element("guid").Value

                          };
            ListFeedReddit.ItemsSource = rssData;
            
        }

        private void LoadCoinDesk()
        {
            var RSSClient = new WebClient();
            RSSClient.DownloadStringCompleted += RSSClientCoinDesk_DownloadStringCompleted;
            RSSClient.DownloadStringAsync(new Uri("http://feeds.feedburner.com/CoinDesk?format=xml"));
        }

        private void RSSClientCoinDesk_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rssData = from rss in XElement.Parse(e.Result).Descendants("item")
                          select new RssFeed
                          {
                              Title = rss.Element("title").Value,
                              Date = rss.Element("pubDate").Value,
                              Description = rss.Element("description").Value,
                              Link = rss.Element("guid").Value

                          };
            ListFeed.ItemsSource = rssData;

        }
       
    }
}