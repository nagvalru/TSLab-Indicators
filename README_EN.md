**TSLab Indicators**

A collection of custom indicators and utility blocks for TSLab.

## What is in the repository

- `Indicators/` – source `.cs` files of indicators and individual `.md` descriptions.
- `src/TSLabIndicators/TSLabIndicators.csproj` – common build project for compiling all indicators into a single DLL.
- `dist/TSLabIndicators.dll` – ready-to-use DLL for loading into TSLab.

## Indicators

- `KaufmanAdaptiveSMA` – adaptive simple moving average with a variable period.
- `KaufmanAdaptiveHighest` – adaptive highest high over a variable period.
- `KaufmanAdaptiveLowest` – adaptive lowest low over a variable period.
- `HasActivePositionByName` – checks for an active position by entry block name.
- `LinearRegressionEndpoint` – value of the linear regression line at the last point of a moving window.

## How to load the DLL into TSLab

1. Download the `dist/TSLabIndicators.dll` file.
2. Open TSLab. In the program: File → Program Settings → Paths.  
   You need the "External handlers folder path".
3. Open the path in Windows Explorer.  
   Place the DLL into the `Handlers` folder.
4. Restart TSLab.
5. Once loaded, the blocks should appear in the TSLab toolbox, under their respective categories.

## How to build the DLL yourself

The project references TSLab libraries as external dependencies. On a development machine, the default path is pre‑configured:

```powershell
dotnet build .\src\TSLabIndicators\TSLabIndicators.csproj -c Release
```

If your TSLab DLLs are located in a different folder, pass the path explicitly:

```powershell
dotnet build .\src\TSLabIndicators\TSLabIndicators.csproj -c Release -p:TSLabRefsDir="C:\Path\To\TSLab"
```

After the build, the resulting file is located at:

```text
dist/TSLabIndicators.dll
```

## External links

- [TrendTeam Traders Club](https://trendteam.pro/club)
