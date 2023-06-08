using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;

using Task15.Driver;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Task15.Pages
{
    public class BasePage
    {
        private By _gearCategoryButtonLocator = By.Id("ui-id-6");
        private By _gearSubmenuItemsLocator = By.CssSelector(".level0.nav-4 li");
        private By _proceedToCheckoutButtonLocator = By.Id("top-cart-btn-checkout");
        private By _cartIconLocator = By.ClassName("minicart-wrapper");
       
        protected IWebDriver _driver => DriverSingleton.Driver;

        protected void MoveToElement(IWebElement element)
        {
            Actions actions = new Actions(_driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        protected void ScrollToElement(IWebElement element)
        {
            Actions actions = new Actions(_driver);
            actions.ScrollToElement(element);
            actions.Perform();
        }
        public ProductListPage ClickGearSubmenuItem(string subMenuName)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            
            var elementToHover = wait.Until(ExpectedConditions.ElementIsVisible(_gearCategoryButtonLocator));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(elementToHover).Perform();
            wait.Until(ExpectedConditions.ElementIsVisible(_gearSubmenuItemsLocator));
            var gearSubmenuItems = _driver.FindElements(_gearSubmenuItemsLocator);
            IWebElement targetGearSubmenu = gearSubmenuItems.FirstOrDefault(i => i.Text.Equals(subMenuName));
            targetGearSubmenu.Click();
            
            wait.Until((driver) => !driver.Title.StartsWith("Home Page"));
            return new ProductListPage();
        }

        public CheckoutPage ClickProceedToCheckoutButton()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(6));
            wait.Until(ExpectedConditions.ElementIsVisible(_cartIconLocator));
            var cartIconButton = _driver.FindElement(_cartIconLocator);
            ScrollToElement(cartIconButton);
            cartIconButton.Click();
            var proceedToCheckoutButton = wait.Until(ExpectedConditions.ElementIsVisible(_proceedToCheckoutButtonLocator));
            MoveToElement(proceedToCheckoutButton);
            proceedToCheckoutButton.Click();
            wait.Until((driver) => driver.Title.StartsWith("Checkout"));
            return new CheckoutPage();
        }

        public GearPage OpenGearCategoryPage()
        {
            var element = _driver.FindElement(_gearCategoryButtonLocator);
            element.Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));

            wait.Until((driver) => driver.Title.StartsWith("Gear"));

            return new GearPage();
        }


    }
}
