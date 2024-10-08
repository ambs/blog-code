﻿using System.Xml.Linq;

namespace NetXmlDt;


public class NetXmlDt<T> where T : NetXmlDtProcessor, new()
{
    private readonly XDocument _document;
    private readonly T _processor;
    private readonly HashSet<string> _handlers;
    private readonly Stack<Element> _path;

    public NetXmlDt(string contents)
    {
        _document = XDocument.Parse(contents);
        _path = new Stack<Element>();
        _processor = new T();
        _handlers = [];

        var availableMethods = _processor.GetType().GetMethods();
        foreach (var method in availableMethods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Element))
            {
                _handlers.Add(method.Name);
            }
        }

        _processor._Initialize_(_path);
    }

    public object? Dt() => RecurseNode(_document);

    private object? RecurseNode(XComment node)
    {
        // if (dispatchTable.TryGetValue("-comment", out var callback))
        // {
        //
        // }

        return null;
    }

    private object? RecurseNode(XCData node) => null;

    private object? RecurseNode(XDocument node) => Aggregator(node.Nodes().Select(RecurseNode));

    private object? RecurseNode(XElement node)
    {

        var name = node.Name.LocalName;
        object? result = null;
        var element = new Element(name, node.Attributes());

        _path.Push(element);
        element.Content = Aggregator(node.Nodes().Select(RecurseNode));
        _path.Pop();

        if (_handlers.Contains(name))
        {
            var callback = _processor.GetType().GetMethod(name, [typeof(Element)]);
            if (callback is null) return null;

            if (callback.ReturnType == typeof(void))
                callback.Invoke(_processor, [element]);
            else
                result = (string) (callback.Invoke(_processor, [element]) ?? "");
        }
        else
            result = _processor.Default_(element);

        return result;
    }

    private object? RecurseNode(XText node) => _processor.PcData_(node.Value);

    private object? RecurseNode(XNode node) => node switch
    {
        XComment  xComment  => RecurseNode(xComment),
        XCData    xcData    => RecurseNode(xcData),
        XDocument xDocument => RecurseNode(xDocument),
        XElement  xElement  => RecurseNode(xElement),
        XText     xText     => RecurseNode(xText),

        _ => throw new NotImplementedException($"Type {node.NodeType}")
        // XContainer xContainer => throw new NotImplementedException(),
        // XDocumentType xDocumentType => throw new NotImplementedException(),
        // XProcessingInstruction xProcessingInstruction => throw new NotImplementedException(),
    };

    private object? Aggregator(IEnumerable<object?> children)
        => _processor.Aggregator_(children.Where(x => x is not null).ToList());

}
