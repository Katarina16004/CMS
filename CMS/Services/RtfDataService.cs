using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using CMS.Services.Interfaces;

namespace CMS.Services
{
    public class RtfDataService:IRtfDataService
    {
        public void SaveRtfContent(RichTextBox rtb, string filePath)
        {
            TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                range.Save(fs, DataFormats.Rtf);
            }
        }
        public void LoadRtfContent(RichTextBox rtb, string filePath)
        {
            if (File.Exists(filePath))
            {
                TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    range.Load(fs, DataFormats.Rtf);
                }
            }
        }
    }

}
