using CMS.Models;
using CMS.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace CMS
{
    /// <summary>
    /// Interaction logic for VisitorContentDetails.xaml
    /// </summary>
    public partial class VisitorContentDetails : Window
    {
        ContentItem selectedItem;
        private RtfDataService rtfService;
        public VisitorContentDetails(ContentItem selectedItem)
        {
            InitializeComponent();
            this.selectedItem = selectedItem;
            this.DataContext= selectedItem;
            rtfService = new RtfDataService();
            rtfService.LoadRtfContent(richTextBox, selectedItem.RtfFilePath);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ImagePreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image)
            {
                Window imageWindow = new Window
                {
                    Title = "Image preview",
                    Width = 500,
                    Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new Image
                    {
                        Source = image.Source,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(10)
                    }
                };

                imageWindow.ShowDialog();
            }
        }
    }
}
