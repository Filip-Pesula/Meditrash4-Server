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
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        Console.WriteLine("usrType: " + resultReader.GetFieldType(i));
                    }
                }
            }
            resultReader.Close();
            return returnVals;
        }
        public void saveObject<T>(T _object) where T : MysqlReadable
        {
            StringBuilder values = new StringBuilder("(");
            foreach (Object obj in _object.getMyValues())
            {
                switch (obj)
                {
                    case System.DateTime:
                        values.Append("'");
                        values.Append(MySqlHelper.EscapeString(((DateTime)obj).ToString("yyyy-MM-dd")));
                        values.Append("'");
                        break;
                    default:
                        values.Append("'");
                        values.Append(MySqlHelper.EscapeString(obj.ToString()));
                        values.Append("'");
                        break;
                }
               
                values.Append(",");
            }
            values.Remove(values.Length - 1, 1);
            values.Append(")");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO " + _object.getMyName() + _object.writeQuerry() + " VALUES " + values.ToString() + ";", conn);
            int execution = cmd.ExecuteNonQuery();
        }
        public List<T> GetObjectList<T>(string condition, int max = -1) where T : MysqlReadable, new()
        {
            T t = new T();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + t.getMyName() + " WHERE " + condition, conn);
            MySqlDataReader resultReader = cmd.ExecuteReader();
            List<T> returnVals = new List<T>();
            Console.WriteLine("usersFCount" + resultReader.FieldCount);

            List<Type> typelist =  t.getMyTypeList();


            if (resultReader.FieldCount != typelist.Count)
            {
                throw new UnmatchingTypeListException();
            }
            if (resultReader.HasRows) {
                for (int i = 0; i < resultReader.FieldCount; i++)
                {
                    if(typelist[i]!= resultReader.GetFieldType(i))
                    {
                        throw new UnmatchingTypeListException();
                    }
                    Console.WriteLine("usrType: " + resultReader.GetFieldType(i));
                }

                while (resultReader.Read())
                {
                    List<Object> data = new List<Object>();
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        data.Add(resultReader.GetValue(i));
                    }
                    T _object = new T();
                    _object.setMyData(data);
                    returnVals.Add(_object);
                }
            }
            resultReader.Close();
            return returnVals;
        }
    }
}
