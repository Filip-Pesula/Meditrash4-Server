using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Meditrash4_Midpoint
{
    class ServerData
    {
        public string ServerAddres { get; set; }
        public string User { get; set; }
        public string Database { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
        public ServerData()
        {
            ServerAddres = "localhost";
            User = "root";
            Database = "db0";
            Port = "3306";
            Password = "";
        }
        public ServerData(JsonDocument doc)
        {
            JsonElement root = doc.RootElement;
            ServerAddres = root.GetProperty("ServerAddres").GetString();
            User = root.GetProperty("User").GetString();
            Database = root.GetProperty("Database").GetString();
            Port = root.GetProperty("Port").GetString();
            Password = root.GetProperty("Password").GetString();
        }
        public string getConnectionString()
        {
            return "server=" + ServerAddres
                + ";user=" + User
                + ";database=" + Database
                + ";port=" + Port
                + ";password=" + Password;
        }
    }
}
