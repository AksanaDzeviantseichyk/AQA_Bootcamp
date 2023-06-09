using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;


namespace Task15.Pages
{
    public class ShoppingCart: BasePage
    {
        private By _deletebuttonLocator = By.ClassName("action-delete");

        public void DeleteAllProduct()
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            var deleteButtons = _driver.FindElements(_deletebuttonLocator);
            if (deleteButtons.Count > 0)
            {
                deleteButtons.First().Click();
                DeleteAllProduct();
            }
            
        }

    }
}
