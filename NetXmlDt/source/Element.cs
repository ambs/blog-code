using System.Net.Mail;
using System.Text;
using System.Xml.Linq;

namespace NetXmlDt;

public class Element
{
    public Element(string tagName, IEnumerable<XAttribute> attributes, object? contents = null)
    {
        TagName = tagName;
        Content = contents;
        Attributes = new Attributes(attributes);
    }

    public string ToXml()
    {
        var tag = new StringBuilder($"<{TagName}");

        if (Attributes.Count > 0)
            tag.Append(string.Join("", Attributes.Select(pair => $" {pair.Key}=\"{pair.Value}\"")));

        switch (Content)
        {
            case string strContent:
                tag.Append($">{strContent}</{TagName}>");
                break;
            case not null:
                tag.Append($">{Content.ToString()}</{TagName}>");
                break;
            default:
                tag.Append("/>");
                break;
        }
        return tag.ToString();
    }

    public string TagName { get; set; }
    public Attributes Attributes { get; }
    public object? Content { get; set; }

    public Element SetTag(string name)
    {
        TagName = name;
        return this;
    }
}