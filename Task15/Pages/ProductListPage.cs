using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace Task15.Pages
{
    public class ProductListPage : BasePage
    {
        private By _productInfoElementLocator = By.ClassName("product-item-info");
        private By _productInfoNames = By.ClassName("product-item-link");
        private By _toCartButton = By.ClassName("tocart");
        private By _loaderLocator = By.ClassName(".loader");

        public void ScrollToProducts()
        {
            var elements = _driver.FindElements(_productInfoElementLocator);
            ScrollToElement(elements.First());
        }

        public void AddProductToCartByName(string productName)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(drv => drv.FindElement(_productInfoElementLocator));
            IWebElement targetProduct = _driver
                .FindElements(_productInfoElementLocator)
                .Where(i => i.FindElement(_productInfoNames).Text.Equals(productName)).First();

            MoveToElement(targetProduct);
            
            IWebElement productAddToCartButton = targetProduct.FindElement(_toCartButton);

            productAddToCartButton.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
        }
        public void AddProductToCartByNumber(int productNumber)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));
            var productList = wait.Until(drv => drv.FindElements(_productInfoElementLocator));
            IWebElement targetProduct = productList[productNumber];
            MoveToElement(targetProduct);
            
            IWebElement productAddToCartButton = targetProduct.FindElement(_toCartButton);

            productAddToCartButton.Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
        }

        public ProductPage OpenProductByNumber(int productNumber)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));
            var productList = wait.Until(drv => drv.FindElements(_productInfoElementLocator));
            IWebElement targetProduct = productList[productNumber];
            MoveToElement(targetProduct);
            
            //IWebElement productAddToCartButton = targetProduct.FindElement(_toCartButton);

            targetProduct.Click();
            // wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            return new ProductPage();
        }

       
    }
}
