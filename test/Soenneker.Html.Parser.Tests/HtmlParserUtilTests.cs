using Soenneker.Html.Parser.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Html.Parser.Tests;

[Collection("Collection")]
public class HtmlParserUtilTests : FixturedUnitTest
{
    private readonly IHtmlParserUtil _util;

    public HtmlParserUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IHtmlParserUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
