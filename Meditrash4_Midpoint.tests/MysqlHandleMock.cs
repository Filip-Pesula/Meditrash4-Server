using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Meditrash4_Midpoint.tests
{
    internal class MysqlHandleMock : MySqlHandle
    {
        
        public void mockConnect(MySqlConnection mySqlConnection)
        {
        }
    }
}
