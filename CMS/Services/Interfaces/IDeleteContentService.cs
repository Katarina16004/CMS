using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Interfaces
{
    public interface IDeleteContentService
    {
        public int DeleteContent(ICollection<ContentItem> items, IEnumerable<ContentItem> toDelete);
    }
}
