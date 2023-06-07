using Newtonsoft.Json;
using Task15.Models;

namespace Task15.Utils
{
    public static class LoginTestDataReader
    {
        private static readonly string _filePath = "Resourses\\LoginTestData.json";
        public static LoginTestData ReadLoginTestData()
        {
            try
            {
                string jsonData = File.ReadAllText(_filePath);
                LoginTestData testData = JsonConvert.DeserializeObject<LoginTestData>(jsonData);
                return testData;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.FileName}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }
    }
}
