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
        private HomePage _homePage;

        [SetUp]
        public void SetUp()
        {
            ConfigData _configData = ConfigDataReader.ReadConfigData("Resourses\\ConfigData.json");
            DriverSingleton.InitializeDriver();
            _homePage = new HomePage();
        }

        [TestCase("Resourses\\TC1_LoginTestData.json", "Watches", "Dash Digital Watch")]
        public void TC1_SignInValidUserAddedProductsToCartCreateOrderCheckOrderData_OrderDataIsCorrect
            (string loginDataFilePath, string submenuName, string productName)
        {
            var customerLoginPage = _homePage.ClickSignInButton();

            LoginTestData loginTestData = LoginTestDataReader.ReadLoginTestData(loginDataFilePath);
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

        [TestCase("Resourses\\TC3_LoginTestData.json", "3")]
        public void TC3_OpenMainPageAddToCartThreeBagsCheckCardIcon_CartIconHasRightNumber(string loginDatafilePath, string expectedCount)
        {
            var customerLoginPage = _homePage.ClickSignInButton();
            LoginTestData loginTestData = LoginTestDataReader.ReadLoginTestData(loginDatafilePath);
            customerLoginPage.Login(loginTestData);
            var gearPage = _homePage.OpenGearCategoryPage();
            var productListPage = gearPage.ClickBagsCategoryButton();
            productListPage.AddProductToCartByNumber(0);
            productListPage.AddProductToCartByNumber(1);
            var productPage = productListPage.OpenProductByNumber(2);
            productPage.ClickAddToCartButton();
            var actualCount = productPage.GetCounterValue();
            var shoppingCart = _homePage.ClickViewEditCartButton();
            shoppingCart.DeleteAllProduct();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TearDown]
        public void TearDown()
        {
            DriverSingleton.QuitDriver();
        }

    }
}