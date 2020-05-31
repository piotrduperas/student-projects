using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SimpleLibrary
{
    static class TableView
    {
        public static void Display(SqlDataReader reader, params (string title, int size)[] fields)
        {
            StringBuilder sbHorizontal = new StringBuilder();
            foreach (var f in fields)
            {
                sbHorizontal.Append("+");
                for (int i = 0; i < Math.Abs(f.size); i++)
                {
                    sbHorizontal.Append("-");
                }
            }
            sbHorizontal.Append("+");
            string horizontal = sbHorizontal.ToString();

            Console.WriteLine(horizontal);
            foreach (var f in fields)
            {
                Console.Write("|" + centeredString(f.title, Math.Abs(f.size)));
            }
            Console.WriteLine("|");
            Console.WriteLine(horizontal);
            while (reader.Read())
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    string val = parse(reader.GetValue(i));
                    string s;
                    if (fields[i].size > 0)
                    {
                        s = leftAlignString(val, fields[i].size);
                    }
                    else
                    {
                        s = rightAlignString(val, -fields[i].size);
                    }
                    Console.Write($"|{s}");
                }
                Console.WriteLine("|");
                Console.WriteLine(horizontal);

            }
        }

        static string parse(object val)
        {
            if (val is Decimal)
            {
                return String.Format("{0:0.00}", (Decimal)val);
            }
            else if (val is DateTime)
            {
                return ((DateTime)val).ToString("yyyy.MM.dd");
            }
            return val.ToString();
        }

        static string centeredString(string s, int width)
        {
            if (s.Length >= width)
            {
                return s;
            }

            int leftPadding = (width - s.Length) / 2;
            int rightPadding = width - s.Length - leftPadding;

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }

        static string rightAlignString(string s, int width)
        {
            if (s.Length >= width)
            {
                return s;
            }

            int leftPadding = (width - s.Length);

            return new string(' ', leftPadding) + s;
        }

        static string leftAlignString(string s, int width)
        {
            if (s.Length >= width)
            {
                return s;
            }

            int rightPadding = width - s.Length;

            return s + new string(' ', rightPadding);
        }
    }

    static class ConsoleUtils
    {
        public static int ReadInt(string prompt, string error)
        {
            do
            {
                try
                {
                    Console.Write(prompt);
                    int n = int.Parse(Console.ReadLine());
                    if (n > 0) return n;
                }
                catch { }
                Console.WriteLine(error);
            } while (true);
        }

        public static int ReadIntOrBlank(string prompt, string error)
        {
            do
            {
                try
                {
                    Console.Write(prompt);
                    string s = Console.ReadLine();
                    if (s.Length == 0) return 0;
                    int n = int.Parse(s);
                    if (n > 0) return n;
                }
                catch { }
                Console.WriteLine(error);
            } while (true);
        }

        public static decimal ReadDecimal(string prompt, string error)
        {
            do
            {
                try
                {
                    Console.Write(prompt);
                    decimal n = decimal.Parse(Console.ReadLine());
                    return n;
                }
                catch { }
                Console.WriteLine(error);
            } while (true);
        }
    }

    static class SqlUtils
    {
        public static void NonQuery(string command, SqlConnection conn, string fail, string error, params SqlParameter[] sqlParams)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    foreach (var p in sqlParams)
                    {
                        cmd.Parameters.Add(p);
                    }

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        Console.WriteLine("Success!");
                    }
                    else
                    {
                        Console.WriteLine(fail);
                    }
                }
            }
            catch
            {
                Console.WriteLine(error);
            }
        }
    }
}
