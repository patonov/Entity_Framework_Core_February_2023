using System;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdoNetDemoExercise
{
    class Program
    {
        private const string ConnectionString = @"Server=DESKTOP-79AOHR2;Database=MinionsDB;Integrated Security=true";

        static async Task Main()
        {

            await using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();

            //Console.WriteLine(await GetVillainsWithMinions(sqlConnection));
            Console.WriteLine(await GetVillainWithAllMiniosByIdAsync(sqlConnection, 2));
        }

        static async Task<string> GetVillainsWithMinions(SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand sqlCommand = new SqlCommand(SQLqueries.VillainsWithMinions, sqlConnection);
            SqlDataReader result = await sqlCommand.ExecuteReaderAsync();

           
                while (result.Read())
                {
                    string name = (string)result["Name"];
                    int count = (int)result["MinionsCount"];

                    sb.AppendLine($"Name => {name}, Count => {count}");
                }
            
            return sb.ToString().TrimEnd();
        }


        static async Task<string> GetVillainWithAllMiniosByIdAsync(SqlConnection sqlConnection, int id)
        {
            SqlCommand sqlCommand = new SqlCommand(SQLqueries.VillainById, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);

            object? villNameObj = await sqlCommand.ExecuteScalarAsync();

            if (villNameObj == null)
            {
                return "No such an annimal!!!";
            }
            string villainName = (string)villNameObj;

            StringBuilder sb = new StringBuilder();

            SqlCommand newSqlCommand = new SqlCommand(SQLqueries.AllMinsByVillainName, sqlConnection);
            newSqlCommand.Parameters.AddWithValue("@Id", id);

            var result = await newSqlCommand.ExecuteReaderAsync();

            sb.AppendLine($"Villain: {villainName}");

            if (!result.HasRows)
            {
                sb.AppendLine("(no minions)");
            }
            else
            {
                while (result.Read())
                {
                    long rowNum = (long)result["RowNum"];
                    string name = (string)result["Name"];
                    int age = (int)result["Age"];

                    sb.AppendLine($"{rowNum}. {name} {age}");
                }

            }
            return sb.ToString().Trim();
        }

        
    }
}
