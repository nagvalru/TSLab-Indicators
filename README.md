# TSLab Indicators

Набор пользовательских индикаторов и служебных блоков для TSLab.

## Что находится в репозитории

- `Indicators/` - исходные `.cs` файлы индикаторов и отдельные `.md` описания.
- `src/TSLabIndicators/TSLabIndicators.csproj` - общий проект сборки всех индикаторов в одну DLL.
- `dist/TSLabIndicators.dll` - готовая DLL для загрузки в TSLab.

## Индикаторы

- `KaufmanAdaptiveSMA` - адаптивная простая скользящая средняя с переменным периодом.
- `KaufmanAdaptiveHighest` - адаптивный максимум на переменном периоде.
- `KaufmanAdaptiveLowest` - адаптивный минимум на переменном периоде.
- `HasActivePositionByName` - проверка активной позиции по имени блока входа.
- `LinearRegressionEndpoint` - значение линии линейной регрессии в последней точке скользящего окна.

## Как загрузить DLL в TSLab

1. Скачайте файл `dist/TSLabIndicators.dll`.
2. Откройте TSLab. В программе: Файл - Настройки программы - Пути к папкам.
   Вам нужен "Путь к доп.библиотекам обработчиков"
3. Откройте путь в проводнике windows.
   Положите в папку Handlers DLL
4. Перезапустите программу TSLab
5. После загрузки блоки должны появиться в панели инструментов TSLab в соответствующих категориях.

## Как собрать DLL самостоятельно

Проект использует библиотеки TSLab как внешние ссылки. На рабочем компьютере путь к ним задан по умолчанию:

```powershell
dotnet build .\src\TSLabIndicators\TSLabIndicators.csproj -c Release
```

Если DLL TSLab лежат в другой папке, передайте путь явно:

```powershell
dotnet build .\src\TSLabIndicators\TSLabIndicators.csproj -c Release -p:TSLabRefsDir="C:\Path\To\TSLab"
```

Готовый файл после сборки находится здесь:

```text
dist/TSLabIndicators.dll
```

## Внешние ссылки

- [Клуб трейдеров TrendTeam](https://trendteam.pro/club)
