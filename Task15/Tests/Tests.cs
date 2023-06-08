using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Task15.Driver;
using Task15.Models;
using Task15.Pages;
using Task15.Utils;

namespace Task15.Tests
{
    public class Tests
    {
        private IWebDriver _driver => DriverSingleton.Driver;
        private HomePage _homePage;


        [SetUp]
        public void SetUp()
        {
            string baseUrl = "https://magento.softwaretestingboard.com/";
            _driver.Navigate().GoToUrl(baseUrl);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
                     
            _homePage = new HomePage();
        }

        [TestCase("Watches", "Dash Digital Watch")]
        public void TC1_SignInValidUserAddedProductsToCartCreateOrderCheckOrderData_OrderDataIsCorrect(string submenuName, string productName)
        {
            var customerLoginPage = _homePage.ClickSignInButton();

            LoginTestData loginTestData = LoginTestDataReader.ReadLoginTestData();
            _homePage = customerLoginPage.Login(loginTestData);

            var productListPage = _homePage.ClickGearSubmenuItem(submenuName);

            productListPage.AddProductToCartByName(productName);

            var checkoutPage = productListPage.ClickProceedToCheckoutButton();

            ShippingAddress shippingAddress = new ShippingAddressGenerator().GenerateShippingAddress();
            checkoutPage.FillShippingAddress(shippingAddress);
            var subtotalExpected = checkoutPage.GetCartSubtotalAmount();
            var shippingExpected = checkoutPage.GetShippingAmount();
            var orderTotalExpected = checkoutPage.GetOrderTotalAmount();    
            var checkoutSuccess = checkoutPage.ClickPlaceOrderButton();
            var orderNumber = checkoutSuccess.GetOrderNumber();
            checkoutSuccess.ClickContinueShoppingButton();

            var myAccountPage = _homePage.ClickMyAccountButton();
            var myOrdersPage = myAccountPage.ClickMyOrdersButton();
            var orderPage = myOrdersPage.ClickViewOrderButton(orderNumber);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(productName, orderPage.GetProductsName());
                Assert.AreEqual(subtotalExpected, orderPage.GetSubtotalAmount());
                Assert.AreEqual(shippingExpected, orderPage.GetShippingAmount());
                Assert.AreEqual(orderTotalExpected, orderPage.GetGrandTotalAmount());
            });
        }

        [Test]
        public void TC2_OpenMainPageOpenRegistrationPageCreateAccountWithoutEmailCheckErrorMessage_ErrorMessageArrears()
        {
            var registrationPage = _homePage.ClickCreateAccountButton();
            CreateAccountData createAccountData = new CreateAccountDataGenerator().GenerateCreateAccountData();
            registrationPage.FillInPersonalInformatFieldWithoutEmail(createAccountData);
            registrationPage.ClickCreateAccountButton();
            Assert.IsTrue(registrationPage.ErrorRequiredEmailMessageIsExist());
        }

        [TestCase("3")]
        public void TC3_OpenMainPageAddToCartThreeBagsCheckCardIcon_CartIconHasRightNumber(string expectedCount)
        {
            var customerLoginPage = _homePage.ClickSignInButton();
            LoginTestData loginTestData = LoginTestDataReader.ReadLoginTestData();
            customerLoginPage.Login(loginTestData);
            var gearPage = _homePage.OpenGearCategoryPage();
            var productListPage = gearPage.ClickBagsCategoryButton();
            productListPage.AddProductToCartByNumber(0);
            productListPage.AddProductToCartByNumber(1);
            var productPage = productListPage.OpenProductByNumber(2);
            productPage.ClickAddToCartButton();
            Assert.AreEqual(expectedCount, productPage.GetCounterValue());
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

    }
}