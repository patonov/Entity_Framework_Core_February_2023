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

            //SqlCommand insertCmd = new SqlCommand(CommandStrings.InsertPirateAndPlunder, connection);
            //int output = insertCmd.ExecuteNonQuery();
            //Console.WriteLine(output);

            SqlCommand sqlCommandSearchingParticipantsInPlunder = new SqlCommand(CommandStrings.SearchParticipantsInPlunderFor("Madagascar"), connection);

            try
            {
                SqlDataReader whatIsDone = sqlCommandSearchingParticipantsInPlunder.ExecuteReader();
                using (whatIsDone)
                {
                    Console.WriteLine("Pirate's Names");
                    while (whatIsDone.Read())
                    {
                        string names = (string)whatIsDone["Pirate's Names"];
                        Console.WriteLine(names);
                    }
                }                
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

            Console.WriteLine("tralalalaltralalalaltralalallaltralalal");

            SqlCommand berthsCommand = new SqlCommand(CommandStrings.ReportAboutFreeBerthsOnBoardOfShip(60), connection);

            object berths = berthsCommand.ExecuteScalar();
            
            if (berths != null)
            {
                Console.WriteLine($"Free Berths on the board of the Ship you are interested: {berths}");
            }
            else
            {
                Console.WriteLine("You have a mistake.");
            }

        }   

    }
}