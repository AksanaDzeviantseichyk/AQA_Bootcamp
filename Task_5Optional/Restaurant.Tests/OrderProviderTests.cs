using FluentAssertions;
using NUnit.Framework;
using Restaurant.Tests.Utils;
using RestaurantErp.Core.Contracts;
using RestaurantErp.Core.Models.Discount;
using RestaurantErp.Core.Models.Order;
using RestaurantErp.Core.Models.Product;
using RestaurantErp.Core.Providers;
using RestaurantErp.Core.Providers.Discount;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Restaurant.Tests
{
    public class OrderProviderTests
    {
        private  ProductProvider _productProvider = new ProductProvider();
        private  IDiscountByTimeProvider _discountManager;
        private  IDiscountCalculator _calculator;
        private readonly AddProductRequestGenerator _addProductRequest = new AddProductRequestGenerator();
        private readonly OrderItemRequestGenerator _orderItemRequest = new OrderItemRequestGenerator();
        private readonly OrderProviderGenerator _orderProvider = new OrderProviderGenerator();
        private readonly DiscountByTimeSettingsGenerator _discountByTimeSettings = new DiscountByTimeSettingsGenerator();

        private static IEnumerable<TestCaseData> AddItemsGenerator()
        {
            AddProductRequestGenerator _addProductRequest = new AddProductRequestGenerator();
            yield return new TestCaseData(_addProductRequest.GenerateAddProductRequest("Starter", 3), 3);
            yield return new TestCaseData(_addProductRequest.GenerateAddProductRequest("Starter", 4), 4);
        }
        private static IEnumerable<TestCaseData> CancelItemsGenerator()
        {
            AddProductRequestGenerator _addProductRequest = new AddProductRequestGenerator();
            yield return new TestCaseData(_addProductRequest.GenerateAddProductRequest("Starter", 3), 4, 1);
            yield return new TestCaseData(_addProductRequest.GenerateAddProductRequest("Starter", 4), 5, 5);
        }
        private static IEnumerable<TestCaseData> IncorrectCancelItemCountGenerator()
        {
            AddProductRequestGenerator _addProductRequest = new AddProductRequestGenerator();
            yield return new TestCaseData(_addProductRequest.GenerateAddProductRequest("Starter", 3), 3, 5);
        }

        [SetUp]
        public void SetUp()
        {
            _discountManager = new DiscountByTimeProvider(new DiscountByTimeProviderSettings
            {
                EndDiscountDelay = TimeSpan.Zero
            }) ;
            _calculator = new DiscountCalculator(new DiscountCalculatorSettings
            {
                MinimalProductPrice = 0
            });

        }
    
        [TestCase(1)]
        [TestCase(4)]
        public void CreateOrder_SomeOrderInOrderStorage_CountOrderIsCorrect(int count)
        {
            // Precondition
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);

            // Action
            for (int _ = 1; _ <= count; _++)
            {
                orderProvider.CreateOrder();
            }
            var _orderStorageField = typeof(OrderProvider)
                .GetField("_orderStorage", BindingFlags.Instance | BindingFlags.NonPublic);
            var orderStorage = (ConcurrentBag<Order>)_orderStorageField.GetValue(orderProvider);
            var actual = orderStorage.Count;
            var expected = count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(AddItemsGenerator))]
        public void AddItem_SomeItemInOrder_ItemCountInOrderIsCorrect(AddProductRequest requestProduct, int count)
        {
            //Precondition
            var productId1 = _productProvider.AddProduct(requestProduct);
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();

            //Action
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, count, productId1));
            var _orderStorageField = typeof(OrderProvider)
                .GetField("_orderStorage", BindingFlags.Instance | BindingFlags.NonPublic);
            var orderStorage = (ConcurrentBag<Order>)_orderStorageField.GetValue(orderProvider);
            var order = orderStorage.Single(o => o.Id == orderId);
            var actual = order.Items.Count;

            //Assert
            var expected = count;
            Assert.AreEqual(expected, actual);
        }
        
        [TestCaseSource(nameof(CancelItemsGenerator))]
        public void CancelItem_OneProductBySomeItemInOrderAndCancelSomeItem_ItemCountInOrderIsCorrect(AddProductRequest requestProduct, int addCount, int cancelCount)
        {
            //Precondition         
            var productId1 = _productProvider.AddProduct(requestProduct);
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();
            
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, addCount, productId1));

            //Action
            orderProvider.CancelItem(_orderItemRequest.GenerateOrderItemRequest(orderId, cancelCount, productId1));
            var _orderStorageField = typeof(OrderProvider)
                .GetField("_orderStorage", BindingFlags.Instance | BindingFlags.NonPublic);
            var orderStorage = (ConcurrentBag<Order>)_orderStorageField.GetValue(orderProvider);
            var order = orderStorage.Single(o => o.Id == orderId);
            var actual = order.Items.Where(i => i.IsCancelled == false).Count();
            var expected = addCount - cancelCount;
            
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(IncorrectCancelItemCountGenerator))]
        public void CancelItem_IncorrectCancelItemCount_ShouldThrowArgumentOutOfRangeException(AddProductRequest requestProduct, int addCount, int cancelCount)
        {
            //Precondition
            var productId1 = _productProvider.AddProduct(requestProduct);
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, addCount, productId1));
            
            //Action-Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => orderProvider.CancelItem(
                _orderItemRequest.GenerateOrderItemRequest(orderId, cancelCount, productId1)));
        }

        [Test]
        public void Checkout_SomeProductBy1ItemInOrder_AllBillExternalFieldsIsCorrect()
        {
            // Precondition
            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Starter", 4);
            var requestProduct2 = _addProductRequest.GenerateAddProductRequest("Main", 7);
            var requestProduct3 = _addProductRequest.GenerateAddProductRequest("Drink", 2.5m);

            var productId1 = _productProvider.AddProduct(requestProduct1);
            var productId2 = _productProvider.AddProduct(requestProduct2);
            var productId3 = _productProvider.AddProduct(requestProduct3);

            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);

            // Action
            var orderId = orderProvider.CreateOrder();
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId2));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId3));
            var actual = orderProvider.Checkout(orderId);

            //Assert
            var expectedAmount = requestProduct1.Price + requestProduct2.Price + requestProduct3.Price;
            var expectedService = 0.1m * (requestProduct1.Price + requestProduct2.Price + requestProduct3.Price);
            var expectedTotal = expectedAmount + expectedService;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(orderId, actual.OrderId);
                Assert.AreEqual(expectedAmount, actual.Amount);
                Assert.AreEqual(0, actual.Discount);
                Assert.AreEqual(expectedAmount, actual.AmountDiscounted);
                Assert.AreEqual(expectedService, actual.Service);
                Assert.AreEqual(expectedTotal, actual.Total);

                var item1 = actual.Items.Single(i => i.ProductName == requestProduct1.Name);
                Assert.AreEqual(requestProduct1.Name, item1.ProductName);
                Assert.AreEqual(requestProduct1.Price, item1.Amount);
                Assert.AreEqual(0, item1.PersonId);
                Assert.AreEqual(0, item1.Discount);
                Assert.AreEqual(requestProduct1.Price, item1.AmountDiscounted);

                var item2 = actual.Items.Single(i => i.ProductName == requestProduct2.Name);
                Assert.AreEqual(requestProduct2.Name, item2.ProductName);
                Assert.AreEqual(requestProduct2.Price, item2.Amount);
                Assert.AreEqual(0, item2.PersonId);
                Assert.AreEqual(0, item2.Discount);
                Assert.AreEqual(requestProduct2.Price, item2.AmountDiscounted);

                var item3 = actual.Items.Single(i => i.ProductName == requestProduct3.Name);
                Assert.AreEqual(requestProduct3.Name, item3.ProductName);
                Assert.AreEqual(requestProduct3.Price, item3.Amount);
                Assert.AreEqual(0, item3.PersonId);
                Assert.AreEqual(0, item3.Discount);
                Assert.AreEqual(requestProduct3.Price, item3.AmountDiscounted);
            });
        }

        [Test]
        public async Task Checkout_LogicOfDiscountingByTime_BillIsCorrect()
        {
            //Precondition
            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Drink", 4);
            var requestProduct2 = _addProductRequest.GenerateAddProductRequest("Main", 5);
            var productId1 = _productProvider.AddProduct(requestProduct1);
            var productId2 = _productProvider.AddProduct(requestProduct2);
            _discountManager.Add(_discountByTimeSettings.GenerateDiscountByTimeSettings(productId1, 1, 0.1m));
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);

            //Action
            var orderId = orderProvider.CreateOrder();
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId2));
            await Task.Delay(TimeSpan.FromSeconds(70));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId2));
            var actual = orderProvider.Checkout(orderId);
            ExpectedBillExternal expectedBillExternal = new ExpectedBillExternal();

            //Assert
            var expected = expectedBillExternal.CheckDiscount(
                orderId, requestProduct1.Name, requestProduct2.Name);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(0, 0)]
        [TestCase(0.1, 0.4)]
        [TestCase(0.2, 0.8)]
        [TestCase(0.3, 1.2)]
        [TestCase(0.4, 1.6)]
        [TestCase(0.5, 2)]
        [TestCase(0.6, 2.4)]
        [TestCase(0.7, 2.8)]
        [TestCase(0.8, 3.2)]
        [TestCase(0.9, 3.6)]
        [TestCase(1, 4)]
        public void Checkout_LogicOfServiceCharge_ServiceChargeInBillIsCorrect(decimal serviceRate, decimal expectedServiceCharge)
        {
            //Preconditon
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, serviceRate);
            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Drink", 4);
            var productId1 = _productProvider.AddProduct(requestProduct1);

            //Action
            var orderId = orderProvider.CreateOrder();
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            var actualServiceCharge = orderProvider.Checkout(orderId).Service;

            //Assert
            Assert.AreEqual(expectedServiceCharge, actualServiceCharge);
        }

        [Test]
        public async Task Checkout_FeatureIntegrations_BillIsCorrect()
        {
            //Precondition
            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Drink", 4);
            var productId1 = _productProvider.AddProduct(requestProduct1);
            _discountManager.Add(_discountByTimeSettings.GenerateDiscountByTimeSettings(productId1, 1, 0.1m));
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();

            //Action
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 2, productId1));
            await Task.Delay(TimeSpan.FromSeconds(70));
            orderProvider.CancelItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 2, productId1));

            var actual = orderProvider.Checkout(orderId);
            ExpectedBillExternal expectedBillExternal = new ExpectedBillExternal();

            //Assert
            var expected = expectedBillExternal.IntegrationDisountServiceRate(orderId, requestProduct1.Name);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Ckeckout_TwoProductsInOrder_AmountIsCorrect()
        {
            // Precondition
            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Starter", 4);
            var requestProduct2 = _addProductRequest.GenerateAddProductRequest("Main", 7);
            var productId1 = _productProvider.AddProduct(requestProduct1);
            var productId2 = _productProvider.AddProduct(requestProduct2);

            // Action
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId2));
            var actual = orderProvider.Checkout(orderId);
            ExpectedBillExternal expectedBillExternal = new ExpectedBillExternal();

            //Assert
            var expected = expectedBillExternal.TwoProductsInOrder(
                orderId, requestProduct1.Name, requestProduct2.Name);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Ckeckout_ThreeProductsBy4Items_BillIsCorrect()
        {
            // Precondition
            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Starter", 4);
            var requestProduct2 = _addProductRequest.GenerateAddProductRequest("Main", 7);
            var requestProduct3 = _addProductRequest.GenerateAddProductRequest("Drink", 2.5m);

            var productId1 = _productProvider.AddProduct(requestProduct1);
            var productId2 = _productProvider.AddProduct(requestProduct2);
            var productId3 = _productProvider.AddProduct(requestProduct3);

            // Action
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();

            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 4, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 4, productId2));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 4, productId3));

            var actual = orderProvider.Checkout(orderId);
            ExpectedBillExternal expectedBillExternal = new ExpectedBillExternal();

            //Assert
            var expected = expectedBillExternal.ThreeProductsBy4Items(
                orderId, requestProduct1.Name, requestProduct2.Name, requestProduct3.Name);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Checkout_ThreeProductsBy4ItemsAndCancellAllProductsBy1Qty_BillIsCorrect()
        {
            // Precondition

            var requestProduct1 = _addProductRequest.GenerateAddProductRequest("Starter", 4);
            var requestProduct2 = _addProductRequest.GenerateAddProductRequest("Main", 7);
            var requestProduct3 = _addProductRequest.GenerateAddProductRequest("Drink", 2.5m);

            var productId1 = _productProvider.AddProduct(requestProduct1);
            var productId2 = _productProvider.AddProduct(requestProduct2);
            var productId3 = _productProvider.AddProduct(requestProduct3);

            // Action
            var orderProvider = _orderProvider.GenerateOrderProvider(_productProvider, _discountManager, _calculator, 0.1m);
            var orderId = orderProvider.CreateOrder();

            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 4, productId1));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 4, productId2));
            orderProvider.AddItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 4, productId3));

            orderProvider.CancelItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId1));
            orderProvider.CancelItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId2));
            orderProvider.CancelItem(_orderItemRequest.GenerateOrderItemRequest(orderId, 1, productId3));

            var actual = orderProvider.Checkout(orderId);
            ExpectedBillExternal expectedBillExternal = new ExpectedBillExternal();

            //Assert
            var expected = expectedBillExternal.CancellAllProductsBy1QtyAndCheckout(
                orderId, requestProduct1.Name, requestProduct2.Name, requestProduct3.Name);
            actual.Should().BeEquivalentTo(expected);
        }      
    }
}