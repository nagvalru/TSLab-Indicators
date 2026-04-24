using System.ComponentModel;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace TSLabIndicators.LinearRegressionEndpoint;

[HandlerCategory(HandlerCategories.Indicators)]
[HelperName("Linear Regression Endpoint", Language = Constants.En)]
[HelperName("Конечная точка линейной регрессии", Language = Constants.Ru)]
[LocalizedDescription("IndicatorDescription")]
[HelperDescription("Returns the fitted linear regression value at the last bar of each rolling window.", Language = Constants.En)]
[HelperDescription("Возвращает значение линии линейной регрессии в последней точке каждого скользящего окна.", Language = Constants.Ru)]
[HelperLink("https://trendteam.pro/club", Name = "TrendTeam Traders Club", Language = Constants.En)]
[HelperLink("https://trendteam.pro/club", Name = "Клуб трейдеров TrendTeam", Language = Constants.Ru)]
[InputsCount(1)]
[Input(0, TemplateTypes.DOUBLE, false, "Source")]
[OutputsCount(1)]
[OutputType(TemplateTypes.DOUBLE)]
public sealed class LinearRegressionEndpoint : IStreamHandler
{
    private const double DeterminantEpsilon = 1e-12;

    [HelperName("Period", Constants.En)]
    [HelperName("Период", Constants.Ru)]
    [LocalizedDescription("PeriodDescription")]
    [HelperDescription("Period of the rolling linear regression window.", Constants.En)]
    [HelperDescription("Период скользящего окна линейной регрессии.", Constants.Ru)]
    [HandlerParameter(true, "14", Min = "2", Max = "500", Step = "1", Name = "Period", NotOptimized = false)]
    public int Period { get; set; } = 14;

    public IList<double> Execute(IList<double> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var count = source.Count;
        if (count == 0)
            return Array.Empty<double>();

        // The indicator needs at least two points to build a regression line.
        var period = Math.Max(2, Period);
        var result = new double[count];
        var warmupCount = Math.Min(period - 1, count);

        // Before the first full regression window, return the source value.
        for (var i = 0; i < warmupCount; i++)
            result[i] = source[i];

        if (count < period)
            return result;

        // X coordinates are always 0..period-1, so these sums are constant.
        double sumX = 0.0;
        double sumXX = 0.0;
        var n = (double)period;

        for (var i = 0; i < period; i++)
        {
            sumX += i;
            sumXX += (double)i * i;
        }

        // Regression denominator. A defensive check below handles degenerate input.
        var denominator = sumXX * n - sumX * sumX;

        for (var bar = period - 1; bar < count; bar++)
        {
            double sumY = 0.0;
            double sumXY = 0.0;
            var start = bar - period + 1;

            // Build the regression line on the current rolling window.
            for (var i = 0; i < period; i++)
            {
                var y = source[start + i];
                sumY += y;
                sumXY += i * y;
            }

            if (Math.Abs(denominator) < DeterminantEpsilon)
            {
                result[bar] = source[bar];
                continue;
            }

            // y = slope * x + intercept
            var slope = (sumXY * n - sumX * sumY) / denominator;
            var intercept = (sumY - slope * sumX) / n;

            // Return the fitted line value at the last point of the window.
            result[bar] = slope * (period - 1) + intercept;
        }

        return result;
    }
}
