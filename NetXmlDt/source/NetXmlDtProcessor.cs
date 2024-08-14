namespace NetXmlDt;

public abstract class NetXmlDtProcessor
{
    public virtual object? PcData_(string pcData) => pcData;

    protected internal virtual object? Default_(Element element) => element.ToXml();

    protected internal virtual object? Aggregator_(List<object?> children)
        => children.Count > 0 ? string.Join("", children) : null;
}