using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script.Handlers;

[HandlerCategory("KaufmanIndicators")]
[Description(
    "Блок: KaufmanAdaptiveHighest\r\n\r\n" +
    "Адаптивный максимум на переменном периоде. Блок берет числовой ряд source, " +
    "считывает на каждом баре длину окна из ряда varPeriod и возвращает максимум на этом окне.\r\n\r\n" +
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
public sealed class KaufmanAdaptiveHighest : IStreamHandler
{
    public IList<double> Execute(IList<double> source, IList<double> varPeriod)
    {
        // Нормализуем входные ссылки заранее, чтобы дальше код работал с гарантированно непустыми коллекциями.
        var sourceValues = source ?? Array.Empty<double>();
        var periodValues = varPeriod ?? Array.Empty<double>();

        // Определяем длину общей доступной истории.
        // Индикатор может считать только там, где одновременно доступны оба входных ряда.
        var count = Math.Min(sourceValues.Count, periodValues.Count);

        // Если доступной истории нет, возвращаем пустой результат без лишних вычислений.
        if (count <= 0)
            return Array.Empty<double>();

        // Подготавливаем выходной массив.
        // В него будем записывать найденный максимум на каждом баре.
        var result = new double[count];

        // Последовательно рассчитываем индикатор слева направо по истории.
        for (var i = 0; i < count; i++)
        {
            // Текущее значение периода может быть дробным, отрицательным или невалидным.
            // Приводим его к безопасной длине окна отдельной функцией.
            var window = ClampPeriod(periodValues[i]);

            // Вычисляем левую границу окна.
            // Если в начале истории баров еще недостаточно, используем все, что уже есть.
            var start = Math.Max(0, i - window + 1);

            // Начальное значение берем минимально возможным.
            // Так первое же значение источника внутри окна гарантированно сможет его заменить.
            var highest = double.MinValue;

            // Просматриваем все значения source внутри текущего адаптивного окна.
            for (var j = start; j <= i; j++)
            {
                // Если очередное значение выше текущего максимума,
                // обновляем максимум окна.
                if (sourceValues[j] > highest)
                    highest = sourceValues[j];
            }

            // Сохраняем итоговый максимум окна как значение индикатора на текущем баре.
            result[i] = highest;
        }

        // Возвращаем полностью сформированный ряд адаптивных максимумов.
        return result;
    }

    private static int ClampPeriod(double value)
    {
        // NaN и бесконечность не подходят для периода.
        // В таких случаях принудительно используем минимальное допустимое окно.
        if (double.IsNaN(value) || double.IsInfinity(value))
            return 1;

        // Округляем период по обычному математическому правилу.
        // После этого страхуемся от нуля и отрицательных значений.
        return Math.Max(1, (int)Math.Round(value, MidpointRounding.AwayFromZero));
    }
}
