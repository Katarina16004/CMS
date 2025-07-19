using CMS.Models;
using CMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Notification.Wpf;

namespace CMS.Services
{
    public class DeleteContentService:IDeleteContentService
    {
        private readonly XmlDataService<ContentItem> _contentService;
        public DeleteContentService(XmlDataService<ContentItem> contentService)
        {
            _contentService = contentService;
        }
        public int DeleteContent(ICollection<ContentItem> items, IEnumerable<ContentItem> toDelete)
        {
            int deletedCount = 0;

            foreach (var item in toDelete.ToList())  // ToList da ne menjamo kolekciju tokom iteracije
            {
                if (!string.IsNullOrEmpty(item.RtfFilePath))
                {
                    if (File.Exists(item.RtfFilePath))
                    {
                        try
                        {
                            File.Delete(item.RtfFilePath);
                        }
                        catch
                        {
                            return -1; //neuspesno brisanje
                        }
                    }
                }
                items.Remove(item);
                deletedCount++;
            }
            _contentService.SaveAll(items.ToList());

            return deletedCount;
        }
    }
}
