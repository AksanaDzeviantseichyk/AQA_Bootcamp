using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using Task15.Driver;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Task15.Pages
{
    public class BasePage
    {
        protected IWebDriver _driver; 
        protected WebDriverWait _wait;

        private By _gearCategoryButtonLocator = By.Id("ui-id-6");
        protected By _welcomeMessageLocator = By.ClassName("logged-in");
        private By _gearSubmenuItemsLocator = By.CssSelector(".level0.nav-4 li");
        private By _proceedToCheckoutButtonLocator = By.Id("top-cart-btn-checkout");
        private By _cartIconLocator = By.ClassName("minicart-wrapper");
        private By _signInButtonLocator = By.ClassName("authorization-link");
        private By _createAccountButtonLocator = By.XPath("//a[contains(text(),'Create an Account')]");
        private By _switchButtonLocator = By.ClassName("switch");
        private By _myAccountButtonLocator = By.XPath("//a[contains(text(), 'My Account')]");
        protected By _loaderLocator = By.ClassName("loader");
        private By _viewAndEditCartButtonLocator = By.ClassName("viewcart");

        public BasePage()
        {
            _driver = DriverSingleton.Driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
        }
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
        protected bool ElementExists(By locator)
        {
            try
            {
                _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
                
                _driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public CustomerLoginPage ClickSignInButton()
        {
            var signInButton = _driver.FindElement(_signInButtonLocator);
            signInButton.Click();

            return new CustomerLoginPage();
        }
        public RegistrationPage ClickCreateAccountButton()
        {
            var createAccountButton = _driver.FindElement(_createAccountButtonLocator);
            createAccountButton.Click();

            return new RegistrationPage();
        }
        public void ClickSwitchAccountButton()
        {
            var switchButton = _driver.FindElement(_switchButtonLocator);
            switchButton.Click();
        }
        public MyAccountPage ClickMyAccountButton()
        {
            ClickSwitchAccountButton();
            var myAccountButton = _driver.FindElement(_myAccountButtonLocator);
            myAccountButton.Click();

            return new MyAccountPage();
        }
        public ProductListPage ClickGearSubmenuItem(string subMenuName)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_gearCategoryButtonLocator));
            var elementToHover = _driver.FindElement(_gearCategoryButtonLocator);
            MoveToElement(elementToHover);
            var gearSubmenuItems = _driver.FindElements(_gearSubmenuItemsLocator);
            IWebElement targetGearSubmenu = gearSubmenuItems.FirstOrDefault(i => i.Text.Equals(subMenuName));
            _wait.Until(ExpectedConditions.ElementToBeClickable(targetGearSubmenu));
            targetGearSubmenu.Click();

            _wait.Until((driver) => !driver.Title.StartsWith("Home Page"));
            return new ProductListPage();
        }
        public void ClickCartIcon()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_cartIconLocator));
            var cartIconButton = _driver.FindElement(_cartIconLocator);
            ScrollToElement(cartIconButton);
            cartIconButton.Click();
           // _wait.Until(ExpectedConditions.ElementIsVisible(_cartIconLocator));
        }
        public CheckoutPage ClickProceedToCheckoutButton()
        {
            ClickCartIcon();
            //
            _wait.Until(ExpectedConditions.ElementToBeClickable(_proceedToCheckoutButtonLocator));
            var proceedToCheckoutButton = _driver.FindElement(_proceedToCheckoutButtonLocator);
            
            _wait.Until(ExpectedConditions.ElementIsVisible(_proceedToCheckoutButtonLocator));
            proceedToCheckoutButton.Click();
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            _wait.Until((driver) => driver.Title.StartsWith("Checkout"));
            return new CheckoutPage();
        }

        public GearPage OpenGearCategoryPage()
        {
            var element = _driver.FindElement(_gearCategoryButtonLocator);
            element.Click();
                   
            _wait.Until((driver) => driver.Title.StartsWith("Gear"));

            return new GearPage();
        }
        public ShoppingCart ClickViewEditCartButton()
        {
            ClickCartIcon();
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            if (ElementExists(_viewAndEditCartButtonLocator))
            {
                var button = _driver.FindElement(_viewAndEditCartButtonLocator);
                button.Click();
            }
            return new ShoppingCart();
        }

    }
}
