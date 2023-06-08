using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task15.Pages
{
    public class OrderPage:BasePage
    {
        private By _productNameLocator = By.CssSelector("tbody .product-item-name");
        private By _subtotalValueLocator = By.CssSelector(".subtotal [data-th='Subtotal']");
        private By _shippingValueLocator = By.CssSelector("[data-th='Shipping & Handling']");
        private By _grandTotalValueLocator = By.CssSelector("[data-th='Grand Total']");

        public string GetProductsName()
        {
            return _driver.FindElement(_productNameLocator).Text; 
        }

        public string GetSubtotalAmount()
        {
           return _driver.FindElement(_subtotalValueLocator).Text;
        }

        public string GetShippingAmount()
        {
            return _driver.FindElement(_shippingValueLocator).Text;
        }
        public string GetGrandTotalAmount()
        {
           return _driver.FindElement(_grandTotalValueLocator).Text;
        }

    }
}
