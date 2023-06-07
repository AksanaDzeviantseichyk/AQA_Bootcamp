using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task15.Pages
{
    public class MyOrdersPage:BasePage
    {
        private By _orderRowLocator = By.XPath(".//td[@class='col id']");
        private string _viewOrderButtonLocator = $".//td[@class='col id' and contains(text(),'{0}')]//..//a[@class='action view']";

        public OrderPage ClickViewOrderButton(string orderNumber)
        {
            var orderRows = _driver.FindElements(_orderRowLocator);
                
            IWebElement order = orderRows.First(i => i.Text.Equals(orderNumber));
            var viewOrderButton = _driver.FindElement(By.XPath("//..//a[@class='action view']"));

            //var viewOrderButton = _driver.FindElement(By.XPath(string.Format(_viewOrderButtonLocator, orderNumber)));
            // ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
            viewOrderButton.Click();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(6));
            wait.Until((driver) => !driver.Title.StartsWith("My Orders"));
            return new OrderPage();
        }
    }
}
