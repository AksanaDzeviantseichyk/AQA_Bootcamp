using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task15.Models;

namespace Task15.Pages
{
    public class RegistrationPage:BasePage
    {
        private By _firstNameInputLocator = By.Id("firstname");
        private By _lastNameInputLocator = By.Id("lastname");
        private By _passwordInputLocator = By.Id("password");
        private By _confirmationPasswordInputLocator = By.Id("password-confirmation");
        private By _createAccountButtonLocator = By.CssSelector(".action.submit");
        private By _errorMessageLocator = By.Id("email_address-error");

        public void FillInPersonalInformatFieldWithoutEmail(CreateAccountData accountData)
        {
            FillInFirstName(accountData.FirstName);
            FillInLastName(accountData.LastName);
            FillInPassword(accountData.Password);
            FillInConfirmationPassword(accountData.Password);
        }

        public void FillInFirstName(string firstName)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            wait.Until(ExpectedConditions.ElementIsVisible(_firstNameInputLocator));
            var inputField = _driver.FindElement(_firstNameInputLocator);
            inputField.SendKeys(firstName);
        }

        public void FillInLastName(string lastName)
        {
            var inputField = _driver.FindElement(_lastNameInputLocator);
            inputField.SendKeys(lastName);
        }

        public void FillInPassword(string password)
        {
            var inputField = _driver.FindElement(_passwordInputLocator);
            inputField.SendKeys(password);
        }
        public void FillInConfirmationPassword(string confirmationPassword)
        {
            var inputField = _driver.FindElement(_confirmationPasswordInputLocator);
            inputField.SendKeys(confirmationPassword);
        }

        public void ClickCreateAccountButton()
        {
            var createButton = _driver.FindElement(_createAccountButtonLocator);
            createButton.Click();
        }

        public bool ErrorRequiredEmailMessageIsExist()
        {
            try
            {
                _driver.FindElement(_errorMessageLocator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
