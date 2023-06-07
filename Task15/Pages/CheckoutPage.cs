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
    public class CheckoutPage:BasePage
    {
        private By _streetInputLocator = By.CssSelector("input[name = 'street[0]']");
        private By _cityInputLocator = By.CssSelector("input[name='city']");
        private By _countrySelectLocator = By.CssSelector("[name = 'country_id']");
        private By _postalCodeInputLocator = By.CssSelector("input[name = 'postcode']");
        private By _phoneNumberInputLocator = By.CssSelector("input[name = 'telephone']");
        private By _nextButtonLocator = By.ClassName("button");
        private By _newAddressButtonLocator = By.ClassName("action-show-popup");
        private By _shipHereButtonLocator = By.ClassName("action-save-address");
        private By _loaderLocator = By.ClassName(".loader");
        private By _placeOrderLocator = By.CssSelector(".action.primary.checkout");

        public void ClickNewAddressButton()
        {
            var button = _driver.FindElement(_newAddressButtonLocator);
            button.Click();
        }
        public void FillInShippingAdsressForm(ShippingAddress shippingAddress)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            wait.Until(ExpectedConditions.ElementIsVisible(_streetInputLocator));
            var inputField = _driver.FindElement(_streetInputLocator);
            inputField.SendKeys(shippingAddress.Street);
            inputField = _driver.FindElement(_cityInputLocator);
            inputField.SendKeys(shippingAddress.City);
            inputField = _driver.FindElement(_countrySelectLocator);
            inputField.SendKeys(shippingAddress.Country);
            inputField = _driver.FindElement(_postalCodeInputLocator);
            inputField.SendKeys(shippingAddress.PostalCode);
            inputField = _driver.FindElement(_phoneNumberInputLocator);
            inputField.SendKeys(shippingAddress.PhoneNumber);
        }
        public void ClickShipHere()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(6));
            var button = wait.Until(drv => drv.FindElement(_shipHereButtonLocator));
            button.Click();
        }
        public void FillShippingAddress(ShippingAddress shippingAddress)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));//(By.CssSelector("loading-element-selector")));

            if (ElementExists())
            {
                ClickNewAddressButton();
                FillInShippingAdsressForm(shippingAddress);
                ClickShipHere();
                ClickNextButton();

            }
            else
            {
                FillInShippingAdsressForm(shippingAddress);
                ClickNextButton();
            }
            
        }

        public void ClickNextButton()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(6));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));// By.CssSelector("loading-element-selector")));
            var nextButton = _driver.FindElement(_nextButtonLocator);
            nextButton.Click();
            
        }
        private bool ElementExists()
        {
            try
            {
                _driver.FindElement(_newAddressButtonLocator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public CheckoutSuccessPage ClickPlaceOrderButton()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(6));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            var placeOrderButton = _driver.FindElement(_placeOrderLocator);
            placeOrderButton.Click();
            return new CheckoutSuccessPage();
        }

    }
}
