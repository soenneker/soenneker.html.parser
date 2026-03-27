using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Soenneker.AngleSharp.Parser.Abstract;
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
    private readonly IAngleSharpParser _angleSharpParser;

    public HtmlParserUtil(IHtmlClient htmlClient, IAngleSharpParser angleSharpParser)
    {
        _htmlClient = htmlClient;
        _angleSharpParser = angleSharpParser;
    }

    public async ValueTask<List<string>> GetAllAnchors(string uri, CancellationToken cancellationToken = default)
    {
        IDocument document = await DownloadAndParse(uri, cancellationToken)
            .NoSync();

        return GetAllAnchorsFromDocument(document);
    }

    public async ValueTask<List<string>> GetAllAnchorsFromHtml(string content, CancellationToken cancellationToken = default)
    {
        if (content.IsNullOrEmpty())
            return [];

        IDocument document = await ParseHtml(content, cancellationToken)
            .NoSync();

        return GetAllAnchorsFromDocument(document);
    }

    public async ValueTask<List<string>> GetAllImageUrlsViaRegex(string uri, CancellationToken cancellationToken = default)
    {
        string content = await DownloadHtml(uri, cancellationToken)
            .NoSync();

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
            unique.Add(content.Substring(m.Index, m.Length));
        }

        return unique.Count == 0 ? [] : [.. unique];
    }

    public async ValueTask<List<string>> GetAllUrlsFromImgTags(string uri, CancellationToken cancellationToken = default)
    {
        IDocument document = await DownloadAndParse(uri, cancellationToken)
            .NoSync();

        return GetAllUrlsFromImgTagsFromDocument(document, uri);
    }

    public async ValueTask<List<string>> GetAllUrlsFromImgTagsFromHtml(string content, string baseUriString, CancellationToken cancellationToken = default)
    {
        if (content.IsNullOrEmpty())
            return [];

        IDocument document = await ParseHtml(content, cancellationToken)
            .NoSync();

        return GetAllUrlsFromImgTagsFromDocument(document, baseUriString);
    }

    /// <summary>
    /// Downloads the HTML once, parses once, and extracts both anchors and image URLs from img[src].
    /// </summary>
    public async ValueTask<(List<string> Anchors, List<string> ImageUrls)> GetAnchorsAndImageUrls(string uri, CancellationToken cancellationToken = default)
    {
        IDocument document = await DownloadAndParse(uri, cancellationToken)
            .NoSync();

        return (GetAllAnchorsFromDocument(document), GetAllUrlsFromImgTagsFromDocument(document, uri));
    }

    private static List<string> GetAllAnchorsFromDocument(IDocument document)
    {
        var unique = new HashSet<string>(StringComparer.Ordinal);

        foreach (IElement anchor in document.QuerySelectorAll("a[href]"))
        {
            string? href = anchor.GetAttribute("href");

            if (!href.IsNullOrWhiteSpace())
                unique.Add(href);
        }

        return unique.Count == 0 ? [] : [.. unique];
    }

    private static List<string> GetAllUrlsFromImgTagsFromDocument(IDocument document, string baseUriString)
    {
        if (baseUriString.IsNullOrWhiteSpace())
            return [];

        if (!Uri.TryCreate(baseUriString, UriKind.Absolute, out Uri? baseUri))
            return [];

        var unique = new HashSet<string>(StringComparer.Ordinal);

        foreach (IElement image in document.QuerySelectorAll("img[src]"))
        {
            string? src = image.GetAttribute("src");

            if (src.IsNullOrEmpty())
                continue;

            if (Uri.TryCreate(src, UriKind.Absolute, out Uri? absoluteUri))
            {
                unique.Add(absoluteUri.ToString());
                continue;
            }

            if (Uri.TryCreate(baseUri, src, out Uri? resolvedUri))
                unique.Add(resolvedUri.ToString());
        }

        return unique.Count == 0 ? [] : [.. unique];
    }

    public async ValueTask<IDocument> DownloadAndParse(string uri, CancellationToken cancellationToken = default)
    {
        string html = await DownloadHtml(uri, cancellationToken)
            .NoSync();

        return await ParseHtml(html, cancellationToken)
            .NoSync();
    }

    public async ValueTask<IDocument> ParseHtml(string html, CancellationToken cancellationToken = default)
    {
        HtmlParser parser = await _angleSharpParser.Get(cancellationToken)
                                                   .NoSync();

        return await parser.ParseDocumentAsync(html, cancellationToken);
    }

    public async ValueTask<string> DownloadHtml(string uri, CancellationToken cancellationToken = default)
    {
        HttpClient client = await _htmlClient.Get(cancellationToken)
                                             .NoSync();

        return await client.GetStringAsync(uri, cancellationToken)
                           .NoSync();
    }
}