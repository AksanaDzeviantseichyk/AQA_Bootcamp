using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Task15.Pages
{
    public class ProductPage: BasePage
    {
        private By _addToCartButtonLocator = By.ClassName("tocart");
        private By _counterProductInCartLocator = By.CssSelector(".counter.qty .counter-number");
        private By _loaderLocator = By.ClassName(".loader");

        public void ClickAddToCartButton()
        {
            //WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            //wait.Until(ExpectedConditions.ElementIsVisible(_addToCartButtonLocator));
            var addToCartButton = _driver.FindElement(_addToCartButtonLocator);
            addToCartButton.Click();
        }
        public string GetCounterValue()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            return _driver.FindElement(_counterProductInCartLocator).Text;
        }
    }
}
