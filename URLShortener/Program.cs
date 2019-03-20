using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener
{
    class Program
    {
        static string connectionString = @"Server=206_11;Database=DB;Trusted_Connection=True;";

        static string Shortener()
        {
            Console.WriteLine("Введите URL: ");
            string URL = Console.ReadLine();

            Console.WriteLine("Введите количество использования: ");
            int amount_of_use = Int32.Parse(Console.ReadLine());

            Random rnd = new Random();
            string short_URL = "shorturl.at/";

            for (int i = 0; i < 5; i++)
            {
                switch (rnd.Next(0, 3))
                {
                    case 0:
                        {
                            char c = (char)rnd.Next(48, 57);
                            short_URL += c;
                        }
                        break;
                    case 1:
                        {
                            char c = (char)rnd.Next(65, 90);
                            short_URL += c;
                        }
                        break;
                    case 2:
                        {
                            char c = (char)rnd.Next(97, 122);
                            short_URL += c;
                        }
                        break;
                }
            }

            string insertInfo = $"INSERT INTO [dbo].[Shortened_URL](long_URL,short_URL,amount_of_use)" +
                $"VALUES('{URL}','{short_URL}',{amount_of_use})";

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(insertInfo, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return short_URL;
        }
        static void FollowTheShortLink()
        {
            Console.WriteLine("Введите укороченный URL: ");
            string short_URL = Console.ReadLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string rowIsExistsSQL = $"SELECT DISTINCT * FROM [dbo].[Shortened_URL] WHERE short_URL='{short_URL}'";
                   
                    SqlCommand command = new SqlCommand(rowIsExistsSQL, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows!=true)
                    {
                        Console.WriteLine("Такого укороченного URL не существует!");
                        Console.WriteLine("\nНажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        string getAmount_Of_UseSQL = $"SELECT amount_of_use FROM [dbo].[Shortened_URL] WHERE short_URL='{short_URL}'";
                        reader.Close();
                        command = new SqlCommand(getAmount_Of_UseSQL, connection);
                        object amount_of_use = command.ExecuteScalar();

                        int x = Int32.Parse(amount_of_use.ToString());

                        string updateAmount_Of_UseSQL = $"UPDATE[dbo].[Shortened_URL] SET amount_of_use={--x} WHERE short_URL='{short_URL}'";
                        command = new SqlCommand(updateAmount_Of_UseSQL, connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static int GetPunctMenu()
        {
            return Int32.Parse(Console.ReadLine());
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Что вы хотите?");
                Console.WriteLine("1: Укоротить ссылку.");
                Console.WriteLine("2: Перейти по укороченной ссылке.");
                Console.WriteLine("0: Выйти");
                bool WannaGoOut = false;
                switch (GetPunctMenu())
                {
                    case 1:
                        {
                            Console.Clear();
                            Console.WriteLine("Ваша укороченная ссылка: {0}", Shortener());
                            Console.WriteLine("\nНажмите любую клавишу...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        break;
                    case 2:
                        {
                            Console.Clear();
                            FollowTheShortLink();
                            Console.WriteLine("\nНажмите любую клавишу...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        break;
                    case 0:
                        {
                            WannaGoOut = true;
                            Console.Clear();
                        }
                        break;
                }
                if (WannaGoOut == true)
                {
                    break;
                }
            }
        }
    }
}
