using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Meditrash4_Midpoint
{
    class CompanyData
    {
        public static COMPANY_FILE_NAME = "companyData.json";
        public string ico { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string ulice { get; set; }
        public string mesto { get; set; }
        public string psc { get; set; }
        public string zuj { get; set; }
        public CompanyData()
        {
            JsonDocument doc;
            sData = new ServerData();
            if (File.Exists(companyData))
            {
                string jsonTxt = File.ReadAllText(companyData);
                doc = JsonDocument.Parse(jsonTxt);
                CompanyData = JsonSerializer.Deserialize<ServerData>(jsonTxt);
            }
            else
            {
                string jsonTxt = JsonSerializer.Serialize(CompanyData);
                File.WriteAllText(companyData, jsonTxt);
            }
        
        }
        public CompanyData(JsonDocument doc)
        {
            JsonElement root = doc.RootElement;
            ico = root.GetProperty("ico").GetString();
            name = root.GetProperty("name").GetString();
            id = root.GetProperty("id").GetString();
            ulice = root.GetProperty("ulice").GetString();
            mesto = root.GetProperty("mesto").GetString();
            psc = root.GetProperty("psc").GetString();
            yuj = root.GetProperty("yuj").GetString();
        }
        
        public void save()
        {
            string jsonTxt = JsonSerializer.Serialize(sData);
            File.WriteAllText(COMPANY_FILE_NAME, jsonTxt);
        }
    }
}
