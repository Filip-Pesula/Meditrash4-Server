using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace Meditrash4_Midpoint
{
    interface MysqlReadable
    {
        List<int> getPrimaryIndex();
        List<KeyValuePair<string,Type>> getMyTypeList();
        List<KeyValuePair<string,object>> getMySerValuesTypeList();
        void setMyData(List<Object> data);
        string writeQuerry();
        List<Object> getMyValues();
        string getMyName();
    }
}
