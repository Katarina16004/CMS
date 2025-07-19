using CMS.Models;
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
using System.Windows.Shapes;

namespace CMS
{
    /// <summary>
    /// Interaction logic for VisitorContentDetails.xaml
    /// </summary>
    public partial class VisitorContentDetails : Window
    {
        ContentItem selectedItem;
        public VisitorContentDetails(ContentItem selectedItem)
        {
            this.selectedItem = selectedItem;
            InitializeComponent();
        }
    }
}
