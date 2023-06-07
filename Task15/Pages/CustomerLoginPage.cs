using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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
            var element = _driver.FindElement(_signInFormButtonLocator);

            element.Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));

            wait.Until((driver) => !driver.Title.StartsWith("Customer Login "));
            
        }

    }
}
