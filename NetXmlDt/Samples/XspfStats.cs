using NetXmlDt;

namespace Samples;
public class XspfStats : NetXmlDtProcessor
{
    private readonly Dictionary<string, List<string>> _byCreator = [];
    private readonly Dictionary<string, List<string>> _byAlbum = [];

    public void title(Element el)
    {
        if (InPath_("track"))
            Father_.Attributes["title"] = (string) el.Content;
    }

    public void creator(Element el)
    {
        if (InPath_("track"))
            Father_.Attributes["creator"] = (string) el.Content;
    }

    public void album(Element el) => Father_.Attributes["album"] = (string) el.Content;

    public void track(Element el)
    {
        _byCreator.AddToList(el.Attributes["creator"], el.Attributes["title"]);
        _byAlbum.AddToList(el.Attributes["album"], el.Attributes["title"]);
    }

    public void playlist(Element el)
    {
        Console.WriteLine("Titles by Creator");
        foreach (var creator in _byCreator.Keys)
        {
            Console.WriteLine(creator + " => " + string.Join("; ", _byCreator[creator]));
        }
        Console.WriteLine("Titles by Album");
        foreach (var creator in _byAlbum.Keys)
        {
            Console.WriteLine(creator + " => " + string.Join("; ", _byAlbum[creator]));
        }
    }
}

static class DictionaryAuxMethods
{
    public static void AddToList(this Dictionary<string, List<string>> dict, string key, string value)
    {
        if (!dict.TryGetValue(key, out var valList))
            dict[key] = [value];
        else
            valList.Add(value);
    }
}