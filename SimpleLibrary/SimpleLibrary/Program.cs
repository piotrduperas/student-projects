using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace SimpleLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = "Server=localhost\\MSSQLSERVER,1433;Database=Library;Trusted_Connection=True;";

            Console.WriteLine("------ LIBRARY MANAGER ------");
            Console.WriteLine("Choose authentication method: ");
            Console.WriteLine("1. Windows Authentication");
            Console.WriteLine("2. Database login & password");
            Console.WriteLine("Other - exit");

            int choice = ConsoleUtils.ReadInt("Choose: ", "Choose 1, or 2, or any other number to exit");

            if(choice == 2)
            {
                Console.Write("Database server: ");
                string server = Console.ReadLine();
                Console.Write("Database port: ");
                string port = Console.ReadLine();
                Console.Write("Database instance: ");
                string ins = Console.ReadLine();
                Console.Write("Database name: ");
                string database = Console.ReadLine();
                Console.Write("Database user: ");
                string user = Console.ReadLine();
                Console.Write("Database password: ");
                string password = Console.ReadLine();
                connString = $"Server={server},{port}\\{ins};Database={database};User Id={user};Password={password};";
            }
            else if(choice != 1)
            {
                return;
            }

            string loginConnString = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    Console.WriteLine("Error: Cannot connect to database!");
                    return;
                }

                List<ILibraryAction> options = new List<ILibraryAction>();
                options.Add(new FindBook());
                options.Add(new FindReader());
                options.Add(new AddReader());
                options.Add(new AddBook());
                options.Add(new RentBook());
                options.Add(new ReturnBook());
                options.Add(new EditReadersData());
                options.Add(new RemoveReader());
                options.Add(new IncreaseReadersDebt());
                options.Add(new CancelReadersDebt());
                options.Add(new SeeRentals());
                options.Add(new SeeAllRentals());
                options.Add(new Statistics());



                int option;

                while(true)
                {
                    option = DisplayMenu(options);

                    if (option == 0) break;

                    if(option <= options.Count)
                    {
                        options[option - 1].Perform(conn);
                        Console.WriteLine("Press ENTER...");
                        Console.ReadLine();
                    }
                }
            }
        }

        static int DisplayMenu(List<ILibraryAction> options)
        {
            Console.WriteLine("\n\n------ LIBRARY MANAGER ------\n");
            Console.WriteLine("Choose an option:");

            for(int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{(i + 1) + ".",-3} {options[i].Title}");
            }
            Console.WriteLine("0.  Quit");
            Console.Write("Choice: ");
            int option;

            try
            {
                option = int.Parse(Console.ReadLine());
            }
            catch
            {
                option = int.MaxValue;
            }
            Console.WriteLine("");
            return option;
        }
    }
}
