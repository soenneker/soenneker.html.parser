using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Html.Parser.Abstract;

/// <summary>
/// A utility library for HTML parsing related operations
/// </summary>
public interface IHtmlParserUtil
{
    /// <summary>
    /// Asynchronously retrieves all unique anchor URLs from the specified URI.
    /// </summary>
    /// <param name="uri">The URI of the webpage to parse.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of unique anchor URLs found on the page.</returns>
    [Pure]
    ValueTask<List<string>> GetAllAnchors(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all unique anchor URLs from the provided HTML content.
    /// </summary>
    /// <param name="content">The HTML content to parse.</param>
    /// <returns>A list of unique anchor URLs found in the HTML content.</returns>
    [Pure]
    List<string> GetAllAnchorsFromHtml(string content);

    /// <summary>
    /// Asynchronously retrieves all unique image URLs from the specified URI using a regular expression.
    /// </summary>
    /// <param name="uri">The URI of the webpage to parse.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of unique image URLs found on the page.</returns>
    [Pure]
    ValueTask<List<string>> GetAllImageUrlsViaRegex(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all unique image URLs from the provided HTML content using a regular expression.
    /// </summary>
    /// <param name="content">The HTML content to parse.</param>
    /// <returns>A list of unique image URLs found in the HTML content.</returns>
    [Pure]
    List<string> GetAllImageUrlsViaRegexFromHtml(string content);

    /// <summary>
    /// Asynchronously retrieves all unique image URLs from img tags in the specified URI, resolving relative URLs.
    /// </summary>
    /// <param name="uri">The URI of the webpage to parse.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a list of unique image URLs from <img> tags on the page.</returns>
    [Pure]
    ValueTask<List<string>> GetAllUrlsFromImgTags(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all unique image URLs from img tags in the provided HTML content, resolving relative URLs based on a base URI.
    /// </summary>
    /// <param name="content">The HTML content to parse.</param>
    /// <param name="baseUriString">The base URI to resolve relative URLs.</param>
    /// <returns>A list of unique image URLs from img tags found in the HTML content.</returns>
    [Pure]
    List<string> GetAllUrlsFromImgTagsFromHtml(string content, string baseUriString);
}