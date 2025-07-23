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
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Notification.Wpf;

namespace CMS
{
    /// <summary>
    /// Interaction logic for AdminUserControl.xaml
    /// </summary>
    public partial class AdminUserControl : UserControl
    {
        private XmlDataService<ContentItem> contentService;
        private ObservableCollection<ContentItem> items;
        private  DeleteContentService _deletionService;
        private readonly NotificationManager _notificationManager = new NotificationManager();

        public AdminUserControl()
        {
            InitializeComponent();
            contentService = new XmlDataService<ContentItem>("Data/Content.xml");
            _deletionService = new DeleteContentService(contentService);
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
            AdminAdd_Update detailsWindow = new AdminAdd_Update(items.ToList(),null);
            Window menuWindow = Window.GetWindow(this);
            detailsWindow.ShowDialog();
            LoadContent();
            if (detailsWindow.IsSuccess)
                ShowNotification("Success", "Content was successfully saved", NotificationType.Success);
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
            
            int deleted = _deletionService.DeleteContent(items, selectedItems);

            if(deleted < 1)
                ShowNotification("Error", $"Problem deleting a RTF file", NotificationType.Error);
            else
                ShowNotification("Success", $"{deleted} selected item{(deleted > 1 ? "s have" : " has")} been successfully deleted", NotificationType.Success);

            contentService.SaveAll(items.ToList());
            UpdateDeleteButtonState();
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (ContentDataGrid.SelectedItem is ContentItem selectedItem)
            {
                AdminAdd_Update detailsWindow = new AdminAdd_Update(items.ToList(), selectedItem);
                Window menuWindow = Window.GetWindow(this);
                detailsWindow.ShowDialog();
                if (detailsWindow.IsUpdate)
                    ShowNotification("Success", "Content was successfully updated", NotificationType.Success);
            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox selectAllCheckBox)
            {
                bool isChecked = selectAllCheckBox.IsChecked == true;

                foreach (var item in ContentDataGrid.Items)
                {
                    if (item is ContentItem contentItem)
                    {
                        contentItem.IsSelected = isChecked;
                    }
                }
                ContentDataGrid.Items.Refresh();
            }
        }
    }
}
