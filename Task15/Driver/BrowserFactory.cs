using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Task15.Driver
{
    public class BrowserFactory
    {
        public static IWebDriver CreateDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--incognito");
            return new ChromeDriver(options);
        }
    }
}
