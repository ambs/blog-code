using NetXmlDt;

namespace TestSuite;

class ToLaTeX : NetXmlDtProcessor
{
    public string b(Element e) => $@"\textbf{{{e.Content}}}";
    public string i(Element e) => $@"\textit{{{e.Content}}}";
}

[TestClass]
public class StringHandlers
{
    [TestMethod]
    [DataRow("<b>bold</b>", @"\textbf{bold}")]
    [DataRow("<i>italic</i>", @"\textit{italic}")]
    public void LaTeXProcessor(string input, string expectedOutput)
    {
        var dt = new NetXmlDt<ToLaTeX>(input);
        Assert.AreEqual(expectedOutput, dt.Dt());
    }
}