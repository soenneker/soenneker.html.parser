using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Html.Client.Abstract;
using Soenneker.Html.Parser.Abstract;
using Soenneker.Html.Parser.Utils;

namespace Soenneker.Html.Parser;

/// <inheritdoc cref="IHtmlParserUtil"/>
public sealed class HtmlParserUtil : IHtmlParserUtil
{
    private readonly IHtmlClient _htmlClient;

    public HtmlParserUtil(IHtmlClient htmlClient)
    {
        _htmlClient = htmlClient;
    }

    public async ValueTask<List<string>> GetAllAnchors(string uri, CancellationToken cancellationToken = default)
    {
        HtmlDocument doc = await DownloadAndParse(uri, cancellationToken)
            .ConfigureAwait(false);
        return GetAllAnchorsFromDocument(doc);
    }

    public List<string> GetAllAnchorsFromHtml(string content)
    {
        if (content.IsNullOrEmpty())
            return [];

        HtmlDocument doc = ParseHtml(content);
        return GetAllAnchorsFromDocument(doc);
    }

    public async ValueTask<List<string>> GetAllImageUrlsViaRegex(string uri, CancellationToken cancellationToken = default)
    {
        // Regex requires the HTML string, so download once; parse is not needed here.
        string content = await DownloadHtml(uri, cancellationToken)
            .ConfigureAwait(false);
        return GetAllImageUrlsViaRegexFromHtml(content);
    }

    public List<string> GetAllImageUrlsViaRegexFromHtml(string content)
    {
        if (content.IsNullOrEmpty())
            return [];

        Regex regex = UrlExtractor.ImageUrlRegex();
        var unique = new HashSet<string>(StringComparer.Ordinal);

        foreach (ValueMatch m in regex.EnumerateMatches(content))
        {
            // Must allocate string for return type
            unique.Add(content.Substring(m.Index, m.Length));
        }

        return unique.Count == 0 ? [] : [..unique];
    }

    public async ValueTask<List<string>> GetAllUrlsFromImgTags(string uri, CancellationToken cancellationToken = default)
    {
        HtmlDocument doc = await DownloadAndParse(uri, cancellationToken)
            .ConfigureAwait(false);
        return GetAllUrlsFromImgTagsFromDocument(doc, uri);
    }

    public List<string> GetAllUrlsFromImgTagsFromHtml(string content, string baseUriString)
    {
        if (content.IsNullOrEmpty())
            return [];

        HtmlDocument doc = ParseHtml(content);
        return GetAllUrlsFromImgTagsFromDocument(doc, baseUriString);
    }

    /// <summary>
    /// Downloads the HTML once, parses once, and extracts both anchors and image URLs from img[src].
    /// </summary>
    public async ValueTask<(List<string> Anchors, List<string> ImageUrls)> GetAnchorsAndImageUrls(string uri, CancellationToken cancellationToken = default)
    {
        HtmlDocument doc = await DownloadAndParse(uri, cancellationToken)
            .ConfigureAwait(false);
        return (GetAllAnchorsFromDocument(doc), GetAllUrlsFromImgTagsFromDocument(doc, uri));
    }

    private static List<string> GetAllAnchorsFromDocument(HtmlDocument doc)
    {
        // DOM walk > XPath
        var unique = new HashSet<string>(StringComparer.Ordinal);

        foreach (HtmlNode a in doc.DocumentNode.Descendants("a"))
        {
            string? href = a.GetAttributeValue("href", null);
            if (!href.IsNullOrWhiteSpace())
                unique.Add(href);
        }

        return unique.Count == 0 ? [] : [..unique];
    }

    private static List<string> GetAllUrlsFromImgTagsFromDocument(HtmlDocument doc, string baseUriString)
    {
        if (baseUriString.IsNullOrWhiteSpace())
            return [];

        if (!Uri.TryCreate(baseUriString, UriKind.Absolute, out Uri? baseUri))
            return [];

        var unique = new HashSet<string>(StringComparer.Ordinal);

        foreach (HtmlNode img in doc.DocumentNode.Descendants("img"))
        {
            string? src = img.GetAttributeValue("src", null);
            if (src.IsNullOrEmpty())
                continue;

            // Absolute?
            if (Uri.TryCreate(src, UriKind.Absolute, out Uri? abs))
            {
                unique.Add(abs.ToString());
                continue;
            }

            // Relative -> resolve
            if (Uri.TryCreate(baseUri, src, out Uri? resolved))
                unique.Add(resolved.ToString());
        }

        return unique.Count == 0 ? [] : [..unique];
    }

    private async ValueTask<HtmlDocument> DownloadAndParse(string uri, CancellationToken cancellationToken)
    {
        string html = await DownloadHtml(uri, cancellationToken)
            .ConfigureAwait(false);
        return ParseHtml(html);
    }

    private static HtmlDocument ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc;
    }

    private async ValueTask<string> DownloadHtml(string uri, CancellationToken cancellationToken)
    {
        HttpClient client = await _htmlClient.Get(cancellationToken)
                                             .NoSync();
        return await client.GetStringAsync(uri, cancellationToken)
                           .NoSync();
    }
}