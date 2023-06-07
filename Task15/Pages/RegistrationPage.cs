using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
