using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SimpleLibrary
{
    interface ILibraryAction
    {
        string Title { get; }
        void Perform(SqlConnection conn);
    }

    class FindBook : ILibraryAction
    {
        public string Title => "Find book";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            Console.Write("Part of title or author's surname or name: ");
            string input = Console.ReadLine();
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Books WHERE Title LIKE '%'+@Match+'%' OR Author LIKE '%'+@Match+'%'", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Match", input));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            TableView.Display(reader, ("ID", -4), ("Author", 30), ("Title", 40), ("Year", 4));
                        }
                        else
                        {
                            Console.WriteLine("No book matches these criteria");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Cannot find");
            }

            
        }
    }




    class FindReader : ILibraryAction
    {
        public string Title => "Find reader";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            Console.Write("Part of reader's surname or name, or ID: ");
            string input = Console.ReadLine();
            int id = 0;
            string command = "SELECT * FROM Readers WHERE Name LIKE '%'+@Match+'%' OR Surname LIKE '%'+@Match+'%'";
            try
            {
                id = int.Parse(input);
                command = "SELECT * FROM Readers WHERE ID = @Id";
            }
            catch { }

            try
            {
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Match", input));
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            TableView.Display(reader, ("ID", -4), ("Name", 20), ("Surname", 20), ("Debt ($)", -8));
                        }
                        else
                        {
                            Console.WriteLine("No one matches these criteria");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Cannot find");
            }


        }
    }





    class AddReader : ILibraryAction
    {
        public string Title => "Add reader";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            Console.Write("Reader's name: ");
            string name = Console.ReadLine();
            Console.Write("Reader's surname: ");
            string surname = Console.ReadLine();

            SqlUtils.NonQuery("INSERT INTO Readers (Name, Surname) VALUES (@Name, @Surname)", conn,
                    "Failure! Something went wrong!",
                    "Cannot insert",
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Surname", surname));
        }
    }





    class AddBook : ILibraryAction
    {
        public string Title => "Add book";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            Console.Write("Author: ");
            string author = Console.ReadLine();
            Console.Write("Title: ");
            string title = Console.ReadLine();

            int year = ConsoleUtils.ReadInt("Year: ", "This is an invalid year value, try again");

            SqlUtils.NonQuery("INSERT INTO Books (Author, Title, Year) VALUES (@Author, @Title, @Year)", conn,
                    "Failure! Something went wrong!",
                    "Cannot insert",
                    new SqlParameter("@Author", author),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Year", year));
        }
    }






    class RentBook : ILibraryAction
    {
        public string Title => "Rent book";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int reader;
            int book;

            while (true) 
            {
                reader = ConsoleUtils.ReadInt("Reader's ID: ", "This is not a valid ID, try again");

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Readers WHERE ID = @ID AND Debt = 0", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", reader));
                    int res = (int)cmd.ExecuteScalar();
                    if (res == 1) break;
                    Console.WriteLine("This user does not exist or cannot rent because of debt");
                }
            }

            while (true)
            {
                book = ConsoleUtils.ReadInt("Book's ID: ", "This is not a valid ID, try again");

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Books WHERE ID = @ID AND NOT EXISTS (SELECT * FROM Rentals WHERE BookID = @ID AND ReturnDate IS NULL)", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", book));
                    int res = (int)cmd.ExecuteScalar();
                    if (res == 1) break;
                    Console.WriteLine("This book does not exists or it is rented, try again");
                }
            }

            SqlUtils.NonQuery("INSERT INTO Rentals (BookID, ReaderID, RentalDate, ReturnDate) VALUES (@Book, @Reader, GETDATE(), NULL)", conn,
                    "Failure! Something went wrong!",
                    "Cannot rent",
                    new SqlParameter("@Book", book),
                    new SqlParameter("@Reader", reader));
        }
    }







    class ReturnBook : ILibraryAction
    {
        public string Title => "Return book";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");

            int book = ConsoleUtils.ReadInt("Book's ID: ", "This is not a valid ID, try again");

            SqlUtils.NonQuery("UPDATE Rentals SET ReturnDate = GETDATE() WHERE ReturnDate IS NULL AND BookID = @ID", conn,
                    "This book is not rented or does not exist",
                    "Cannot return",
                    new SqlParameter("@Id", book));
        }
    }






    class EditReadersData : ILibraryAction
    {
        public string Title => "Edit reader's data";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int readerid = ConsoleUtils.ReadInt("Reader's ID: ", "This is not a valid ID, try again");

            try
            {
                string name;
                string surname;

                using (SqlCommand cmd = new SqlCommand("SELECT Name, Surname FROM Readers WHERE ID = @ID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ID", readerid));
                  
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            name = reader.GetString(0);
                            surname = reader.GetString(1);
                        }
                        else
                        {
                            Console.WriteLine("No one has this ID");
                            return;
                        }
                    }
                }

                Console.Write($"New name (leave blank if no change)({name}): ");
                string newName = Console.ReadLine();

                Console.Write($"New surname (leave blank if no change)({surname}): ");
                string newSurname = Console.ReadLine();

                SqlUtils.NonQuery("UPDATE Readers SET Name = @Name, Surname = @Surname WHERE ID = @Id", conn,
                    "Failure! Something went wrong.",
                    "Cannot update",
                    new SqlParameter("@Id", readerid),
                    new SqlParameter("@Name", newName.Length == 0 ? name : newName),
                    new SqlParameter("@Surname", newSurname.Length == 0 ? surname : newSurname));
            }
            catch
            {
                Console.WriteLine("Cannot update");
            }


        }
    }










    class IncreaseReadersDebt : ILibraryAction
    {
        public string Title => "Increase reader's debt";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int readerid = ConsoleUtils.ReadInt("Reader's ID: ", "This is not a valid ID, try again");

            try
            {
                string name;
                string surname;
                decimal debt;

                using (SqlCommand cmd = new SqlCommand("SELECT Name, Surname, Debt FROM Readers WHERE ID = @ID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ID", readerid));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            name = reader.GetString(0);
                            surname = reader.GetString(1);
                            debt = (decimal) reader.GetValue(2);
                        }
                        else
                        {
                            Console.WriteLine("No one has this ID");
                            return;
                        }
                    }
                }

                decimal incDebt = ConsoleUtils.ReadDecimal($"Increase debt (${debt:0.00}) of {name} {surname} by $", "Faulty value, try again");

                SqlUtils.NonQuery("UPDATE Readers SET Debt = Debt + @Inc WHERE ID = @Id", conn,
                    "Failure! ID is probably wrong.",
                    "Cannot update",
                    new SqlParameter("@Id", readerid),
                    new SqlParameter("@Inc", incDebt));
            }
            catch
            {
                Console.WriteLine("Cannot update");
            }


        }
    }






    class CancelReadersDebt : ILibraryAction
    {
        public string Title => "Cancel reader's debt";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int readerid = ConsoleUtils.ReadInt("Reader's ID: ", "This is not a valid ID, try again");

            SqlUtils.NonQuery("UPDATE Readers SET Debt = 0 WHERE ID = @Id", conn,
                "Failure! ID is probably wrong.",
                "Cannot update",
                new SqlParameter("@Id", readerid));
        }
    }



    class RemoveReader : ILibraryAction
    {
        public string Title => "Remove reader";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int readerid = ConsoleUtils.ReadInt("Reader's ID: ", "This is not a valid ID, try again");

            try
            {
                string name;
                string surname;
                int rid;

                using (SqlCommand cmd = new SqlCommand("SELECT Name, Surname, ID FROM Readers WHERE ID = @ID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ID", readerid));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            name = reader.GetString(0);
                            surname = reader.GetString(1);
                            rid = (int)reader.GetValue(2);
                        }
                        else
                        {
                            Console.WriteLine("No one has this ID");
                            return;
                        }
                    }
                }
                Console.WriteLine($"Are you sure that you want to remove user {name} {surname} (ID {rid})?");
                Console.Write($"If so, type \"yes\": ");

                if(Console.ReadLine() == "yes")
                {
                    SqlUtils.NonQuery("DELETE Readers WHERE ID = @Id", conn,
                        "Failure! Something wrong.",
                        "Cannot update",
                        new SqlParameter("@Id", rid));
                }
                else
                {
                    Console.WriteLine("Aborted removal");
                }
                
            }
            catch
            {
                Console.WriteLine("Cannot update");
            }


        }
    }






    class SeeRentals : ILibraryAction
    {
        public string Title => "See current rentals";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int id = ConsoleUtils.ReadIntOrBlank("Reader's ID or blank for everyone: ", "This is not a valid ID");

            string command = "SELECT Books.ID, Books.Title, Books.Author, CONCAT(Readers.Name, ' ', Readers.Surname, ' (', Readers.ID, ')') as Reader, Rentals.RentalDate FROM Rentals JOIN Books ON Books.ID = BookID JOIN Readers ON Readers.ID = ReaderID WHERE ReturnDate IS NULL " + (id > 0 ? " AND ReaderID = @Id" : "") + " ORDER BY RentalDate";

            try
            {
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            TableView.Display(reader, ("Book", -4), ("Book's title", 35), ("Book's author", 26), ("Reader", 26), ("Rental date", 11));
                        }
                        else
                        {
                            Console.WriteLine("No rental matches these criteria");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Cannot find data");
            }
        }
    }



    class SeeAllRentals : ILibraryAction
    {
        public string Title => "See all rentals";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");
            int id = ConsoleUtils.ReadIntOrBlank("Reader's ID or blank for everyone: ", "This is not a valid ID");

            string command = "SELECT Books.ID, Books.Title, Books.Author, Readers.ID, Rentals.RentalDate, Rentals.ReturnDate FROM Rentals JOIN Books ON Books.ID = BookID JOIN Readers ON Readers.ID = ReaderID "+ (id > 0 ? "WHERE ReaderID = @Id": "") + " ORDER BY RentalDate";

            try
            {
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            TableView.Display(reader, ("Book", -4), ("Book's title", 35), ("Book's author", 26), ("Reader ID", 9), ("Rental date", 11), ("Return date", 11));
                        }
                        else
                        {
                            Console.WriteLine("No rental matches these criteria");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Cannot find data");
            }
        }
    }



    class Statistics : ILibraryAction
    {
        public string Title => "See statistics";

        public void Perform(SqlConnection conn)
        {
            Console.WriteLine($"-- {Title} --");

            string command = @"
                select top 1 (select sum(Debt) as DebtSum from Readers) as DebtSum,
                        (select count(*) from Books) as BooksCount,
                        (select count(*) from Readers) as ReadersCount,
                        (select count(*) from Rentals where ReturnDate is null) as CurrentRentalsCount,
                        (select count(*) from Rentals) as RentalsCount,
                        (select avg(Debt) as DebtAvg from Readers) as DebtAvg,
                        DebtData.*,
                        BestData.*,
                        BestBookData.*
                    from Readers r
                            join (select ID as DebtID, Name as DebtName, Surname as DebtSurname, Debt as DebtMax
                                   from Readers
                                   where debt in (select max(debt) from Readers)) as DebtData on 1 = 1

                            join(select bID as BestID, Name as BestName, Surname as BestSurname, BestCount
                                   from(select ReaderID as bID, count(*) as BestCount
                                        from Rentals group by ReaderID
                                        having count(*) = (
                                            select max(mycount)
                                            from(
                                                select ReaderID, count(*) mycount
                                                from Rentals
                                                group by ReaderID
                                            ) as ff))
                                       as dd join Readers on dd.bID = ID
                                ) as BestData on 1 = 1

                            join(select bID as BestBookID, Title as BestBookTitle, Author as BestBookAuthor, BestBookCount
                                   from(select BookID as bID, count(*) as BestBookCount
                                        from Rentals group by BookID
                                        having count(*) = (
                                            select max(mycount)
                                            from(
                                                select BookID, count(*) mycount
                                                from Rentals
                                                group by BookID
                                            ) as ff))
                                       as dd join Books on dd.bID = ID
                                ) as BestBookData on 1 = 1";
                                //uff

            try
            {
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Console.WriteLine($"\nGeneral: ");
                            Console.WriteLine($"\tBooks: {reader["BooksCount"]}");
                            Console.WriteLine($"\tReaders: {reader["ReadersCount"]}");
                            Console.WriteLine($"\tAll rented books: {reader["RentalsCount"]}");
                            Console.WriteLine($"\tCurrently rented books: {reader["CurrentRentalsCount"]}");
                            Console.WriteLine($"\nThe most popular book: ");
                            Console.WriteLine($"\tTitle: {reader["BestBookTitle"]}");
                            Console.WriteLine($"\tAuthor: {reader["BestBookAuthor"]}");
                            Console.WriteLine($"\tID: {reader["BestBookID"]}");
                            Console.WriteLine($"\tRented: {reader["BestBookCount"]} times");
                            Console.WriteLine($"\nThe most active reader: ");
                            Console.WriteLine($"\tName: {reader["BestName"]}");
                            Console.WriteLine($"\tSurname: {reader["BestSurname"]}");
                            Console.WriteLine($"\tID: {reader["BestID"]}");
                            Console.WriteLine($"\tRented: {reader["BestCount"]} books");
                            Console.WriteLine($"\nDebt: ");
                            Console.WriteLine($"\tSum: ${reader["DebtSum"]}");
                            Console.WriteLine($"\tAverage: ${reader["DebtAvg"]}");
                            Console.WriteLine($"\nMost indebted reader: ");
                            Console.WriteLine($"\tName: {reader["DebtName"]}");
                            Console.WriteLine($"\tSurname: {reader["DebtSurname"]}");
                            Console.WriteLine($"\tID: {reader["BestID"]}");
                            Console.WriteLine($"\tHis/her debt: ${reader["DebtMax"]}");
                        }
                        else
                        {
                            Console.WriteLine("No statistics available");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Cannot find data");
            }
        }
    }
}
