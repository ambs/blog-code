
using NetXmlDt;
using Samples;

var fileContents = File.ReadAllText("playlist.xml");
var convert2Html = new NetXmlDt<Xspf2Html>(fileContents);
var result = (string) convert2Html.Dt();
Console.WriteLine(result);

var stats = new NetXmlDt<XspfStats>(fileContents);
stats.Dt();
