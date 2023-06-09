using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace Task15.Pages
{
    public class ProductPage: BasePage
    {
        private By _addToCartButtonLocator = By.ClassName("tocart");
        private By _counterProductInCartLocator = By.CssSelector(".counter.qty .counter-number");
        
        public void ClickAddToCartButton()
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            var addToCartButton = _driver.FindElement(_addToCartButtonLocator);
            addToCartButton.Click();
        }
        public string GetCounterValue()
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            return _driver.FindElement(_counterProductInCartLocator).Text;
        }
    }
}
