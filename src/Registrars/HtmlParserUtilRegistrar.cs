using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Html.Client.Registrars;
using Soenneker.Html.Parser.Abstract;

namespace Soenneker.Html.Parser.Registrars;

/// <summary>
/// A utility library for HTML parsing related operations
/// </summary>
public static class HtmlParserUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IHtmlParserUtil"/> as a singleton service. <para/>
    /// </summary>
    public static void AddHtmlParserUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHtmlClientAsSingleton();
        services.TryAddSingleton<IHtmlParserUtil, HtmlParserUtil>();
    }

    /// <summary>
    /// Adds <see cref="IHtmlParserUtil"/> as a scoped service. <para/>
    /// </summary>
    public static void AddHtmlParserUtilAsScoped(this IServiceCollection services)
    {
        services.AddHtmlClientAsScoped();
        services.TryAddScoped<IHtmlParserUtil, HtmlParserUtil>();
    }
}
