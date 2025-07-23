using CMS.Models;
using System.Windows.Controls;

namespace CMS.Services.Interfaces
{
    public interface ISaveContentService
    {
        public bool AddContentItem(ICollection<ContentItem> allItems, string title, int numericValue, string imagePath, RichTextBox richTextBox);
        public bool UpdateContentItem(ICollection<ContentItem> allItems, ContentItem selectedItem, string newTitle, int newNumericValue, string newImagePath, RichTextBox newRichTextBox);
    }
}
