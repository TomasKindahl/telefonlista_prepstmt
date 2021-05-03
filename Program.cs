using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Tools → Nuget Package Manager → Package Manager Console
 * Uninstall-Package MySqlConnector -Version 1.3.7
 * Install-Package MySqlConnector -Version 1.3.7
 * Källa: https://www.nuget.org/packages/MySqlConnector/
 */

namespace telefonlista_prepstmt
{
    class Program
    {
        static string connString;
        static string table = "contactlist";

        static void LoadDatabaseCredentials()
        {
            string line;
            string User_ID = "", Password = "", Database = "";
            StreamReader file = new StreamReader(@"credentials.txt");
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
                string[] tagval = line.Split('=');
                switch (tagval[0])
                {
                    case "User ID":
                        User_ID = tagval[1].Trim();
                        break;
                    case "Password":
                        Password = tagval[1].Trim();
                        break;
                    case "Database":
                        Database = tagval[1].Trim();
                        break;
                }
            }
            connString = $"Server=localhost;User ID={User_ID};Password={Password};Database={Database}";
        }

        static void DeleteID(int ID)
        {
            using (var connection = new MySqlConnection(connString))
            {
                string sql = $"DELETE FROM {table} WHERE ID = {ID};";
                Console.WriteLine("Kommando: " + sql);
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) ;
                    }
                }
                connection.Close();
                Console.WriteLine($"ID {ID} was deleted.");
            }
        }

        static void InsertNew(int ID, string fornamn, string efternamn, string gatuadress, string telefon, DateTime fodelsedag)
        {
            /* Not yet implemented: använd kod från ditt inlämningsprojekt,
             *   och lägg till prepared statements! */
        }
        static void ListTable()
        {
            /* Not yet implemented: använd kod från ditt inlämningsprojekt! 
             *   Observera att prepared statements inte behövs här. */
        }
        static void UpdateColumn(int ID, string column, string newValue)
        {
            /* Not yet implemented: använd kod från ditt inlämningsprojekt,
             *   och lägg till prepared statements! */
        }
        static string GetString(string prompt)
        {
            Console.Write(prompt + ": ");
            return Console.ReadLine();
        }
        static int GetInt(string prompt)
        {
            int result;
            while (!int.TryParse(GetString(prompt), out result))
            {
                Console.WriteLine("Inte ett heltal, försök igen!");
            }
            return result;
        }
        static DateTime GetDateTime(string prompt)
        {
            DateTime result;
            while (!DateTime.TryParse(GetString(prompt), out result))
            {
                Console.WriteLine("Inte ett datum, försök igen!");
            }
            return result;
        }
        static void Main(string[] args)
        {
            string[] menu =
            {
                "+-------kommandon------+",
                "| 1. visa kontakter    |",
                "| 2. lägg till kontakt |",
                "| 3. ta bort kontakt   |",
                "| 4. uppdatera kontakt |",
                "| 5. avsluta           |",
                "+----------------------+"
            };
            Console.WriteLine("Hej och välkommen till telefonlistan!");
            LoadDatabaseCredentials();
            string command;
            bool end = false;
            do
            {
                for (int i = 0; i < menu.Length; i++)
                {
                    Console.WriteLine(menu[i]);
                }
                Console.Write("Ange ett kommando: ");
                command = Console.ReadLine();
                int ID;
                switch (command)
                {
                    case "1":
                        Console.WriteLine("Visa listan av kontakter:");
                        ListTable();
                        break;
                    case "2":
                        Console.WriteLine("Lägg till en ny kontakt:");
                        ID = GetInt("Ange ID");
                        string fornamn = GetString("Ange förnamn");
                        string efternamn = GetString("Ange efternamn");
                        string gatuadress = GetString("Ange gatuadress");
                        string telefon = GetString("Ange telefon");
                        DateTime fodelsedag = GetDateTime("Ange födelsedag");
                        InsertNew(ID, fornamn, efternamn, gatuadress, telefon, fodelsedag);
                        break;
                    case "3":
                        Console.WriteLine("Ta bort en kontakt:");
                        ID = GetInt("Ange ID");
                        DeleteID(ID);
                        break;
                    case "4":
                        Console.WriteLine("Uppdatera en kontakt:");
                        ID = GetInt("Ange ID");
                        string column = GetString("Ange parameter du vill ändra:\n  förnamn, "
                                                 + "efternamn, gatuadress, telefon, födelsedag\n  parameter");
                        string newVal = GetString("Ange nytt värde");
                        UpdateColumn(ID, column, newVal);
                        break;
                    case "5":
                        Console.WriteLine("Hej då!");
                        end = true;
                        break;
                    default:
                        Console.WriteLine("Ogiltig val.");
                        break;
                }
            } while (!end);
        }
    }
}
