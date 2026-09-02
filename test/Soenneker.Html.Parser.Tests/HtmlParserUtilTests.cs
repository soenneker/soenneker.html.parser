using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    [Test]
    public async Task Image_extraction_resolves_relative_urls_and_ignores_non_http_schemes(CancellationToken cancellationToken)
    {
        const string html = """
            <img src="/images/logo.png">
            <img src="https://cdn.example.com/photo.webp">
            <img src="data:image/png;base64,abc">
            <img src="javascript:alert(1)">
            """;

        List<string> urls = await _util.GetAllUrlsFromImgTagsFromHtml(html, "https://example.com/products/", cancellationToken: cancellationToken);

        await Assert.That(urls).IsEquivalentTo([
            "https://example.com/images/logo.png",
            "https://cdn.example.com/photo.webp"
        ]);
    }
}
