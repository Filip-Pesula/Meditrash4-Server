using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Meditrash4_Midpoint.tests")]

namespace Meditrash4_Midpoint
{
    interface MysqlReadable
    {
        List<int> getPrimaryIndex();
        List<KeyValuePair<string,Type>> getMyTypeList();
        List<DbVariable> getObjectData();
        void setMyData(List<Object> data);
        string contentQuerry();
        public string valueQuerry();
        List<Object> getMyValues();
        string getMyName();
    }
}
