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
        static string Shortener(string longURL, int amountOfUse)
        {
            Random rnd = new Random();
            string shortenedURL = @"shorturl.at/";

            for (int i = 0; i < 5; i++)
            {
                switch (rnd.Next(0, 3))
                {
                    case 0:
                        {
                            char c = (char)rnd.Next(48, 57);
                            shortenedURL += c;
                        }
                        break;
                    case 1:
                        {
                            char c = (char)rnd.Next(65, 90);
                            shortenedURL += c;
                        }
                        break;
                    case 2:
                        {
                            char c = (char)rnd.Next(97, 122);
                            shortenedURL += c;
                        }
                        break;
                }
            }
           

            string connectionString = @"Server=DESKTOP-20VIBLO\SQLEXPRESS;Database=DB;Trusted_Connection=True;";

            string executeSql = $@"INSERT INTO [dbo].[Shortened_URL](long_URL,short_URL,amount_of_use)" +
                $"VALUES({longURL},{shortenedURL},{amountOfUse})";
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(executeSql, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
            }

            return shortenedURL;
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Введите URL: ");
            string URL = @""+Console.ReadLine();

            Console.WriteLine("Введите количество использования: ");
            int amountOfUse = Int32.Parse(Console.ReadLine());

            Shortener(URL, amountOfUse);

        }
    }
}
