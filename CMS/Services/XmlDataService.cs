using CMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CMS.Services
{
    public class XmlDataService<T>:IXmlDataService<T>
    {
        private readonly string filePath;
        private XmlSerializer serializer;

        public XmlDataService(string xmlFilePath)
        {
            filePath = xmlFilePath;
            serializer = new XmlSerializer(typeof(List<T>));
        }

        public List<T> LoadAll()
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return (List<T>)serializer.Deserialize(fs);
            }
        }

        public void SaveAll(List<T> items)
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, items);
            }
        }
    }
}
