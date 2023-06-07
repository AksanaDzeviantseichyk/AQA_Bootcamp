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

        [Test]
        public void TC1_SignInValidUserAddedProductsToCartCreateOrderCheckOrderData_OrderDataIsCorrect()
        {
            var customerLoginPage = _homePage.ClickSignInButton();

            LoginTestData loginTestData = LoginTestDataReader.ReadLoginTestData();
            _homePage = customerLoginPage.Login(loginTestData);

            var productListPage = _homePage.ClickGearSubmenuItem("Watches");

            productListPage.AddProductToCart("Dash Digital Watch");

            var checkoutPage = productListPage.ClickProceedToCheckoutButton();

            ShippingAddress shippingAddress = new ShippingAddressGenerator().GenerateShippingAddress();
            checkoutPage.FillShippingAddress(shippingAddress);
            var checkoutSuccess = checkoutPage.ClickPlaceOrderButton();
            var orderNumber = checkoutSuccess.GetOrderNumber();
            checkoutSuccess.ClickContinueShoppingButton();

            var myAccountPage = _homePage.ClickMyAccountButton();
            var myOrdersPage = myAccountPage.ClickMyOrdersButton();
            var orderPage = myOrdersPage.ClickViewOrderButton(orderNumber);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Dash Digital Watch", orderPage.GetProductsName());
                Assert.AreEqual("$92.00", orderPage.GetSubtotalAmount());
                Assert.AreEqual("$5.00", orderPage.GetShippingAmount());
                Assert.AreEqual("$97.00", orderPage.GetGrandTotalAmount());
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

        [Test]
        public void TC3_OpenMainPageAddToCartThreeBagsCheckCardIcon_CartIconHasRightNumber()
        {
            var customerLoginPage = _homePage.ClickSignInButton();
            LoginTestData loginTestData = LoginTestDataReader.ReadLoginTestData();
            customerLoginPage.Login(loginTestData);
            var gearPage = _homePage.OpenGearCategoryPage();

        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

    }
}