using NetXmlDt;

namespace Samples;

public class Xspf2Html : NetXmlDtProcessor
{
    // ignore all tags that are not described in the processor
    protected override object Default_(Element element) => "";

    // title gets an H1 tag
    public string title(Element el)
    {
        el.TagName = InPath_("track") ? "td" : "h1";
        return el.ToXml();
    }

    public string creator(Element el) => InPath_("track") ? el.SetTag("td").ToXml() : "";

    // tracklist is a table tag
    public string trackList(Element el) => el.SetTag("table").ToXml();

    // track is a table row
    public string track(Element el) => el.SetTag("tr").ToXml();

    // title, creator and album are table cells

    public string album(Element el) => el.SetTag("td").ToXml();

    public string playlist(Element el) => el.SetTag("html").ToXml();
}