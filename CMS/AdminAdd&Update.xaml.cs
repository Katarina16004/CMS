using CMS.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CMS.Services;
using CMS.Services.Interfaces;
using Path = System.IO.Path;


namespace CMS
{
    /// <summary>
    /// Interaction logic for AdminAdd_Update.xaml
    /// </summary>
    public partial class AdminAdd_Update : Window
    {
        private DispatcherTimer notificationTimer;
        private ContentItem selectedItem, originalItem;
        private List<ContentItem> items;
        private string photo = "";
        private ContentValidationService validationService;
        private SaveContentService _saveContentService;
        private readonly RtfDataService rtfService;
        public bool IsSuccess { get; private set; } = false;
        public bool IsUpdate { get; private set; } = false;
        public AdminAdd_Update(List<ContentItem> items, ContentItem selectedItem = null)
        {
            InitializeComponent();
            validationService = new ContentValidationService();

            var xmlService = new XmlDataService<ContentItem>("Content.xml");
            this.rtfService = new RtfDataService();
            _saveContentService = new SaveContentService(xmlService, rtfService);
            #region notificationSetup
            notificationTimer = new DispatcherTimer();
            notificationTimer.Interval = TimeSpan.FromSeconds(3);
            notificationTimer.Tick += (s, e) =>
            {
                NotificationPanel.Visibility = Visibility.Collapsed;
                notificationTimer.Stop();
            };
            #endregion

            this.items = items;
            this.selectedItem = selectedItem;
            this.DataContext = selectedItem;
            
            //ako je selected item null znamo da smo pozvali dodavanje, prazna polja
            //ako nije null, radimo izmenu, popunjena polja

            if (selectedItem == null)
                TitleTextBox.Focus();
            else
            {
                originalItem = new ContentItem
                {
                    NumericValue = selectedItem.NumericValue,
                    Text = selectedItem.Text,
                    ImagePath = selectedItem.ImagePath,
                    RtfFilePath = selectedItem.RtfFilePath,
                    DateAdded = selectedItem.DateAdded
                };
                LoadSelectedItem();
            }
                

            FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); //sortira po imenu
            FontFamily defaultFont = new FontFamily("Segoe UI");
            FontFamilyComboBox.SelectedItem = defaultFont;
            FontSizeComboBox.ItemsSource = new List<double> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontSizeComboBox.SelectedItem = 12.0;

            var colors = typeof(Colors).GetProperties()
            .Select(p => new
            {
                Name = p.Name,
                Brush = new SolidColorBrush((Color)p.GetValue(null))
            }).OrderBy(c => c.Name).ToList();

