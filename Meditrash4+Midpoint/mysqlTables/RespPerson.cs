using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.mysqlTables
{
    internal class RespPerson : MysqlReadable
    {
        public long ico { get; private set; }
        public string name { get; private set; }
        public string ulice { get; private set; }
        public int cislo_popisne { get; private set; }
        public string mesto { get; private set; }
        public int psc { get; private set; }
        public int zuj { get; private set; }
        public RespPerson()
        {
            ico = 0;
            name = "";
            ulice = "";
            cislo_popisne = 0;
            mesto = "";
            psc = 0;
            zuj = 0;
        }
        public RespPerson(long ico,string name, string ulice, int cislo_popisne, string mesto, int psc, int zuj)
        {
            this.ico = ico;
            this.name = name;
            this.ulice = ulice;
            this.cislo_popisne = cislo_popisne;
            this.mesto = mesto;
            this.psc = psc;
            this.zuj = zuj;
        }
        public string contentQuerry()
        {
            return "(ico,name,ulice,cislo_popisne,mesto,PSC,ZUJ)";
        }

        public string getMyName()
        {
            return "RespPerson";
        }

        public List<KeyValuePair<string, Type>> getMyTypeList()
        {
            List<KeyValuePair<string, Type>> a = new List<KeyValuePair<string, Type>>();
            a.Add(new KeyValuePair<string, Type>("ico", typeof(System.Int64)));
            a.Add(new KeyValuePair<string, Type>("name", typeof(System.String)));
            a.Add(new KeyValuePair<string, Type>("ulice", typeof(System.String)));
            a.Add(new KeyValuePair<string, Type>("cislo_popisne", typeof(System.Int32)));
            a.Add(new KeyValuePair<string, Type>("mesto", typeof(System.String)));
            a.Add(new KeyValuePair<string, Type>("PSC", typeof(System.Int32)));
            a.Add(new KeyValuePair<string, Type>("ZUJ", typeof(System.Int32)));
            return a;
        }

        public List<object> getMyValues()
        {
            List<object> a = new List<object>();
            a.Add(ico);
            a.Add(name);
            a.Add(ulice);
            a.Add(cislo_popisne);
            a.Add(mesto);
            a.Add(psc);
            a.Add(zuj);
            return a;
        }

        public List<DbVariable> getObjectData()
        {
            List<DbVariable> a = new List<DbVariable>();
            a.Add(new DbVariable("ico", MySqlDbType.Int64, ico));
            a.Add(new DbVariable("name", MySqlDbType.Int32, name));
            a.Add(new DbVariable("ulice", MySqlDbType.VarChar, ulice));
            a.Add(new DbVariable("cislo_popisne", MySqlDbType.VarChar, cislo_popisne));
            a.Add(new DbVariable("mesto", MySqlDbType.VarChar, mesto));
            a.Add(new DbVariable("PSC", MySqlDbType.Binary, psc));
            a.Add(new DbVariable("ZUJ", MySqlDbType.Int32, zuj));
            return a;
        }

        public List<int> getPrimaryIndex()
        {
            List<int> pkList = new List<int>();
            pkList.Add(1);
            return pkList;
        }

        public void setMyData(List<object> data)
        {
            try
            {
                ico = (long)data[0];
                name = (string)data[1];
                ulice = (string)data[2];
                cislo_popisne = (int)data[3];
                mesto = (string)data[4];
                psc = (int)data[5];
                zuj = (int)data[6];
            }
            catch (Exception e)
            {
                throw new UnmatchingTypeListException("RespPerson data format not correct", e);
            }
        }

        public string valueQuerry()
        {
            return "(@ico,@name,@ulice,@cislo_popisne,@mesto,@PSC,@ZUJ)";
        }
    }
}
