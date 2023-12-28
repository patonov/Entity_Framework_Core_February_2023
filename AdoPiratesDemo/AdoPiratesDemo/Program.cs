using System;
using System.Data.SqlClient;

namespace AdoPiratesDemo
{
    public class Program
    {
        private const string connectingString = @"Server=DESKTOP-79AOHR2;Database=Pirates;Integrated Security=true";

        static void Main(string[] args)
        {
            
            using SqlConnection connection = new SqlConnection(connectingString);

                connection.Open();
                string command = "SELECT COUNT(*) FROM Ships";

                SqlCommand cmd = new SqlCommand(command, connection);

                int result = (int)cmd.ExecuteScalar();

                Console.WriteLine(result);

            Console.WriteLine("================================================");

            string piratesNamesCommand = "SELECT Names FROM Pirates";

            SqlCommand namesCmd = new SqlCommand(piratesNamesCommand, connection);

            SqlDataReader reader = namesCmd.ExecuteReader();

            using (reader)
            { 
                while (reader.Read()) 
                {
                    string names = (string)reader["Names"];
                    Console.WriteLine(names);
                }
            }

            Console.WriteLine("*************************************************");

            SqlCommand shipNamesCommand = new SqlCommand(CommandStrings.SelectShipNames, connection);
            SqlDataReader shipNamesReader = shipNamesCommand.ExecuteReader();
            using (shipNamesReader) 
            { 
                while(shipNamesReader.Read()) 
                { 
                string name = (string)shipNamesReader["Name"];
                Console.WriteLine(name);
                }
            }
            Console.WriteLine("adoadoadoadoadoadoadoadoadoadoadoadoadoadoadoadoado");
        }   
    }
}