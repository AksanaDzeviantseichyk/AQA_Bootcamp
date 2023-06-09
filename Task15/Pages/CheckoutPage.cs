using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
        private By _placeOrderLocator = By.CssSelector(".action.primary.checkout");
        private By _cartSubtotalLocator = By.CssSelector("[data-th='Cart Subtotal']");
        private By _shippingLocator = By.CssSelector("[data-th='Shipping']");
        private By _orderTotalLocator = By.CssSelector("[data-th='Order Total']");

        public void ClickNewAddressButton()
        {
            var button = _driver.FindElement(_newAddressButtonLocator);
            ScrollToElement(button);
            _wait.Until(ExpectedConditions.ElementToBeClickable(button));
           button.Click();
        }
        public void FillInShippingAdsressForm(ShippingAddress shippingAddress)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(_streetInputLocator));
            FillInStreet(shippingAddress.Street);
            FillInCity(shippingAddress.City);
            FillInCountry(shippingAddress.Country);
            FillInPostalCode(shippingAddress.PostalCode);
            FillInPhoneNumber(shippingAddress.PhoneNumber);
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
        }
        public void ClickShipHere()
        {
            var button = _wait.Until(drv => drv.FindElement(_shipHereButtonLocator));
            button.Click();
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
        }
        public void FillShippingAddress(ShippingAddress shippingAddress)
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));

            if (ElementExists(_newAddressButtonLocator))
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
            //
            _wait.Until(ExpectedConditions.ElementToBeClickable(_loaderLocator));
            var nextButton = _driver.FindElement(_nextButtonLocator);
            ScrollToElement(nextButton);
            nextButton.Click();
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));

        }
        
        public CheckoutSuccessPage ClickPlaceOrderButton()
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            var placeOrderButton = _driver.FindElement(_placeOrderLocator);
            placeOrderButton.Click();
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loaderLocator));
            return new CheckoutSuccessPage();
        }

        public void FillInCity(string city)
        {
            var inputField = _driver.FindElement(_cityInputLocator);
            inputField.SendKeys(city);
        }

        public void FillInStreet(string street)
        {
            var inputField = _driver.FindElement(_streetInputLocator);
            inputField.SendKeys(street);
        }
        public void FillInCountry(string country)
        {
            var inputField = _driver.FindElement(_countrySelectLocator);
            inputField.SendKeys(country);
        }

        public void FillInPhoneNumber(string phone)
        {
            var inputField = _driver.FindElement(_phoneNumberInputLocator);
            inputField.SendKeys(phone);
        }
        public void FillInPostalCode(string postalCode)
        {
            var inputField = _driver.FindElement(_postalCodeInputLocator);
            inputField.SendKeys(postalCode);
        }

        public string GetCartSubtotalAmount()
        {
            return _driver.FindElement(_cartSubtotalLocator).Text;
        }

        public string GetShippingAmount()
        {
            return _driver.FindElement(_shippingLocator).Text;
        }

        public string GetOrderTotalAmount()
        {
            return _driver.FindElement(_orderTotalLocator).Text;
        }
    }
}
