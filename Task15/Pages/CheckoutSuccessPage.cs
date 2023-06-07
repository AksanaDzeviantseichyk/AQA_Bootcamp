using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task15.Pages
{
    public class CheckoutSuccessPage:BasePage
    {
        private By _orderNumberLocator = By.ClassName("order-number");
        private By _continueShoppingButtonLocator = By.ClassName("continue");

        public string GetOrderNumber()
        {
            
            return _driver.FindElement(_orderNumberLocator).Text;
        }

        public void ClickContinueShoppingButton()
        {
            var continueShoppingButton = _driver.FindElement(_continueShoppingButtonLocator);
            continueShoppingButton.Click();
        }
    }
}
