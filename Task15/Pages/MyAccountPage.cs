using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
