using OpenQA.Selenium;

namespace Task15.Pages
{
    public class GearPage: BasePage
    {
        private By _bagsCategoryButton = By.XPath("//span[@class='count']/preceding::a[contains(text(),'Bags')]");

        public ProductListPage ClickBagsCategoryButton()
        {
            var bagsCategoryButton = _driver.FindElement(_bagsCategoryButton);
            bagsCategoryButton.Click();
            _wait.Until((driver) => !driver.Title.StartsWith("Gear"));
            return new ProductListPage();
        }
    }
}
