using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class Department : MysqlReadable
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public Department(string name)
        {
            this.name = name;
            this.id = 0;
        }
        public Department()
        {
            this.name = "";
            this.id = 0;
        }

        public string getMyName()
        {
            return "Department";
        }
        

        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new  List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("uid", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("name", typeof(string)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(name);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("uid",MySqlDbType.Int32,id));
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
                throw new UnmatchingTypeListException("User data format not correct", e);
            }
        }

        public string writeQuerry()
        {
            return "(name)";
        }
        public string valueQuerry()
        {
            return "(@name)";
        }
    }
}
