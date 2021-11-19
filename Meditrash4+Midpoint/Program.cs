using System;
using System.Collections.Generic;
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

            AppConnector appConnector = new AppConnector(mySqlHandle);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                mySqlHandle.connect();


                string sql = "SELECT* FROM users";
                //mySqlHandle.querry(sql,10);
                String testName = MySqlHelper.EscapeString("ROOT");

                List<User> users = mySqlHandle.GetObjectList<User>("name="+"'"+testName+ "'", 2);
                if (users.Count!=1) {
                    Logger.Log("user ROOT not found");
                }
                /*
                User userToset = new User("petr.novak", 5,1,"Petr","Novak");
                try
                {
                    mySqlHandle.saveObject(userToset);
                }
                catch(MySqlException e)
                {

                    Logger.LogE("writing user error", e);
                }
                */

            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
            }
            Console.WriteLine("PressEnter to exit");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { };

            mySqlHandle.close();
            appConnector.stop();
            setup.save();
            Logger.Log("Done.",ConsoleColor.Green);
        }
    }
}
