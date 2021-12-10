using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint
{
    internal class CompanyDataWrapper
    {
        public CompanyData _data { get; private set; }
        public CompanyDataWrapper()
        {
            JsonDocument doc;
            _data = new CompanyData();
            if (File.Exists(CompanyData.COMPANY_FILE_NAME))
            {
                string jsonTxt = File.ReadAllText(CompanyData.COMPANY_FILE_NAME);
                doc = JsonDocument.Parse(jsonTxt);
                _data = JsonSerializer.Deserialize<CompanyData>(jsonTxt);
            }
            else
            {
                string jsonTxt = JsonSerializer.Serialize(_data);
                File.WriteAllText(CompanyData.COMPANY_FILE_NAME, jsonTxt);
            }
        }
        public void save()
        {
            string jsonTxt = JsonSerializer.Serialize(_data);
            File.WriteAllText(CompanyData.COMPANY_FILE_NAME, jsonTxt);
        }
    }
}
