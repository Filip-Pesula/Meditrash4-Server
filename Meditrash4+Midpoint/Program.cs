using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace Meditrash4_Midpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSetup setup = new ServerSetup("confugFile.json");
            string connStr = setup.getConnectionString();

            MySqlHandle mySqlHandle = new MySqlHandle(setup);
           
            

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                mySqlHandle.connect();


                string sql = "SELECT* FROM users";
                mySqlHandle.querry(sql,10);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            mySqlHandle.close();
            Console.WriteLine("Done.");
            setup.save();
        }
    }
}
