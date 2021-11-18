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
        List<Type> getMyTypeList();
        void setMyData(List<Object> data);
        string writeQuerry();
        List<Object> getMyValues();
        string getMyName();
    }
}
