using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script.Handlers;

[HandlerCategory("KaufmanIndicators")]
[Description(
    "Блок: KaufmanAdaptiveSMA\r\n\r\n" +
    "Адаптивная простая средняя на переменном периоде. Блок берет числовой ряд source, " +
    "считывает на каждом баре длину окна из ряда varPeriod и возвращает среднее значение на этом окне.\r\n\r\n" +
    "Параметры\r\n\r\n" +
    "Без параметров.\r\n\r\n" +
    "Входы\r\n\r\n" +
    "• source - число\r\n" +
    "• varPeriod - число\r\n\r\n" +
    "Тип выхода\r\n\r\n" +
    "• Число\r\n\r\n" +
    "Внешние ссылки\r\n\r\n" +
    "• Клуб трейдеров TrendTeam\r\n" +
    "• https://trendteam.pro/club")]
[InputsCount(2)]
[Input(0, TemplateTypes.DOUBLE, false, "source")]
[Input(1, TemplateTypes.DOUBLE, false, "varPeriod")]
[OutputType(TemplateTypes.DOUBLE)]
public sealed class KaufmanAdaptiveSMA : IStreamHandler
{
    public IList<double> Execute(IList<double> source, IList<double> varPeriod)
    {
        // Приводим входы к гарантированно непустым коллекциям,
        // чтобы дальнейший расчет не зависел от null-ссылок.
        var sourceValues = source ?? Array.Empty<double>();
        var periodValues = varPeriod ?? Array.Empty<double>();

        // Определяем длину общей доступной истории.
        // Индикатор может рассчитываться только там, где присутствуют оба входа.
        var count = Math.Min(sourceValues.Count, periodValues.Count);

        // Если данных нет, ничего не считаем.
        if (count <= 0)
            return Array.Empty<double>();

        // Подготавливаем выходной массив.
        var result = new double[count];

        // Идем по истории последовательно, потому что длина окна может меняться на каждом баре.
        for (var i = 0; i < count; i++)
        {
            // Нормализуем текущее значение периода в безопасный целый размер окна.
            var window = ClampPeriod(periodValues[i]);

            // Вычисляем начало текущего окна.
            // На ранних барах окно автоматически сокращается до доступной истории.
            var start = Math.Max(0, i - window + 1);

            // Создаем накопитель для суммы значений внутри окна.
            double sum = 0d;

            // Суммируем все значения исходного ряда внутри текущего адаптивного окна.
            for (var j = start; j <= i; j++)
                sum += sourceValues[j];

            // Делим сумму на фактическое количество элементов в окне.
            // Это особенно важно на первых барах, где полное окно еще не сформировалось.
            result[i] = sum / (i - start + 1);
        }

        // Возвращаем готовый ряд адаптивной простой средней.
        return result;
    }

    private static int ClampPeriod(double value)
    {
        // Защищаемся от NaN и бесконечности.
        // В таких случаях используем минимальное окно.
        if (double.IsNaN(value) || double.IsInfinity(value))
            return 1;

        // Округляем период до целого и не допускаем нуля или отрицательных значений.
        return Math.Max(1, (int)Math.Round(value, MidpointRounding.AwayFromZero));
    }
}
