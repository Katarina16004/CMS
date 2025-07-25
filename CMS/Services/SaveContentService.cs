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
            _contentService = new XmlDataService<ContentItem>("Data/Content.xml");
            _rtfDataService = rtfDataService;
        }

        public bool UpdateContentItem(ICollection<ContentItem> allItems, ContentItem selectedItem, string newTitle, int newNumericValue, string newImagePath, RichTextBox newRichTextBox)
        {
            try
            {
                selectedItem.NumericValue = newNumericValue;
                selectedItem.Text = newTitle;
                selectedItem.ImagePath = newImagePath;

                _rtfDataService.SaveRtfContent(newRichTextBox, selectedItem.RtfFilePath);
                _contentService.SaveAll(allItems.ToList());

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddContentItem(ICollection<ContentItem> allItems, string title, int numericValue, string imagePath, RichTextBox richTextBox)
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string rtfFolderPath = Path.Combine(basePath, "Data");

                string rtfName = title.Replace(" ", "_");
                string rtfFilePath = Path.Combine(rtfFolderPath, $"{rtfName}.rtf");
                _rtfDataService.SaveRtfContent(richTextBox, rtfFilePath);

                string relativeRtfPath = Path.GetRelativePath(basePath, rtfFilePath);

                ContentItem newItem = new ContentItem
                {
                    Text = title,
                    NumericValue = numericValue,
                    ImagePath = imagePath,  
                    RtfFilePath = relativeRtfPath
                };

                allItems.Add(newItem);
                _contentService.SaveAll(allItems.ToList());

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
