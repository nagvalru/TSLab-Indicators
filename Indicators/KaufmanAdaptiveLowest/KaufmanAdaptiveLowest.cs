using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script.Handlers;

[HandlerCategory("KaufmanIndicators")]
[Description(
    "Блок: KaufmanAdaptiveLowest\r\n\r\n" +
    "Адаптивный минимум на переменном периоде. Блок берет числовой ряд source, " +
    "считывает на каждом баре длину окна из ряда varPeriod и возвращает минимум на этом окне.\r\n\r\n" +
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
public sealed class KaufmanAdaptiveLowest : IStreamHandler
{
    public IList<double> Execute(IList<double> source, IList<double> varPeriod)
    {
        // Нормализуем входы в гарантированно непустые коллекции,
        // чтобы дальше не разыменовывать потенциально пустые ссылки.
        var sourceValues = source ?? Array.Empty<double>();
        var periodValues = varPeriod ?? Array.Empty<double>();

        // Определяем длину общей части истории,
        // на которой одновременно существуют и исходный ряд, и ряд периодов.
        var count = Math.Min(sourceValues.Count, periodValues.Count);

        // Если данных нет, возвращаем пустой результат.
        if (count <= 0)
            return Array.Empty<double>();

        // Подготавливаем массив результата для всех рассчитываемых баров.
        var result = new double[count];

        // Последовательно обрабатываем каждый бар истории.
        for (var i = 0; i < count; i++)
        {
            // Переводим текущее значение адаптивного периода в безопасную длину окна.
            var window = ClampPeriod(periodValues[i]);

            // Вычисляем начало окна.
            // В начале истории окно автоматически укорачивается до доступной длины.
            var start = Math.Max(0, i - window + 1);

            // Стартуем с максимально возможного значения double,
            // чтобы первый реальный элемент окна сразу мог стать новым минимумом.
            var lowest = double.MaxValue;

            // Ищем наименьшее значение внутри адаптивного окна.
            for (var j = start; j <= i; j++)
            {
                // Если нашли новое более низкое значение, обновляем минимум окна.
                if (sourceValues[j] < lowest)
                    lowest = sourceValues[j];
            }

            // Сохраняем найденный минимум как значение индикатора на текущем баре.
            result[i] = lowest;
        }

        // Возвращаем готовый ряд адаптивных минимумов.
        return result;
    }

    private static int ClampPeriod(double value)
    {
        // Нечисловые и бесконечные значения периода считаем ошибочными
        // и заменяем их на минимально допустимое окно.
        if (double.IsNaN(value) || double.IsInfinity(value))
            return 1;

        // Округляем период и не позволяем ему стать меньше одного бара.
        return Math.Max(1, (int)Math.Round(value, MidpointRounding.AwayFromZero));
    }
}
