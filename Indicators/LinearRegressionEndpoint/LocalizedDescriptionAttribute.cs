using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace TSLabIndicators.LinearRegressionEndpoint;

/// <summary>
/// Reads DescriptionAttribute text from embedded resources using the current UI culture.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class LocalizedDescriptionAttribute : DescriptionAttribute
{
    private static readonly ResourceManager ResourceManager =
        new("TSLabIndicators.LinearRegressionEndpoint.Properties.Resources", Assembly.GetExecutingAssembly());

    private readonly string _resourceKey;

    public LocalizedDescriptionAttribute(string resourceKey)
    {
        _resourceKey = resourceKey;
    }

    public override string Description
    {
        get
        {
            var text = ResourceManager.GetString(_resourceKey, CultureInfo.CurrentUICulture);
            return string.IsNullOrWhiteSpace(text) ? _resourceKey : text;
        }
    }
}
