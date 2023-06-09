using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Task15.Models;

namespace Task15.Pages
{
    public class CustomerLoginPage: BasePage
    {
        private By _emailInputLocator = By.Name("login[username]");
        private By _passwordInputLocator = By.Name("login[password]");
        private By _signInFormButtonLocator = By.Id("send2");

        public HomePage Login(LoginTestData loginTestData)
        {
            EnterEmail(loginTestData.Email);
            EnterPassword(loginTestData.Password);
            ClickSignInButton();
            return new HomePage();
        }
        public void EnterEmail(string value)
        {
            var element = _driver.FindElement(_emailInputLocator);
            element.SendKeys(value);
        }
        public void EnterPassword(string value)
        {
            var element = _driver.FindElement(_passwordInputLocator);
            element.SendKeys(value);
        }
        public void ClickSignInButton()
        {            
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(_signInFormButtonLocator));
            element.Click();
            _wait.Until((driver) => driver.FindElement(_welcomeMessageLocator).Text.StartsWith("Welcome, "));
        }
    }
}
