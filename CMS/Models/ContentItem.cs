using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CMS.Models
{
    public class ContentItem: INotifyPropertyChanged
    {
        private bool isSelected;
        private int numericValue;
        private string text;
        private string imagePath;
        private string rtfFilePath;
        private DateTime dateAdded;

        public string RtfFilePath
        {
            get => rtfFilePath;
            set
            {
                if (rtfFilePath != value)
                {
                    rtfFilePath = value;
                    OnPropertyChanged(nameof(RtfFilePath));
                }
            }
        }
        public DateTime DateAdded
        {
            get => dateAdded;
            set
            {
                if (dateAdded != value)
                {
                    dateAdded = value;
                    OnPropertyChanged(nameof(DateAdded));
                }
            }
        }
        public string ImagePath
        {
            get => imagePath;
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }
        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }
        public int NumericValue
        {
            get => numericValue;
            set
            {
                if (numericValue != value)
                {
                    numericValue = value;
                    OnPropertyChanged(nameof(NumericValue));
                }
            }
        }
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
