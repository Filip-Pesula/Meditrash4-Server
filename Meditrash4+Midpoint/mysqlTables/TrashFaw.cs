using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class TrashFaw : MysqlReadable
    {
        public long user_rod_c { get; private set; }
        public int odpad_uid { get; private set; }
        public TrashFaw()
        {
            this.user_rod_c = 0;
            this.odpad_uid = 0;
        }
        public TrashFaw(long user_rod_c, int odpad_uid)
        {
            this.user_rod_c = user_rod_c;
            this.odpad_uid = odpad_uid;
        }
        public string getMyName()
        {
            return "Odpad_User_Settings";
        }

        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("User_rodCislo", typeof(Int64)));
            a.Add(new KeyValuePair<string, Type>("Odpad_uid", typeof(Int32)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(user_rod_c);
            a.Add(odpad_uid);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("User_rodCislo", MySqlDbType.Int32, user_rod_c));
            a.Add(new DbVariable("Odpad_uid", MySqlDbType.String, odpad_uid));
            return a;
        }

        public List<int> getPrimaryIndex()
        {
            return new List<int> { 0 , 1 };
        }

        public void setMyData(List<object> data)
        {
            try
            {
                user_rod_c = (long)data[0];
                odpad_uid = (int)data[1];
            }
            catch (Exception e)
            {
                throw new UnmatchingTypeListException("TrashFaw data format not correct", e);
            }
        }

        public string valueQuerry()
        {
            return "(@User_rodCislo,@Odpad_uid)";
        }

        public string contentQuerry()
        {
            return "(User_rodCislo,Odpad_uid)";
        }
    }
}
