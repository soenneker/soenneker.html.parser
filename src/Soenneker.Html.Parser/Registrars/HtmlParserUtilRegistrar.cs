using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.AngleSharp.Parser.Registrars;
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
    public static IServiceCollection AddHtmlParserUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHtmlClientAsSingleton()
                .AddAngleSharpParserAsSingleton();
        services.TryAddSingleton<IHtmlParserUtil, HtmlParserUtil>();
        return services;
    }

    /// <summary>
    /// Adds <see cref="IHtmlParserUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHtmlParserUtilAsScoped(this IServiceCollection services)
    {
        services.AddHtmlClientAsScoped()
                .AddAngleSharpParserAsScoped();
        services.TryAddScoped<IHtmlParserUtil, HtmlParserUtil>();
        return services;
    }
}