using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace Task15.Pages
{
    public class ProductListPage: BasePage
    {
        private By _productInfoElementLocator = By.ClassName("product-item-info");
        private By _productInfoNames = By.ClassName("product-item-link");
        private By _toCartButton = By.ClassName("tocart");
        private By _loaderLocator = By.ClassName(".loader");

        public void ScrollToProducts()
        {
            var elements = _driver.FindElements(_productInfoElementLocator);

            Actions actions = new Actions(_driver);
            actions.ScrollToElement(elements.First());
            actions.Perform();
        }

        public void AddProductToCart(string productName)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(drv => drv.FindElement(_productInfoElementLocator));
            IWebElement targetProduct = _driver
                .FindElements(_productInfoElementLocator)
                .Where(i => i.FindElement(_productInfoNames).Text.Equals(productName)).First();

            Actions actions = new Actions(_driver);
            actions.MoveToElement(targetProduct);
            actions.Perform();

            IWebElement productAddToCartButton = targetProduct.FindElement(_toCartButton);

            productAddToCartButton.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
        }


    }
}
