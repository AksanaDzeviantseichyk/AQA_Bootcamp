using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task15.Models;
using Task15.Utils;

namespace Task15.Driver
{
    public class DriverSingleton
    {
        private static Lazy<IWebDriver> _lazyDriver;
        private static IWebDriver _driverInstance;
        public static IWebDriver Driver => _driverInstance ?? throw new NullReferenceException("Driver instance has not been initialized.");
        private static readonly ConfigData _configData = ConfigDataReader.ReadConfigData("Resourses\\ConfigData.json");
         public static void QuitDriver()
        {
            if (_driverInstance != null)
            {
                _driverInstance.Quit();
                _driverInstance = null;
            }
        }

        public static void InitializeDriver()
        {
            if (_driverInstance == null)
            {
                _lazyDriver = new Lazy<IWebDriver>(() => BrowserFactory.CreateDriver(_configData));
                _driverInstance = _lazyDriver.Value;
                _driverInstance.Navigate().GoToUrl(_configData.BaseUrl);
                _driverInstance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_configData.SmallWait);
                _driverInstance.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_configData.MediumWait);
            }
        }
    }
}
