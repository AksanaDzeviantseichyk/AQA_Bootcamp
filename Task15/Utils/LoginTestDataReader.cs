﻿using Newtonsoft.Json;
using Task15.Models;

namespace Task15.Utils
{
    public static class LoginTestDataReader
    {
        public static LoginTestData ReadLoginTestData(string filePath)
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                LoginTestData testData = JsonConvert.DeserializeObject<LoginTestData>(jsonData);
                return testData;
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
