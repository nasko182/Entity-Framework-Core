using System.Data.SqlTypes;
using System.Text;
using System.Xml.Serialization;
using Trucks.DataProcessor.ImportDto;

namespace Trucks.Utilities;

public class XmlHelper
{

	public T Deserialize<T>(string xmlInput, string rootName)
	{
        var root = new XmlRootAttribute(rootName);
        var serializer = new XmlSerializer(typeof(T), root);
        T despatchersDTOs;

        using (var reader = new StringReader(xmlInput))
        {
            despatchersDTOs = (T)serializer.Deserialize(reader);
        };

        return despatchersDTOs;
    }

    public string Serialize<T>(T collection, string rootName)
    {
        var sb = new StringBuilder();
        var root = new XmlRootAttribute(rootName);
        var serializer = new XmlSerializer(typeof(T), root);

        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);

        using (var writer = new StringWriter(sb))
        {
            serializer.Serialize(writer, collection, namespaces);
        };

        return sb.ToString().TrimEnd();
    }
}
