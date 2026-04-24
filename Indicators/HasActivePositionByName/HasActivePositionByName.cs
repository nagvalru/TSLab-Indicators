using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

[HandlerCategory("Position")]
[DisplayName("Есть активная позиция по имени")]
[HelperName("Has active position by name", Language = "en-us")]
[HelperName("Есть активная позиция по имени", Language = "ru-ru")]
[Description(
    "Блок: Есть активная позиция по имени\r\n\r\n" +
    "Проверяет, есть ли на текущем баре активная позиция, открытая блоком открытия позиции с заданным именем. " +
    "Вход блока такой же, как у штатного блока \"Есть активная позиция\": источник инструмента SECURITY.\r\n\r\n" +
    "Параметры\r\n\r\n" +
    "• Имя блока открытия - точное имя блока открытия позиции, например openLong.\r\n\r\n" +
    "Входы\r\n\r\n" +
    "• Source - источник инструмента SECURITY.\r\n\r\n" +
    "Тип выхода\r\n\r\n" +
    "• Логический ряд BOOL: true, если на баре активна позиция с EntrySignalName, равным указанному имени.\r\n\r\n" +
    "Внешние ссылки\r\n\r\n" +
    "• Клуб трейдеров TrendTeam\r\n" +
    "• https://trendteam.pro/club")]
[HelperDescription(
    "Returns true on bars where a position opened by the specified entry block name is active.",
    Language = "en-us")]
[HelperDescription(
    "Возвращает true на барах, где активна позиция, открытая блоком с указанным именем.",
    Language = "ru-ru")]
[HelperLink("https://trendteam.pro/club", Name = "TrendTeam Traders Club", Language = "en-us")]
[HelperLink("https://trendteam.pro/club", Name = "Клуб трейдеров TrendTeam", Language = "ru-ru")]
[InputsCount(1)]
[Input(0, TemplateTypes.SECURITY, Name = "SECURITYSource")]
[OutputsCount(1)]
[OutputType(TemplateTypes.BOOL)]
public sealed class HasActivePositionByName : IBar2BoolHandler
{
    [DisplayName("Имя блока открытия")]
    [HelperName("Entry block name", "en-us")]
    [HelperName("Имя блока открытия", "ru-ru")]
    [HelperDescription("Exact code/name of the entry block that opened the position.", "en-us")]
    [HelperDescription("Точное имя блока открытия позиции, например openLong.", "ru-ru")]
    [HandlerParameter(true, "openLong", Name = "EntryBlockName", NotOptimized = true)]
    public string EntryBlockName { get; set; } = "openLong";

    public bool Execute(ISecurity source, int barNum)
    {
        return ActivePositionByNameCore.Execute(source, barNum, EntryBlockName);
    }
}

[HandlerCategory("Position")]
[DisplayName("Есть активная позиция по имени")]
[HelperName("Has active position by name", Language = "en-us")]
[HelperName("Есть активная позиция по имени", Language = "ru-ru")]
[Description(
    "Блок: Есть активная позиция по имени\r\n\r\n" +
    "Проверяет, есть ли на текущем баре активная позиция, открытая блоком открытия позиции с заданным именем. " +
    "Вход блока такой же, как у штатного блока \"Есть активная позиция\": источник инструмента SECURITY.\r\n\r\n" +
    "Параметры\r\n\r\n" +
    "• Имя блока открытия - точное имя блока открытия позиции, например openLong.\r\n\r\n" +
    "Входы\r\n\r\n" +
    "• Source - источник инструмента SECURITY.\r\n\r\n" +
    "Тип выхода\r\n\r\n" +
    "• Логический ряд BOOL: true, если на баре активна позиция с EntrySignalName, равным указанному имени.\r\n\r\n" +
    "Внешние ссылки\r\n\r\n" +
    "• Клуб трейдеров TrendTeam\r\n" +
    "• https://trendteam.pro/club")]
[HelperDescription(
    "Returns true on bars where a position opened by the specified entry block name is active.",
    Language = "en-us")]
[HelperDescription(
    "Возвращает true на барах, где активна позиция, открытая блоком с указанным именем.",
    Language = "ru-ru")]
[HelperLink("https://trendteam.pro/club", Name = "TrendTeam Traders Club", Language = "en-us")]
[HelperLink("https://trendteam.pro/club", Name = "Клуб трейдеров TrendTeam", Language = "ru-ru")]
[InputsCount(1)]
[Input(0, TemplateTypes.SECURITY, Name = "SECURITYSource")]
[OutputsCount(1)]
[OutputType(TemplateTypes.BOOL)]
public sealed class ЕстьАктивнаяПозицияПоИмени : IBar2BoolHandler
{
    [DisplayName("Имя блока открытия")]
    [HelperName("Entry block name", "en-us")]
    [HelperName("Имя блока открытия", "ru-ru")]
    [HelperDescription("Exact code/name of the entry block that opened the position.", "en-us")]
    [HelperDescription("Точное имя блока открытия позиции, например openLong.", "ru-ru")]
    [HandlerParameter(true, "openLong", Name = "EntryBlockName", NotOptimized = true)]
    public string EntryBlockName { get; set; } = "openLong";

    public bool Execute(ISecurity source, int barNum)
    {
        return ActivePositionByNameCore.Execute(source, barNum, EntryBlockName);
    }
}

internal static class ActivePositionByNameCore
{
    public static bool Execute(ISecurity source, int barNum, string entryBlockName)
    {
        var signalName = entryBlockName?.Trim();
        if (source?.Positions is null || string.IsNullOrEmpty(signalName))
            return false;

        var positions = source.Positions;
        var activePosition = positions.GetLastActiveForSignal(signalName, barNum);
        if (activePosition is not null)
            return true;

        if (!source.IsRealtime || source.SimulatePositionOrdering)
            return false;

        var lastForSignal = positions.GetLastForSignal(signalName, positions.BarsCount);
        return lastForSignal?.IsActiveForBar(barNum) == true;
    }
}
