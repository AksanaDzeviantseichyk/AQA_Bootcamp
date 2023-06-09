using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Task15.Models;
using Task15.Enum;

namespace Task15.Driver
{
    public class BrowserFactory
    {
        public static IWebDriver CreateDriver(ConfigData configData)
        {
            switch (configData.BrowserName)
            {
                case Browsers.CHROME:
                    ChromeOptions options = new ChromeOptions();
                    foreach (var option in configData.Options)
                    {
                        options.AddArgument(option);
                    }
                    return new ChromeDriver(options);
                default:
                    throw new NotSupportedException($"Browser '{configData.BrowserName}' is not supported.");
            }           
        }
    }
}
