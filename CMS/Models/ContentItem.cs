using System;

namespace CMS.Models
{
    public class ContentItem
    {
        public bool IsSelected { get; set; }
        public int NumericValue { get; set; }
        public string Text { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string RtfFilePath { get; set; }=string.Empty;

        public DateTime DateAdded { get; set; }

        public ContentItem()
        {
            DateAdded = DateTime.Now;
        }
        public ContentItem(int numeric, string text, string imgPath,string rtfFilePath)
        {
            NumericValue= numeric;
            Text= text;
            ImagePath= imgPath;
            RtfFilePath= rtfFilePath;
            DateAdded = DateTime.Now; 
        }
    }
}
