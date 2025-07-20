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
    /// Interaction logic for AdminAdd_Update.xaml
    /// </summary>
    public partial class AdminAdd_Update : Window
    {
        ContentItem selectedItem;
        public AdminAdd_Update(ContentItem selectedItem=null)
        {
            InitializeComponent();

            this.selectedItem = selectedItem;
            //ako je selected item null znamo da smo pozvali dodavanje, prazna polja
            //ako nije null, radimo izmenu, popunjena polja
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
