using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class ExportRecords : MysqlReadable
    {
        public int uid { get; private set; }
        public DateTime exportDate { get; private set; }
        public long respPerson { get; private set; }
        public ExportRecords()
        {
            uid = 0;
            exportDate = DateTime.Now;
            respPerson = 0;
        }
        public ExportRecords(long respPerson)
        {
            uid = 0;
            exportDate = DateTime.Now;
            this.respPerson = respPerson;
        }
        public string getMyName()
        {
            return "ExportRecords";
        }

        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("uid", typeof(System.Int32)));
            a.Add(new KeyValuePair<string, Type>("exportDate", typeof(System.DateTime)));
            a.Add(new KeyValuePair<string, Type>("RespPerson_ico", typeof(System.Int64)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(uid);
            a.Add(exportDate);
            a.Add(respPerson);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("uid", MySqlDbType.Int32, uid));
            a.Add(new DbVariable("exportDate", MySqlDbType.Date, exportDate));
            a.Add(new DbVariable("RespPerson_ico", MySqlDbType.Int64, respPerson));
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
                exportDate = (DateTime)data[1];
                respPerson = (long)data[2];
            }
            catch (Exception e)
            {
                throw new UnmatchingTypeListException("ExportRecords data format not correct", e);
            }
        }

        public string valueQuerry()
        {
            return "(@exportDate,@RespPerson_ico)";
        }

        public string contentQuerry()
        {
            return "(exportDate,RespPerson_ico)";
        }
    }
}
