using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CMS.Models
{
    public class ContentItem: INotifyPropertyChanged
    {
        private bool isSelected;
        public int NumericValue { get; set; }
        public string Text { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string RtfFilePath { get; set; }=string.Empty;

        public DateTime DateAdded { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
