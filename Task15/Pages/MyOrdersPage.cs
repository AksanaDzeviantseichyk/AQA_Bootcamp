using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Task15.Pages
{
    public class MyOrdersPage:BasePage
    {
        private By _orderRowLocator = By.XPath(".//td[@class='col id']");
        private By _viewOrderButtonLocator = By.XPath("//..//a[@class='action view']");

        public OrderPage ClickViewOrderButton(string orderNumber)
        {
            var orderRows = _driver.FindElements(_orderRowLocator);
                
            IWebElement order = orderRows.First(i => i.Text.Equals(orderNumber));
            var viewOrderButton = _driver.FindElement(_viewOrderButtonLocator);

            viewOrderButton.Click();
            _wait.Until((driver) => !driver.Title.StartsWith("My Orders"));
            return new OrderPage();
        }
    }
}
