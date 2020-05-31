=SimpleLibrary=

Projekt prostej aplikacji do obsługi biblioteki, z użyciem bazy MSSQL.

==1. Kompilacja==
Powinno wystarczyć skompilowanie w Visual Studio (pisane w VS 2019).

==2. Utworzenie bazy danych==
Należy wykonać polecenia zawarte w plikach .sql.

==3. Połączenie z bazą danych==
Przy otwieraniu aplikacji jest wybór metody uruchomienia. Można wybrać
Windows Authentication z localhost lub połączyć się ze zdalnym serwerem.
Aby to zrobić należy podać dane do połączenia:
- host
- port
- instancja bazy danych
- nazwa bazy danych
- użytkownik
- hasło

==3. Zasady działania==
Są trzy tabele: 
- Readers - informacje o czytelnikach biblioteki: imię, nazwisko, zaległości w płatnościach
- Books - informacje o książkach: tytuł, autor, rok wydania
- Rentals - informacje o wypożyczeniach: nr książki, nr czytelnika, data wypożyczenia, data zwrotu (null jeśli nadal wypożyczona)

Aplikacja pozwala przeglądać czytelników, książki oraz wypożyczenia na podstawie różnych kryteriów.
Można też dodawać nowych czytelników, nowe książki, wypożyczać i oddawać książki, oraz usuwać czytelników.
Można również edytować dane czytelników i modyfikować ich stan należności do zapłacenia. Czytelnik nie może wypożyczyć książki
dopóki jego dług nie zostanie anulowany.
Jako raport, można zobaczyć statystyki biblioteki, m.in. ilości książek, czytelników, średni poziom należności, najaktywniejszego czytelnika itp.

