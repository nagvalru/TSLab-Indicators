# LinearRegressionEndpoint

## Назначение

`LinearRegressionEndpoint` рассчитывает значение линии линейной регрессии в последней точке каждого скользящего окна.

Индикатор строит регрессионную прямую по последним `Period` значениям ряда `Source` и возвращает значение этой прямой на последнем баре окна. Это сглаженный регрессионный уровень, а не классическая скользящая средняя.

## Parameters

- `Period` - period of the rolling linear regression window.

## Параметры

- `Period` - период скользящего окна линейной регрессии.

## Inputs

- `Source` - input numeric series.

## Входы

- `Source` - входной числовой ряд.

## Output

- Numeric series with fitted regression endpoint values.

## Выход

- Числовой ряд со значениями линии линейной регрессии в конечной точке окна.

## Внешние ссылки

- [Клуб трейдеров TrendTeam](https://trendteam.pro/club)
