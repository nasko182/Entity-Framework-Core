using System.Text;
using System.Xml.Serialization;

namespace Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string xmlInput, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            using (var reader = new StringReader(xmlInput))
            {
                T dtos = (T)serializer.Deserialize(reader);

                return dtos;
            }

        }

            public string Serialize<T>(T dtoCollection, string rootName)
        {
            var root = new XmlRootAttribute(rootName);
            var serializer = new XmlSerializer(typeof(T), root);
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, dtoCollection, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
