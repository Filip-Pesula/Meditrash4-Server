using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Meditrash4_Midpoint
{
    class MySqlHandle
    {
        MySqlConnection conn;
        public MySqlHandle(ServerSetup sdata)
        {
            conn = new MySqlConnection(sdata.getConnectionString());
        }
        public void connect()
        {
            try
            {
                conn.Open();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void close()
        {
            conn.Close();
        }

        public List<object> querry(string querry, int max = -1)
        {
            MySqlCommand cmd = new MySqlCommand(querry, conn);
            MySqlDataReader resultReader = cmd.ExecuteReader();
            List<object> returnVals = new List<object>();
            Console.WriteLine("usersFCount" + resultReader.FieldCount);
            if (resultReader.HasRows) {
                while (resultReader.Read())
                {
                    Console.WriteLine("usrName:"+resultReader.GetString(0));
                }
            }
            
            return returnVals;
        }
    }
}
