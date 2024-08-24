
namespace NetXmlDt;

public abstract class NetXmlDtProcessor
{
    private Stack<Element> _path = new();
    public void _Initialize_(Stack<Element> path)
    {
        _path = path;
    }
    public bool InPath_(string name) => _path.Any(elem => elem.TagName == name);

    public Element Father_ => _path.Count > 0 ? _path.Peek() : throw new IndexOutOfRangeException();

    public virtual object? PcData_(string pcData) => pcData;

    protected internal virtual object? Default_(Element element) => element.ToXml();

    protected internal virtual object? Aggregator_(List<object?> children)
        => children.Count > 0 ? string.Join("", children) : null;
}