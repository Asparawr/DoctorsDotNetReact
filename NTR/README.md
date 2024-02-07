# NTR laboratorium

[Get started with ASP.NET Core MVC](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-6.0&tabs=visual-studio)

## Treść zadania

Napisać program do obsługi przychodni.

### Zadanie 1 - MVC

Lista lekarzy - każdy lekarz ma jedną specjalność ze stałej listy specjalnosci (domowy, laryngolog, dermatolog, okulista, neurolog, ortopeda, pediatra).
Konto lekarza zakłada dyrektor.
Lekaz loguje sie za pomocą hasła.

Lista pacjentów (logowanie z hasłem)
Pacjent rejestruje się w systemie (zakłada konto), ale zapisy do lekarzy i przegladanie wizyt po uaktywnieniu konta przez dyrektora.


Grafik na każdy kolejny tydzien: lekaz, dzien, start, koniec  np: Kowalski, 2023-11-08 8.00 12.00. W ramach jednego wpisu w grafiku moze odbyć się wiele wizyt.
Grafik wpisuje/modyfikuje dyrektor (dodatkowa opcja: kopiuj porzedni tydzień).

### Uruchomienie rozwiązania:
- Instalacja podstawowego net8.0 oraz instalacja paczek:
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

- Upewnienie się, że instalacja zawiera Identities
- Build rozwiązania w folderze \NTR\
- host na https://localhost:7248/