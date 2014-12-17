using System.Linq;
using System.Windows;
using BitcoinMeum.Resources;
using Microsoft.Phone.Controls;
using System.Reflection;

namespace BitcoinMeum
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            FillAboutInformation();
        }

        private void FillAboutInformation()
        {
            System.Xml.Linq.XElement appxml = System.Xml.Linq.XElement.Load("WMAppManifest.xml");
            var appElement = (from manifestData in appxml.Descendants("App") select manifestData).SingleOrDefault();
 
            Author.Text = "Marek Šíp";
            AuthorEmail.Content = "marek.sip@hotmail.com";

            if (appElement != null)
            {
                var xAttribute = appElement.Attribute("Version");
                if (xAttribute != null) Version.Text = xAttribute.Value;
            }
            VersionDescription.Text = AppResources.AboutDescription;
        }

        private void CopyAddressButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(DonateAddressTb.Text);

        }

    }
}