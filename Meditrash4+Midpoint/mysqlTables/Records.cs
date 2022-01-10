using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class Records : MysqlReadable
    {
        public int uid { get; private set; }
        public DateTime date { get; private set; }
        public int amount { get; private set; }
        public int Odpad_uid { get; private set; }
        public long User_rodCislo { get; private set; }
        public int? DeStoreRecords_uid { get; private set; }
        public string getMyName()
        {
            return "Records";
        }
        public Records()
        {
            uid = 0;
            date = DateTime.Now;
            this.amount = 0;
            this.Odpad_uid = 0;
            this.User_rodCislo = 0;
            this.DeStoreRecords_uid = null;
        }
        public Records(int amount, int odpad_uid, long user_rocCislo)
        {
            uid = 0;
            date = DateTime.Now;
            this.amount = amount;
            this.Odpad_uid = odpad_uid;
            this.User_rodCislo = user_rocCislo;
            DeStoreRecords_uid = null;
        }
        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("uid", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("storageDate", typeof(DateTime)));
            a.Add(new KeyValuePair<string, Type>("amount", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("Odpad_uid", typeof(Int32)));
            a.Add(new KeyValuePair<string, Type>("User_rodCislo", typeof(Int64)));
            a.Add(new KeyValuePair<string, Type>("DeStoreRecords_uid", typeof(Int32)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(uid);
            a.Add(date);
            a.Add(amount);
            a.Add(Odpad_uid);
            a.Add(User_rodCislo);
            a.Add(DeStoreRecords_uid);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("uid", MySqlDbType.Int32, uid));
            a.Add(new DbVariable("storageDate", MySqlDbType.DateTime, date));
            a.Add(new DbVariable("amount", MySqlDbType.Int32, amount));
            a.Add(new DbVariable("Odpad_uid", MySqlDbType.Int32, Odpad_uid));
            a.Add(new DbVariable("User_rodCislo", MySqlDbType.Int64, User_rodCislo));
            a.Add(new DbVariable("DeStoreRecords_uid", MySqlDbType.Int64, DeStoreRecords_uid));
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
                date = (DateTime)data[1];
                amount = (int)data[2];
                Odpad_uid = (int)data[3];
                User_rodCislo = (long)data[4];
                DeStoreRecords_uid = (int?)data[5];
            }
            catch (Exception e)
            {
                throw new UnmatchingTypeListException("Records data format not correct", e);
            }
        }

        public string valueQuerry()
        {
            return "(@storageDate,@amount,@Odpad_uid,@User_rodCislo,@DeStoreRecords_uid)";
        }

        public string contentQuerry()
        {
            return "(storageDate,amount,Odpad_uid,User_rodCislo,DeStoreRecords_uid)";
        }
    }
}
