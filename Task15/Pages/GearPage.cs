using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task15.Pages
{
    public class GearPage: BasePage
    {
        private By _bagsCategoryButton = By.XPath("//span[@class='count']/preceding::a[contains(text(),'Bags')]");

        public ProductListPage ClickBagsCategoryButton()
        {
            var bagsCategoryButton = _driver.FindElement(_bagsCategoryButton);
            bagsCategoryButton.Click();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));
            wait.Until((driver) => !driver.Title.StartsWith("Gear"));
            return new ProductListPage();
        }
    }
}
