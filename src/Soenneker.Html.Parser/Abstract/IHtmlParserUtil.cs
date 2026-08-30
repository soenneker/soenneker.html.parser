using AngleSharp.Dom;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Html.Parser.Abstract;

/// <summary>
/// Downloads and parses HTML, then extracts links and image URLs.
/// </summary>
public interface IHtmlParserUtil
{
    /// <summary>
    /// Asynchronously retrieves all unique anchor URLs from the specified URI.
    /// </summary>
    /// <param name="uri">Receives the normalized absolute URI when parsing succeeds.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of unique anchor URLs found on the page.</returns>
    [Pure]
    ValueTask<List<string>> GetAllAnchors(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all Anchors From HTML.
    /// </summary>
    /// <param name="content">Content to render, store, or send.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A list of unique anchor URLs found in the HTML content.</returns>
    [Pure]
    ValueTask<List<string>> GetAllAnchorsFromHtml(string content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all unique image URLs from the specified URI using a regular expression.
    /// </summary>
    /// <param name="uri">Receives the normalized absolute URI when parsing succeeds.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of unique image URLs found on the page.</returns>
    [Pure]
    ValueTask<List<string>> GetAllImageUrlsViaRegex(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all Image Urls Via Regex From HTML.
    /// </summary>
    /// <param name="content">Content to render, store, or send.</param>
    /// <returns>A list of unique image URLs found in the HTML content.</returns>
    [Pure]
    List<string> GetAllImageUrlsViaRegexFromHtml(string content);

    /// <summary>
    /// Asynchronously retrieves all unique image URLs from img tags in the specified URI, resolving relative URLs.
    /// </summary>
    /// <param name="uri">The URI of the webpage to parse.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task whose result contains the unique image URLs found in <c>img</c> tags on the page.</returns>
    [Pure]
    ValueTask<List<string>> GetAllUrlsFromImgTags(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all unique image URLs from img tags in the provided HTML content, resolving relative URLs based on a base URI.
    /// </summary>
    /// <param name="content">The HTML content to parse.</param>
    /// <param name="baseUriString">The base URI to resolve relative URLs.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of unique image URLs from img tags found in the HTML content.</returns>
    [Pure]
    ValueTask<List<string>> GetAllUrlsFromImgTagsFromHtml(string content, string baseUriString, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads and parses a page once, returning its unique raw anchor targets and resolved HTTP(S) image URLs.
    /// </summary>
    /// <param name="uri">The absolute HTTP(S) page URI, also used to resolve relative image sources.</param>
    /// <param name="cancellationToken">Stops the download or parsing operation.</param>
    /// <returns>The anchor <c>href</c> values and resolved image URLs found in the document.</returns>
    ValueTask<(List<string> Anchors, List<string> ImageUrls)> GetAnchorsAndImageUrls(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads an HTML document and parses it with AngleSharp.
    /// </summary>
    /// <param name="uri">Receives the normalized absolute URI when parsing succeeds.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested document.</returns>
    ValueTask<IDocument> DownloadAndParse(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Parses HTML as a document with AngleSharp's error recovery.
    /// </summary>
    /// <param name="html">Rendered page HTML to inspect.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested document.</returns>
    ValueTask<IDocument> Parse(string html, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads HTML and requires a successful HTTP response.
    /// </summary>
    /// <param name="uri">Receives the normalized absolute URI when parsing succeeds.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by download HTML.</returns>
    ValueTask<string> DownloadHtml(string uri, CancellationToken cancellationToken = default);
}
