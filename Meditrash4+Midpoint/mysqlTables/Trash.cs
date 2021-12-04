using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class Trash : MysqlReadable
    {
        public int uid { get; private set; }
        public string name { get; private set; }
        public int cathegory { get; private set; }
        public int weight { get; private set; }
        public string getMyName()
        {
            return "Odpad";
        }

        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("uid", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("name", typeof(string)));
            a.Add(new KeyValuePair<string, Type>("category", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("weight_g", typeof(Int32)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(uid);
            a.Add(name);
            a.Add(cathegory);
            a.Add(weight);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("uid", MySqlDbType.Int32, uid));
            a.Add(new DbVariable("name", MySqlDbType.String, name));
            a.Add(new DbVariable("category", MySqlDbType.Int32, cathegory));
            a.Add(new DbVariable("weight_g", MySqlDbType.Int32, weight));
            return a;
        }

        public List<int> getPrimaryIndex()
        {
            return new List<int> { 0 };
        }

        public void setMyData(List<object> data)
        {
            try
            {
                uid = (int)data[0];
                name = (string)data[1];
                cathegory = (int)data[2];
                weight = (int)data[3];
            }
            catch (Exception e)
            {
                throw new UnmatchingTypeListException("User data format not correct", e);
            }
        }

        public string valueQuerry()
        {
            return "(uid,name,category,weight_g)";
        }

        public string writeQuerry()
        {
            return "(@uid,@name,@category,@weight_g)";
        }
    }
}
