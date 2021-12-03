using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Meditrash4_Midpoint
{
    internal class DbVariable
    {
        public string name { get; set; }
        public MySqlDbType type { get; set; }
        public Object value { get; set; }

        public DbVariable(string name, MySqlDbType type, object value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
}
