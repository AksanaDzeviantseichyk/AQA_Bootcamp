using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task15.Driver
{
    public class DriverSingleton
    {
        private static Lazy<IWebDriver> _lazyDriver = new Lazy<IWebDriver>(() => BrowserFactory.CreateDriver());
        public static IWebDriver Driver => _lazyDriver.Value;

        private readonly string _baseUrl = "https://magento.softwaretestingboard.com/men.html";
        public void OpenBaseUrl()
        {
            _lazyDriver.Value.Navigate().GoToUrl(_baseUrl);
        }

    }
}
