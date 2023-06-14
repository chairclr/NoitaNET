using System.Xml;
using System.Xml.Serialization;

namespace NoitaNET.Loader.Utility;

internal static class XmlUtility
{
    public static T? Deserialize<T>(XmlReader xmlReader)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        return (T?)xmlSerializer.Deserialize(xmlReader);
    }
}
