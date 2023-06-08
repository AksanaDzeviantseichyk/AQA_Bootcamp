using OpenQA.Selenium;

namespace Task15.Pages
{
    public class MyAccountPage:BasePage
    {
        private By _myOrdersButtonLocator = By.XPath("//a[contains(text(), 'My Orders')]");

        public MyOrdersPage ClickMyOrdersButton()
        {
            var myOrdersButton = _driver.FindElement(_myOrdersButtonLocator);
            myOrdersButton.Click();
            return new MyOrdersPage();
        }
    }
}
