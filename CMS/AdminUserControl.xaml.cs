using CMS.Models;
using CMS.Services;
using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace CMS
{
    /// <summary>
    /// Interaction logic for AdminUserControl.xaml
    /// </summary>
    public partial class AdminUserControl : UserControl
    {
        private XmlDataService<ContentItem> contentService;
        private List<ContentItem> contentItems;
        public AdminUserControl()
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        { 
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
