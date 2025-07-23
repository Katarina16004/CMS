using CMS.Models;
using System.Windows.Controls;

namespace CMS.Services.Interfaces
{
    public interface ISaveContentService
    {
        //void AddContent(ICollection<ContentItem> items, ContentItem newItem, RichTextBox rtb);
        public bool UpdateContentItem(ICollection<ContentItem> allItems, ContentItem selectedItem, string newTitle, int newNumericValue, string newImagePath, RichTextBox newRichTextBox);
    }
}
