using System.Xml.Linq;

namespace NetXmlDt;

public class Attributes : Dictionary<string, string>
{
    public Attributes(IEnumerable<XAttribute> attributes)
    {
        foreach (var a in attributes)
        {
            var key = a.Name.LocalName;
            var value = a.Value;
            this[key] = value;
        }
    }
}