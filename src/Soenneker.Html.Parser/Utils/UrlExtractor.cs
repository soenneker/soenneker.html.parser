using System.Text.RegularExpressions;

namespace Soenneker.Html.Parser.Utils;

internal partial class UrlExtractor
{
    // Use RegexGenerator to create a compiled regex method at compile time
    [GeneratedRegex(@"https?:\/\/[^\s""']*?\.(?:jpg|jpeg|png|gif|bmp|webp|svg|tiff)", RegexOptions.IgnoreCase)]
    internal static partial Regex ImageUrlRegex();
}