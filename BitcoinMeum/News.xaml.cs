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
            var rssClient = new WebClient();
            rssClient.DownloadStringCompleted += RSSClientReddit_DownloadStringCompleted;
            rssClient.DownloadStringAsync(new Uri("http://www.reddit.com/r/bitcoin/.rss"));
        }

        private void RSSClientReddit_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rssData = from rss in XElement.Parse(e.Result).Descendants("item")
                let xElement = rss.Element("title").ToString()
                          where !xElement.Contains("/r/Bitcoin FAQ - Newcomers please read")
                select new RssFeed
                          {
                              Title = rss.Element("title").Value,
                              Date = rss.Element("pubDate").Value,
                              //Description = rss.Element("description").Value, commented until RSS parsing is completed
                              Link = rss.Element("guid").Value

                          };
            ListFeedReddit.ItemsSource = rssData;
            
        }

        private void LoadCoinDesk()
        {
            var rssClient = new WebClient();
            rssClient.DownloadStringCompleted += RSSClientCoinDesk_DownloadStringCompleted;
            rssClient.DownloadStringAsync(new Uri("http://feeds.feedburner.com/CoinDesk?format=xml"));
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