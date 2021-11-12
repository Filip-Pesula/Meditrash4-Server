using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint
{
    class User : MysqlReadable
    {
        public string userName { get; private set; }
        public DateTime bDate { get; private set; }
        public int id { get; private set; }
        public void getMyData(List<Object> data)
        {
            userName = (string)data[0];
            bDate = (DateTime)data[1];
            id = (int)data[2];
        }

        public string getMyName()
        {
            return "users";
        }

        public List<Type> getMyTypeList()
        {
            List <Type>  a = new List<Type>();
            a.Add(typeof(System.String));
            a.Add(typeof(System.DateTime));
            a.Add(typeof(System.Int32));
            return  a;
        }
    }
}
