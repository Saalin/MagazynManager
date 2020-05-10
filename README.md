# MagazynManager

![.NET Core](https://github.com/Saalin/MagazynManager/workflows/.NET%20Core/badge.svg)  [![codecov](https://codecov.io/gh/Saalin/MagazynManager/branch/master/graph/badge.svg)](https://codecov.io/gh/Saalin/MagazynManager)

Wersja demonstracyjna aplikacji dostępna jest pod adresem http://172.104.158.90/index.html, tam też znajdują sie szczegóły dotyczące jej działania

## Informacje
- zbudowanie aplikacji wymaga .NET Core SKD 3.1
- aplikacja wymaga do działania instancji SQL Server 2019, ciąg połączenia podawany jest w `appsettings.json` w projekcie `MagazynManager.Server`
- kompilacja wyzwalana jest poleceniem `dotnet build`
- testy wyzwalane są poleceniem `dotnet test`, przed uruchomieniem należy zmienić ciąg połączenia w projekcie `Magazyn.Tests`
