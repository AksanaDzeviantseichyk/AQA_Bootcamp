using OpenQA.Selenium;


namespace Task15.Pages
{
    public class HomePage: BasePage
    {
        private By _signInButtonLocator = By.ClassName("authorization-link");
        private By _createAccountButtonLocator = By.XPath("//a[contains(text(),'Create an Account')]");
        private By _switchButtonLocator = By.ClassName("switch");
        private By _myAccountButtonLocator = By.XPath("//a[contains(text(), 'My Account')]");
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
        
        public MyAccountPage ClickMyAccountButton()
        {
            var switchButton = _driver.FindElement(_switchButtonLocator);
            switchButton.Click();
            var myAccountButton = _driver.FindElement(_myAccountButtonLocator);
            myAccountButton.Click();

            return new MyAccountPage();
        }
    }
}
