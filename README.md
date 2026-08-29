[![](https://img.shields.io/nuget/v/soenneker.html.parser.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.html.parser/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.html.parser/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.html.parser/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.html.parser.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.html.parser/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.html.parser/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.html.parser/actions/workflows/codeql.yml)

# Soenneker.Html.Parser

A utility library for HTML parsing related operations.

## Install

```bash
dotnet add package Soenneker.Html.Parser
```

## Quick start

```csharp
using Soenneker.Html.Parser.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddHtmlParserUtilAsSingleton();
```

Adds `IHtmlParserUtil` as a singleton service.

## What you get

- `IHtmlParserUtil` — A utility library for HTML parsing related operations.
- `HtmlParserUtilRegistrar` — A utility library for HTML parsing related operations.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IHtmlParserUtil.GetAllAnchors(uri, cancellationToken)` | Asynchronously retrieves all unique anchor URLs from the specified URI. | A task representing the asynchronous operation, with a list of unique anchor URLs found on the page. |
| `IHtmlParserUtil.GetAllAnchorsFromHtml(content, cancellationToken)` | Retrieves all Anchors From HTML. | A list of unique anchor URLs found in the HTML content. |
| `IHtmlParserUtil.GetAllImageUrlsViaRegex(uri, cancellationToken)` | Asynchronously retrieves all unique image URLs from the specified URI using a regular expression. | A task representing the asynchronous operation, with a list of unique image URLs found on the page. |
| `IHtmlParserUtil.GetAllImageUrlsViaRegexFromHtml(content)` | Retrieves all Image Urls Via Regex From HTML. | A list of unique image URLs found in the HTML content. |
| `IHtmlParserUtil.GetAllUrlsFromImgTags(uri, cancellationToken)` | Asynchronously retrieves all unique image URLs from img tags in the specified URI, resolving relative URLs. | A task whose result contains the unique image URLs found in `img` tags on the page. |
| `IHtmlParserUtil.GetAllUrlsFromImgTagsFromHtml(content, baseUriString, cancellationToken)` | Retrieves all unique image URLs from img tags in the provided HTML content, resolving relative URLs based on a base URI. | A list of unique image URLs from img tags found in the HTML content. |
| `IHtmlParserUtil.DownloadHtml(uri, cancellationToken)` | Downloads HTML. | A task whose result is the text returned by download HTML. |
| `HtmlParserUtilRegistrar.AddHtmlParserUtilAsSingleton(services)` | Adds `IHtmlParserUtil` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `HtmlParserUtilRegistrar.AddHtmlParserUtilAsScoped(services)` | Adds `IHtmlParserUtil` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
