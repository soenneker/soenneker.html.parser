using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Soenneker.Extensions.String;
using Soenneker.Html.Client.Abstract;
using Soenneker.Html.Parser.Abstract;
using Soenneker.Html.Parser.Utils;

namespace Soenneker.Html.Parser;

///<inheritdoc cref="IHtmlParserUtil"/>
public class HtmlParserUtil : IHtmlParserUtil
{
    private readonly IHtmlClient _htmlClient;

    public HtmlParserUtil(IHtmlClient htmlClient)
    {
        _htmlClient = htmlClient;
    }

    public async ValueTask<List<string>> GetAllAnchors(string uri, CancellationToken cancellationToken = default)
    {
        string content = await DownloadHtml(uri, cancellationToken);
        return GetAllAnchorsFromHtml(content);
    }

    public List<string> GetAllAnchorsFromHtml(string content)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
        var uniqueLinks = new HashSet<string>();

        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                string href = node.GetAttributeValue("href", string.Empty);

                if (!href.IsNullOrWhiteSpace())
                {
                    uniqueLinks.Add(href);
                }
            }
        }

        return new List<string>(uniqueLinks);
    }

    public async ValueTask<List<string>> GetAllImageUrlsViaRegex(string uri, CancellationToken cancellationToken = default)
    {
        string content = await DownloadHtml(uri, cancellationToken);
        return GetAllImageUrlsViaRegexFromHtml(content);
    }

    public List<string> GetAllImageUrlsViaRegexFromHtml(string content)
    {
        var uniqueImageUrls = new HashSet<string>();
        MatchCollection matches = UrlExtractor.ImageUrlRegex().Matches(content);

        foreach (Match match in matches)
        {
            uniqueImageUrls.Add(match.Value);
        }

        return new List<string>(uniqueImageUrls);
    }

    public async ValueTask<List<string>> GetAllUrlsFromImgTags(string uri, CancellationToken cancellationToken = default)
    {
        string content = await DownloadHtml(uri, cancellationToken);
        return GetAllUrlsFromImgTagsFromHtml(content, uri);
    }

    public List<string> GetAllUrlsFromImgTagsFromHtml(string content, string baseUriString)
    {
        var uniqueImageUrls = new HashSet<string>();
        var document = new HtmlDocument();
        document.LoadHtml(content);

        var imageNodes = document.DocumentNode.SelectNodes("//img[@src]");
        if (imageNodes != null)
        {
            var baseUri = new Uri(baseUriString);

            foreach (var img in imageNodes)
            {
                string imageUrl = img.GetAttributeValue("src", null);

                if (imageUrl.IsNullOrEmpty()) 
                    continue;

                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    imageUrl = new Uri(baseUri, imageUrl).ToString();
                }

                uniqueImageUrls.Add(imageUrl);
            }
        }

        return new List<string>(uniqueImageUrls);
    }

    private async ValueTask<string> DownloadHtml(string uri, CancellationToken cancellationToken)
    {
        HttpClient client = await _htmlClient.Get(cancellationToken);
        return await client.GetStringAsync(uri, cancellationToken);
    }
}