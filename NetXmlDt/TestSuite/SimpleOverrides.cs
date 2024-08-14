using NetXmlDt;

namespace TestSuite;

public class RemovePcData : NetXmlDtProcessor
{
    public override object? PcData_(string pcData) => null;
}

public class RemoveTags : NetXmlDtProcessor
{
    protected override object? Default_(Element element) => element.Content;
}


[TestClass]
public class SimpleOverrides
{
    [TestMethod]
    [DataRow("<xml>foo</xml>", "<xml/>")]
    [DataRow("<a>foo<b>bar</b>zbr</a>", "<a><b/></a>")]
    [DataRow("<a foo=\"bar\">foo</a>", "<a foo=\"bar\"/>")]
    public void RemovePcData(string input, string expectedOutput)
    {
        var dt = new NetXmlDt<RemovePcData>(input);
        Assert.IsInstanceOfType<NetXmlDt<RemovePcData>>(dt);
        var result = dt.Dt();
        Assert.AreEqual(expectedOutput, result);
    }

    [TestMethod]
    [DataRow("<xml>foo</xml>", "foo")]
    [DataRow("<a>foo<b>bar</b>zbr</a>", "foobarzbr")]
    [DataRow("<a id=\"bar\">xpto bar</a>", "xpto bar")]
    public void RemoveTags(string input, string expectedOutput)
    {
        var dt = new NetXmlDt<RemoveTags>(input);
        Assert.IsInstanceOfType<NetXmlDt<RemoveTags>>(dt);
        var result = dt.Dt();
        Assert.AreEqual(expectedOutput, result);
    }
}