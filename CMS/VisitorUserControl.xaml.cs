using CMS.Models;
using CMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CMS
{
    /// <summary>
    /// Interaction logic for VisitorUserControl.xaml
    /// </summary>
    public partial class VisitorUserControl : UserControl
    {
        private XmlDataService<ContentItem> contentService;
        private List<ContentItem> contentItems;
        public VisitorUserControl()
        {
            InitializeComponent();
            contentService = new XmlDataService<ContentItem>("Data/Content.xml");
            LoadContent();
        }
        private void LoadContent()
        {
            contentItems = contentService.LoadAll();
            ContentDataGrid.ItemsSource = contentItems;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (ContentDataGrid.SelectedItem is ContentItem selectedItem)
            {
                VisitorContentDetails detailsWindow = new VisitorContentDetails(selectedItem);
                Window menuWindow = Window.GetWindow(this);
                detailsWindow.Show();
            }
        }
    }
}
