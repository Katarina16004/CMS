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

            TitleTextBox.Focus();

            FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); //sortira po imenu
            FontFamily defaultFont = new FontFamily("Segoe UI");
            FontFamilyComboBox.SelectedItem = defaultFont;
            FontSizeComboBox.Text = "12";
            var colors = typeof(Colors).GetProperties()
            .Select(p => new
            {
                Name = p.Name,
                Brush = new SolidColorBrush((Color)p.GetValue(null))
            }).OrderBy(c => c.Name).ToList();

            FontColorComboBox.ItemsSource = colors;
            FontColorComboBox.SelectedItem= colors.FirstOrDefault(c => c.Name == "Black");

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
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
            if (FontSizeComboBox.SelectedItem!=null && !EditorRichTextBox.Selection.IsEmpty)
                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
        }

        private void FontColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontColorComboBox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {
                dynamic colorItem = FontColorComboBox.SelectedItem;
                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, colorItem.Brush);
            }
        }
    }
}
