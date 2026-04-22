using Soenneker.Html.Parser.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Html.Parser.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class HtmlParserUtilTests : HostedUnitTest
{
    private readonly IHtmlParserUtil _util;

    public HtmlParserUtilTests(Host host) : base(host)
    {
        _util = Resolve<IHtmlParserUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
