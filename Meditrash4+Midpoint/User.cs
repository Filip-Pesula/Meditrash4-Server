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

        public long rod_cislo { get; private set; }
        public int rights { get; private set; }
        public int department_id { get; private set; }
        public string firstName { get; private set; }
        public string lastName { get; private set; }
        public User()
        {
            this.userName = "";
            this.rod_cislo = -1;
            this.rights = 0;
            this.department_id = -1;
            this.firstName = "";
            this.lastName = "";
        }
        public User(string userName, int rod_cislo, int departmentid, string firstName, string lastName)
        {
            this.userName = userName;
            this.rod_cislo = rod_cislo;
            this.rights = 0;
            this.department_id = departmentid;
            this.firstName = firstName;
            this.lastName = lastName;
        }
        public void setMyData(List<Object> data)
        {
            try
            {
                rod_cislo = (long)data[0];
                rights = (int)data[1];
                userName = (string)data[2];
                firstName = (string)data[3];
                lastName = (string)data[4];
                department_id = (int)data[5];
            }
            catch(Exception e)
            {
                throw new UnmatchingTypeListException("User data format not correct", e);
            }
        }

        public string getMyName()
        {
            return "User";
        }




        public string writeQuerry()
        {
            return "(name,rodCislo,Department_uid,firstName,lastName)";
        }

        public List<object> getMyValues()
        {
            List<object>  a =  new List<object>();
            a.Add(rod_cislo);
            a.Add(rights);
            a.Add(userName);
            a.Add(firstName);
            a.Add(lastName);
            a.Add(department_id);
            return a;
        }

        public List<int> getPrimaryIndex()
        {
            List<int> pkList = new List<int>();
            pkList.Add(1);
            return pkList;
        }

        List<KeyValuePair<string, Type>> MysqlReadable.getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string,Type>("rodCislo", typeof(System.Int64)));
            a.Add(new KeyValuePair<string,Type>("userRights", typeof(System.Int32)));
            a.Add(new KeyValuePair<string,Type>("name", typeof(System.String)));
            a.Add(new KeyValuePair<string,Type>("firstName", typeof(System.String)));
            a.Add(new KeyValuePair<string,Type>("lastName", typeof(System.String)));
            a.Add(new KeyValuePair<string,Type>("Department_uid", typeof(System.Int32)));
            return a;

        }
    }
}
