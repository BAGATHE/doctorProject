using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Docteur.Models
{
    internal class Connect
    {
        public NpgsqlConnection getConnectPostgres()
        {
            try
            {

                string connString = "Server=localhost;Port=5432;Database=docteur;User Id=postgres;Password=postgres;";
                Console.Write("Connecting to PostgreSQL Server ... ");
                NpgsqlConnection connection = new NpgsqlConnection(connString);
                Console.WriteLine(connection);
                connection.Open();
                Console.WriteLine("Done.");
                return connection;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
