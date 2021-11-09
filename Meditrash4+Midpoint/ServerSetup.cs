using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;



namespace Meditrash4_Midpoint
{
    class ServerSetup
    {
        ServerData sData;
        string setupFile;
        public ServerSetup(string setupFile)
        {
            this.setupFile = setupFile;
            JsonDocument doc;
            sData = new ServerData();
            if (File.Exists(setupFile))
            {
                string jsonTxt = File.ReadAllText(setupFile);
                doc = JsonDocument.Parse(jsonTxt);
                sData = JsonSerializer.Deserialize<ServerData>(jsonTxt);
            }
            else
            {
            }
        }
        public string getConnectionString()
        {
            return "server=" + sData.ServerAddres 
                +";user=" + sData.User
                +";database=" + sData.Database
                +";port=" + sData.Port
                +";password=" + sData.Password;
        }
        public void save()
        {
            string jsonTxt = JsonSerializer.Serialize(sData);
            File.WriteAllText(setupFile, jsonTxt);
        }

    }
}
