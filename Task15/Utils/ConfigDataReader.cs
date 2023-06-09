using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task15.Models;

namespace Task15.Utils
{
    public class ConfigDataReader
    {
        public static ConfigData ReadConfigData(string filePath)
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                ConfigData configData = JsonConvert.DeserializeObject<ConfigData>(jsonData);
                return configData;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (JsonException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
