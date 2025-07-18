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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Notification.Wpf;
using Notification.Wpf.Controls;
using System;

namespace CMS
{
    /// <summary>
    /// Interaction logic for AdminUserControl.xaml
    /// </summary>
    public partial class AdminUserControl : UserControl
    {
        private XmlDataService<ContentItem> contentService;
        private ObservableCollection<ContentItem> items;
        private readonly NotificationManager _notificationManager = new NotificationManager();

        public AdminUserControl()
        {
            InitializeComponent();
            contentService = new XmlDataService<ContentItem>("Data/Content.xml");
            LoadContent();
        }
        public void ShowNotification(string title,string message, NotificationType type= NotificationType.Error)
        {
            _notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            },expirationTime: TimeSpan.FromSeconds(3));
        }
        private void LoadContent()
        {
            var loadedItems = contentService.LoadAll();
            items = new ObservableCollection<ContentItem>(loadedItems);
            items.CollectionChanged += Items_CollectionChanged;

            foreach (var item in items)
                item.PropertyChanged += Item_PropertyChanged;

            ContentDataGrid.ItemsSource = items;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContentItem.IsSelected))
            {
                UpdateDeleteButtonState();
            }
        }
        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ContentItem item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (ContentItem item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
        }

        private void UpdateDeleteButtonState()
        {
             DeleteButton.IsEnabled = items.Any(i => i.IsSelected);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = items.Where(i => i.IsSelected).ToList();
            if (selectedItems.Count == 0)
                return;
            var result = MessageBox.Show(
                $"Are you sure you want to delete {selectedItems.Count} selected item{(selectedItems.Count > 1 ? "s" : "")}?",
                "Delete Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;
            foreach (var item in selectedItems)
            {
                if (!string.IsNullOrEmpty(item.RtfFilePath))
                {
                    if (File.Exists(item.RtfFilePath))
                    {
                        try
                        {
                            File.Delete(item.RtfFilePath);
                        }
                        catch (Exception ex)
                        {
                            ShowNotification("Error", $"Couldn't delete RTF file: {item.RtfFilePath}\n{ex.Message}",NotificationType.Error);
                        }
                    }
                    /*else
                    {
                        ShowNotification("Warning", $"RTF file doesn't exist: {item.RtfFilePath}", NotificationType.Warning);
                    }*/
                }
                items.Remove(item);
                
            }
            ShowNotification("Success", $"{selectedItems.Count} selected item{(selectedItems.Count > 1 ? "s have" : " has")} been successfully deleted", NotificationType.Success);

            contentService.SaveAll(items.ToList());
            UpdateDeleteButtonState();
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
