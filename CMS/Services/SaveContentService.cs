using CMS.Models;
using CMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;

namespace CMS.Services
{
    public class SaveContentService:ISaveContentService
    {
        private readonly XmlDataService<ContentItem> _contentService;
        private readonly IRtfDataService _rtfDataService;

        public SaveContentService(XmlDataService<ContentItem> contentService, IRtfDataService rtfDataService)
        {
            _contentService = contentService;
            _rtfDataService = rtfDataService;
        }

        public bool UpdateContentItem(ICollection<ContentItem> allItems, ContentItem selectedItem, string newTitle, string newImagePath, RichTextBox newRichTextBox)
        {
            try
            {
                int index = allItems.ToList().FindIndex(item => item.NumericValue == selectedItem.NumericValue);
                var list = allItems.ToList();

                list[index].Text = newTitle;
                list[index].ImagePath = newImagePath;

                _rtfDataService.SaveRtfContent(newRichTextBox, list[index].RtfFilePath);
                _contentService.SaveAll(list);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