            FontColorComboBox.ItemsSource = colors;
            FontColorComboBox.SelectedItem= colors.FirstOrDefault(c => c.Name == "Black");

        }
        private void LoadSelectedItem()
        {
            TitleTextBox.Text = selectedItem.Text;
            NumberTextBox.Text = selectedItem.NumericValue.ToString();
            photo = selectedItem.ImagePath;
            try
            {
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedItem.ImagePath);
                ImagePreview.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            catch { }

            try
            {
                string fullRtfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedItem.RtfFilePath);
                rtfService.LoadRtfContent(EditorRichTextBox, fullRtfPath);
            }
            catch (Exception ex)
            {
                ShowToast("Error", "Failed to load RTF content: " + ex.Message, false);
            }
        }
        private void ShowToast(string title, string message, bool success=false)
        {
            NotificationTitle.Text = title;
            NotificationMessage.Text = message;
            NotificationPanel.Visibility = Visibility.Visible;

            if (success)
            {
                NotificationPanel.Background = new SolidColorBrush(Colors.LightGreen);
                NotificationTitle.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
            else
            {
                NotificationPanel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD4444"));
                NotificationTitle.Foreground = Brushes.White;
                NotificationMessage.Foreground = Brushes.White;
            }

            notificationTimer.Stop();
            notificationTimer.Start();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem != null && originalItem != null)
            {
                TitleTextBox.Text = originalItem.Text;
                NumberTextBox.Text = originalItem.NumericValue.ToString();
                photo = originalItem.ImagePath;
                ImagePreview.Source = new BitmapImage(new Uri(photo, UriKind.RelativeOrAbsolute));

                rtfService.LoadRtfContent(EditorRichTextBox, originalItem.RtfFilePath);
            }

            this.Close();
        }

        private void EditorRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange text = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);
            int wordCount = text.Text.Split(new char[] { ' ', '\r', '\n', '.', ',', ';', ':', '\t', '!', '?', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries).Length;
            WordCountTextBlock.Text = $"Words: {wordCount}";
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {
                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem); //za svojstvo font family primeni izabrani font family
            }
        }

        private void EditorRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object fontWeight = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BoldButton.IsChecked = (fontWeight != DependencyProperty.UnsetValue) && (fontWeight.Equals(FontWeights.Bold)); //da li vrednost nije null
            object fontStyle = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ItalicButton.IsChecked = (fontStyle != DependencyProperty.UnsetValue) && fontStyle.Equals(FontStyles.Italic);
            object textDecoration = EditorRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (fontStyle != DependencyProperty.UnsetValue) && textDecoration.Equals(TextDecorations.Underline);

            object fontFamily = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontFamilyComboBox.SelectedItem = fontFamily;

            object fontSize = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
            FontSizeComboBox.SelectedItem = fontSize;
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem is double fontSize && !EditorRichTextBox.Selection.IsEmpty)
            {
                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize);
            }
        }

        private void FontColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontColorComboBox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {
                dynamic colorItem = FontColorComboBox.SelectedItem;
                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, colorItem.Brush);
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG Images (*.png)|*.png";
            openFileDialog.Title = "Select an image";

            if (openFileDialog.ShowDialog() == true)
            {
                photo = openFileDialog.FileName;
                ImagePreview.Source = new BitmapImage(new Uri(photo));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);
            string plainText = textRange.Text;
            bool hasText = !string.IsNullOrWhiteSpace(plainText.Trim());
            int parsedId = 0;
            bool isIdValid = int.TryParse(NumberTextBox.Text, out parsedId);

            var result = validationService.ValidationSuccessful(items, TitleTextBox.Text, NumberTextBox.Text, photo, hasText, selectedItem);
            if(result.IsValidationError)
            {
                if (string.IsNullOrWhiteSpace(NumberTextBox.Text) || !isIdValid)
                {
                    NumberNote.Content = "Fill in id field with a number";
                    NumberTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    NumberNote.Content = "";
                    NumberTextBox.ClearValue(Border.BorderBrushProperty);
                }
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    TitleNote.Content = "Fill in title field";
                    TitleTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    TitleNote.Content = "";
                    TitleTextBox.ClearValue(Border.BorderBrushProperty);
                }

                if (string.IsNullOrWhiteSpace(photo))
                {
                    PhotoNote.Content = "Insert image";
                    SelectImageButton.BorderBrush= Brushes.Red;
                }
                else
                {
                    PhotoNote.Content = "";
                    SelectImageButton.ClearValue(Border.BorderBrushProperty);
                }

                if (hasText==false)
                {
                    RichTbNote.Content = "Write some text";
                    EditorRichTextBox.BorderBrush= Brushes.Red;
                }
                else
                {
                    RichTbNote.Content = "";
                    EditorRichTextBox.ClearValue(Border.BorderBrushProperty);
                }
                return;
            }
            TitleNote.Content = "";
            TitleTextBox.ClearValue(Border.BorderBrushProperty);
            NumberNote.Content = "";
            NumberTextBox.ClearValue(Border.BorderBrushProperty);
            PhotoNote.Content = "";
            SelectImageButton.ClearValue(Border.BorderBrushProperty);
            RichTbNote.Content = "";
            EditorRichTextBox.ClearValue(Border.BorderBrushProperty);
            if (!result.Success)
            {
                if(result.Message.Contains("Id"))
                    ShowToast("Error", "Id already exists");
                else if (result.Message.Contains("Title"))
                    ShowToast("Error", "Title already exists");
                return;
            }

            //prosla verifikacija
            if (selectedItem == null) //dodavanje je ako je null
            {
                bool addResult=_saveContentService.AddContentItem(items,TitleTextBox.Text, parsedId, photo, EditorRichTextBox);
                if (addResult)
                    IsSuccess = true;
                else
                    IsSuccess = false;  
            }
            else
            {
                bool updateResult = _saveContentService.UpdateContentItem(items, selectedItem, TitleTextBox.Text, parsedId, photo, EditorRichTextBox);

                if (updateResult)
                    IsUpdate = true;
                else
                    IsUpdate=false;
            }
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
