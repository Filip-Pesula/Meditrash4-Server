using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class Cathegory : MysqlReadable
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public Cathegory(int id,string name)
        {
            this.id = id;
            this.name = name;
        }
        public Cathegory()
        {
            this.id = 0;
            this.name = "";
        }
        public string contentQuerry()
        {
            return "(id,name)";
        }

        public string getMyName()
        {
            return "TrashCathegody";
        }

        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("id", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("name", typeof(string)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(id);
            a.Add(name);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("uid", MySqlDbType.Int32, id));
            a.Add(new DbVariable("name", MySqlDbType.String, name));
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
                id = (int)data[0];
                name = (string)data[1];
            }
            catch (Exception e)
            {
                throw new UnmatchingTypeListException("Cathegory data format not correct", e);
            }
        }
        public string valueQuerry()
        {
            return "(@id,@name)";
        }
    }
}
