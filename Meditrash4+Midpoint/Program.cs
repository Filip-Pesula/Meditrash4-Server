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
            CompanyData companyData = new CompanyData();
            string connStr = setup.getConnectionString();

            MySqlHandle mySqlHandle = new MySqlHandle();

            AppConnector appConnector = new AppConnector(mySqlHandle);

            menu.Menu menu = new menu.Menu(setup.GetServerData());
            menu.setConnectCallBack(() =>
            {
                mySqlHandle.close();
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    mySqlHandle.connect(setup);
                    Console.Clear();
                    menu.setConnected();
                }
                catch (Exception ex)
                {
                    menu.setNotConnected();
                    Logger.LogE(ex);
                }
            });
            menu.setResetCallBack(() =>
            {
                mySqlHandle.reset();
            });
            menu.setSaveCallBack(() =>
            {
                setup.save();
            });

            menu.setTestCallBack(() => {
                List<string> errorList = mySqlHandle.checkSql();
                if (errorList.Count == 0)
                {
                    Logger.Log("All Tabs Correct",ConsoleColor.Green);
                }
                else{
                    Logger.Log("Tabs Error", ConsoleColor.Red);
                    errorList.ForEach((x) =>
                    {
                        Logger.Log(x, ConsoleColor.Red);
                    });
                }
                Logger.Log("Press Enter");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {

                }
            });
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                mySqlHandle.connect(setup);
                Console.Clear();
                menu.setConnected();
            }
            catch (Exception ex)
            {
                menu.setNotConnected();
                Logger.LogE(ex);
            }

            try
            {
                Console.Clear();

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

            int menuSelectIndex = 0;
            bool end = false;
            
            while (!menu.run()) {
            };

            mySqlHandle.close();
            appConnector.stop();
            setup.save();
            Logger.Log("Done.",ConsoleColor.Green);
        }
    }
}
