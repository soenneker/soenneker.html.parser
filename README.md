[![](https://img.shields.io/nuget/v/soenneker.html.parser.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.html.parser/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.html.parser/build-and-test.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.html.parser/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.html.parser/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.html.parser/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.html.parser.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.html.parser/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.html.parser/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.html.parser/actions/workflows/codeql.yml)

# Soenneker.Html.Parser

Downloads and parses HTML with AngleSharp, with helpers for extracting anchors and image URLs.

## Install

```bash
dotnet add package Soenneker.Html.Parser
```

## Register

```csharp
using Soenneker.Html.Parser.Registrars;

services.AddHtmlParserUtilAsSingleton();
```

Use `AddHtmlParserUtilAsScoped()` when the parser and its HTTP client should be owned by a dependency-injection scope.

## Parse HTML

```csharp
using AngleSharp.Dom;
using Soenneker.Html.Parser.Abstract;

IDocument document = await parser.Parse(html, cancellationToken);

string? title = document.QuerySelector("title")?.TextContent;
IElement? heading = document.QuerySelector("main h1");
```

AngleSharp parses with browser-style error recovery, so malformed markup may produce a repaired document rather than an exception.

## Download and extract links

```csharp
List<string> anchors = await parser.GetAllAnchors(
    "https://example.com/docs",
    cancellationToken);

(List<string> pageAnchors, List<string> images) =
    await parser.GetAnchorsAndImageUrls(
        "https://example.com/docs",
        cancellationToken);
```

Anchor helpers return unique, non-empty `href` strings exactly as written in the document; relative links are not resolved. The combined helper downloads and parses the page once.

## Extract image URLs from existing HTML

```csharp
List<string> images = await parser.GetAllUrlsFromImgTagsFromHtml(
    html,
    "https://example.com/catalog/",
    cancellationToken);
```

This reads `img[src]`, resolves relative values against the base URI, and returns unique HTTP(S) URLs. Data, JavaScript, file, and other schemes are ignored.

`GetAllImageUrlsViaRegexFromHtml()` is a separate text scan for absolute HTTP(S) image URLs ending in a common image extension. It does not understand HTML, resolve relative paths, inspect `srcset`, or retain query strings after the extension; prefer the DOM-based helper for normal pages.

The download APIs use `HttpClient.GetStringAsync`, require a successful response, and buffer the response body as a string. Do not expose arbitrary user-supplied URLs to these methods without applying your application's SSRF controls.
