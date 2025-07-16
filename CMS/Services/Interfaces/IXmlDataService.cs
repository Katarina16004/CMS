using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Interfaces
{
    public interface IXmlDataService<T>
    {
        public List<T> LoadAll();
        public void SaveAll(List<T> items);
    }
}
