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
        public User()
        {
            this.userName = "";
            this.bDate = DateTime.MinValue;
            this.id = -1;
        }
        public User(string userName, DateTime bDate)
        {
            this.userName = userName;
            this.bDate = bDate;
            this.id = -1;
        }
        public void setMyData(List<Object> data)
        {
            try
            {
                userName = (string)data[0];
                bDate = (DateTime)data[1];
                id = (int)data[2];
            }
            catch(Exception e)
            {
                throw new UnmatchingTypeListException("User data format not correct", e);
            }
            
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



        public string writeQuerry()
        {
            return "(name,birth)";
        }

        public List<object> getMyValues()
        {
            List<object>  a =  new List<object>();
            a.Add(userName);
            a.Add(bDate.Date);
            return a;
        }
    }
}
