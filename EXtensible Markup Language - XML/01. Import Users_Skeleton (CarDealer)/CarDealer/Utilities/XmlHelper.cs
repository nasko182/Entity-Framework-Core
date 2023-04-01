using CarDealer.DTOs.Export;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Utilities;

public class XmlHelper
{
    public T Deserialize<T>(string inputXml, string rootName)
    {
        XmlRootAttribute root = new XmlRootAttribute(rootName);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), root);

        using (StringReader reader = new StringReader(inputXml))
        {
            T Dtos = (T)xmlSerializer.Deserialize(reader);

            return Dtos;
        };
    }

    public string Serialize<T>(T DtoCollection,string rootName)
    {
        var root = new XmlRootAttribute(rootName);
        var serializer = new XmlSerializer(typeof(T), root);

        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);

        var sb = new StringBuilder();
        using (var writer = new StringWriter(sb))
        {
            serializer.Serialize(writer, DtoCollection, namespaces);
        }

        return sb.ToString().TrimEnd();
    }
}
