using System.ComponentModel;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace PmaIndicatorHandlersV3;

[HandlerCategory(HandlerCategories.Indicators)]
[HelperName("PMA V3", Language = Constants.En)]
[HelperName("PMA V3", Language = Constants.Ru)]
[LocalizedDescription("IndicatorDescription")]
[HelperDescription("Prosvirin Moving Average based on the linear regression line value projected onto the last bar of each rolling window.", Language = Constants.En)]
[HelperLink("https://trendteam.pro/club", Name = "TrendTeam Traders Club", Language = Constants.En)]
[HelperLink("https://trendteam.pro/club", Name = "Клуб трейдеров TrendTeam", Language = Constants.Ru)]
[InputsCount(1)]
[Input(0, TemplateTypes.DOUBLE, false, "Source")]
[OutputsCount(1)]
[OutputType(TemplateTypes.DOUBLE)]
public sealed class PmaIndicatorHandlerV3 : IStreamHandler
{
    private const double DeterminantEpsilon = 1e-12;

    [HelperName("Period", Constants.En)]
    [HelperName("Период", Constants.Ru)]
    [LocalizedDescription("PeriodDescription")]
    [HelperDescription("Period of the linear regression window.", Constants.En)]
    [HandlerParameter(true, "14", Min = "2", Max = "500", Step = "1", Name = "Period", NotOptimized = false)]
    public int Period { get; set; } = 14;

    public IList<double> Execute(IList<double> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var count = source.Count;
        if (count == 0)
            return Array.Empty<double>();

        var period = Math.Max(2, Period);
        var result = new double[count];
        var warmupCount = Math.Min(period - 1, count);

        for (var i = 0; i < warmupCount; i++)
            result[i] = source[i];

        if (count < period)
            return result;

        // Regression constants depend only on the selected period, so compute them once.
        double sumX = 0.0;
        double sumXX = 0.0;
        var n = (double)period;

        for (var i = 0; i < period; i++)
        {
            sumX += i;
            sumXX += (double)i * i;
        }

        // num10 is the regression denominator; guard against degenerate windows.
        var num10 = sumXX * n - sumX * sumX;

        for (var bar = period - 1; bar < count; bar++)
        {
            double sumY = 0.0;
            double sumXY = 0.0;
            var start = bar - period + 1;

            for (var i = 0; i < period; i++)
            {
                var y = source[start + i];
                sumY += y;
                sumXY += i * y;
            }

            if (Math.Abs(num10) < DeterminantEpsilon)
            {
                result[bar] = source[bar];
                continue;
            }

            var a = (sumXY * n - sumX * sumY) / num10;
            var b = (sumY - a * sumX) / n;

            result[bar] = a * (period - 1) + b;
        }

        return result;
    }
}
