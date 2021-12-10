using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Meditrash4_Midpoint
{
    class CompanyData
    {
        public static string COMPANY_FILE_NAME = "companyData.json";
        public string ico { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string ulice { get; set; }
        public string mesto { get; set; }
        public string psc { get; set; }
        public string zuj { get; set; }
        public CompanyData()
        {
            ico = "";
            name = "";
            id = "";
            ulice = "";
            mesto = "";
            psc = "";
            zuj = "";
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
            zuj = root.GetProperty("yuj").GetString();
        }
    }
}
