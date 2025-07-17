using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CMS.Services.Interfaces
{
    public interface IRtfDataService
    {
        public void SaveRtfContent(RichTextBox rtb, string filePath);
        public void LoadRtfContent(RichTextBox rtb, string filePath);
    }
}
