# MagazynManager

![.NET Core](https://github.com/Saalin/MagazynManager/workflows/.NET%20Core/badge.svg)  [![Codacy Badge](https://api.codacy.com/project/badge/Coverage/7002a609712a4204ad8759825e7c8fe8)](https://www.codacy.com/manual/saalin/MagazynManager?utm_source=github.com&utm_medium=referral&utm_content=Saalin/MagazynManager&utm_campaign=Badge_Coverage)  [![Codacy Badge](https://api.codacy.com/project/badge/Grade/7002a609712a4204ad8759825e7c8fe8)](https://www.codacy.com/manual/saalin/MagazynManager?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Saalin/MagazynManager&amp;utm_campaign=Badge_Grade)

Wersja demonstracyjna aplikacji dostępna jest pod adresem http://172.104.158.90/index.html, tam też znajdują sie szczegóły dotyczące jej działania

## Informacje
  - zbudowanie aplikacji wymaga .NET Core SKD 3.1
  - aplikacja wymaga do działania instancji SQL Server 2019, ciąg połączenia podawany jest w `appsettings.json` w projekcie `MagazynManager.Server`
  - kompilacja wyzwalana jest poleceniem `dotnet build`
  - testy wyzwalane są poleceniem `dotnet test`, przed uruchomieniem należy zmienić ciąg połączenia w projekcie `Magazyn.Tests`
