using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Meditrash4_Midpoint.tests")]

namespace Meditrash4_Midpoint
{
    class MySqlHandle
    {
        private static object connLock = new object();
        MySqlConnection conn;
        public MySqlHandle(ServerSetup sdata)
        {
            conn = new MySqlConnection(sdata.getConnectionString());
        }
        public void connect()
        {
            conn.Open();
        }
        public void close()
        {
            conn.Close();
        }
        public bool checkSql()
        {
            /*
            MySqlCommand cmd = new MySqlCommand(querry, conn);
            MySqlDataReader resultReader = cmd.ExecuteReader();
            List<object> returnVals = new List<object>();
            Console.WriteLine("usersFCount" + resultReader.FieldCount);
            if (resultReader.HasRows)
            {
                while (resultReader.Read())
                {
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        Console.WriteLine("usrType: " + resultReader.GetFieldType(i));
                    }
                }
            }
            resultReader.Close();
            */
            return false;
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
        public void setObjectParam<T>(T _object, string collum, string value) where T : MysqlReadable
        {
            List<DbVariable> vallist = _object.getObjectData();
            List<int> pkList = _object.getPrimaryIndex();
            string cond = "";
            foreach(int obj in _object.getPrimaryIndex())
            {
                cond += vallist[obj].name+ " '" + MySqlHelper.EscapeString(vallist[obj].value.ToString()) + "' AND";
            }
            value = MySqlHelper.EscapeString(value);
            MySqlCommand cmd = new MySqlCommand("UPDATE " + _object.getMyName() +  " SET " + collum + "='" +value+"'"+ " where " , conn);
            int execution = cmd.ExecuteNonQuery();
        }
        
        public static string genSelectCommandParamQ<T>(T _object) where T : MysqlReadable
        {
            StringBuilder values = new StringBuilder("(");
            List<DbVariable>  dataPairs = _object.getObjectData();
            foreach (DbVariable obj in dataPairs)
            {
                values.Append('@');
                values.Append(obj.name);
                values.Append(',');
            }
            values.Remove(values.Length-1,1);
            values.Append(")");
            return values.ToString();
        }
        public static void fillSelectCommandParamQ<T>(ref MySqlCommand cmd, T _object) where T : MysqlReadable
        {
            List<DbVariable> dataPairs = _object.getObjectData();
            foreach (DbVariable obj in dataPairs)
            {
                cmd.Parameters.Add('@' + obj.name, obj.type).Value = obj.value;
            }
        }
        public void saveObject<T>(T _object) where T : MysqlReadable
        {
            Exception e = null;


            Monitor.Enter(connLock);
            try
            {
                string valText = _object.valueQuerry();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + _object.getMyName() + _object.writeQuerry() + " VALUES " + valText + ";", conn);
                fillSelectCommandParamQ(ref cmd, _object);
                int execution = cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                e = ex;
            }
            finally
            {
                Monitor.Exit(connLock);
            }
            
            if (e != null)
            {
                throw e;
            }
        }
        public List<T> GetObjectList<T>(string condition, int max = -1) where T : MysqlReadable, new()
        {
            Exception e = null;
            Monitor.Enter(connLock);
            MySqlDataReader resultReader = null;
            try
            {
                T t = new T();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + t.getMyName() + " WHERE " + condition, conn);
                resultReader = cmd.ExecuteReader();
                List<T> returnVals = new List<T>();

                List<KeyValuePair<string, Type>> typelist = t.getMyTypeList();


                if (resultReader.FieldCount != typelist.Count)
                {
                    throw new UnmatchingTypeListException();
                }
                if (resultReader.HasRows)
                {
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        //TODO předělat na hashmap based on key
                        resultReader.GetName(i);
                        if (typelist[i].Value != resultReader.GetFieldType(i))
                        {
                            throw new UnmatchingTypeListException();
                        }
                    }

                    while (resultReader.Read())
                    {
                        List<Object> data = new List<Object>();
                        for (int i = 0; i < resultReader.FieldCount; i++)
                        {
                            object holderObj = resultReader.GetValue(i);
                            if (resultReader.IsDBNull(i))
                            {
                                holderObj = null;
                            }
                            data.Add(holderObj);
                        }
                        T _object = new T();
                        _object.setMyData(data);
                        returnVals.Add(_object);
                    }
                }
                return returnVals;
            }catch (Exception ex)
            {
                e = ex;
            }
            finally
            {
                if (resultReader != null)
                {
                    resultReader.Close();
                }
                Monitor.Exit(connLock);
            }
            if (e != null)
            {
                throw e;
            }
            return new List<T>();
        }
    }
}
