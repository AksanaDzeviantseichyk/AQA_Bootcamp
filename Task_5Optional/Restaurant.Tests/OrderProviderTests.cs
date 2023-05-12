using FluentAssertions;
using NUnit.Framework;
using RestaurantErp.Core.Contracts;
using RestaurantErp.Core.Helpers;
using RestaurantErp.Core.Models;
using RestaurantErp.Core.Models.Bill;
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
using System.Reflection.Emit;

namespace Restaurant.Tests
{
    public class OrderProviderTests
    {
        private ProductProvider productProvider;
        private IDiscountByTimeProvider discountManager;
        private IDiscountCalculator calculator;
        private IOrderProvider orderProvider;

        private static IEnumerable<TestCaseData> AddItemsGenerator()
        {
            yield return new TestCaseData(new AddProductRequest{Name = "Starter",Price = 3},3);
            yield return new TestCaseData(new AddProductRequest{Name = "Starter",Price = 4},4);
        }
        private static IEnumerable<TestCaseData> CancelItemsGenerator()
        {
            yield return new TestCaseData(new AddProductRequest{Name = "Starter",Price = 3}, 4, 1);
            yield return new TestCaseData(new AddProductRequest{Name = "Starter",Price = 4}, 5, 5);
        }
        private static IEnumerable<TestCaseData> IncorrectCancelItemCountGenerator()
        {
            yield return new TestCaseData(new AddProductRequest{Name = "Starter", Price = 3},3, 5);
        }

        [SetUp]
        public void SetUp()
        {
            productProvider = new ProductProvider();
            discountManager = new DiscountByTimeProvider(new DiscountByTimeProviderSettings
            {
                EndDiscountDelay = TimeSpan.FromSeconds(50)
            });
            calculator = new DiscountCalculator(new DiscountCalculatorSettings
            {
                MinimalProductPrice = 0.2m
            });

            orderProvider = new OrderProvider(
                (IPriceStorage)productProvider,
                new[] { (IDiscountProvider)discountManager },
                calculator,
                new TimeHelper(),
                new BillHelper(productProvider),
                new ServiceChargeProvider(new ServiceChargeProviderSettings { ServiceRate = 0.1m }));

        }

        [Test]
        public void Checkout_AllBillExternal_()
        {
            // Precondition


            var requestProduct1 = new AddProductRequest
            {
                Name = "Starter",
                Price = 4
            };

            var requestProduct2 = new AddProductRequest
            {
                Name = "Main",
                Price = 7
            };

            var requestProduct3 = new AddProductRequest
            {
                Name = "Drink",
                Price = 2.5m
            };

            var productId1 = productProvider.AddProduct(requestProduct1);
            var productId2 = productProvider.AddProduct(requestProduct2);
            var productId3 = productProvider.AddProduct(requestProduct3);

            // Action

            var orderId = orderProvider.CreateOrder();

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId1
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId2
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId3
            });

            var actual = orderProvider.Checkout(orderId);
            Console.WriteLine($"{actual.Amount}, {actual.Discount}, {actual.AmountDiscounted}, {actual.Service}, {actual.Total}, {actual.Items.ToString()}");

        }

        [TestCase(1)]
        [TestCase(4)]
        public void CreateOrder_SomeOrderInOrderStarage_CountOrderIsCorrect(int count)
        {
            // Precondition
            var orderProvider = new OrderProvider(
                (IPriceStorage)productProvider,
                new[] { (IDiscountProvider)discountManager },
                calculator,
                new TimeHelper(),
                new BillHelper(productProvider),
                new ServiceChargeProvider(new ServiceChargeProviderSettings { ServiceRate = 0.1m }));

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

        [Test]
        [TestCaseSource(nameof(AddItemsGenerator))]
        public void AddItem_SomeItemInOrder_ItemCountInOrderIsCorrect(AddProductRequest requestProduct, int count)
        {
            //Precondition
            var productId1 = productProvider.AddProduct(requestProduct);
            var orderId = orderProvider.CreateOrder();
            var orderItemRequest = new OrderItemRequest
            {
                OrderId = orderId,
                Count = count,
                ProductId = productId1
            };

            //Action
            orderProvider.AddItem(orderItemRequest);
            var _orderStorageField = typeof(OrderProvider)
                .GetField("_orderStorage", BindingFlags.Instance | BindingFlags.NonPublic);
            var orderStorage = (ConcurrentBag<Order>)_orderStorageField.GetValue(orderProvider);
            var order = orderStorage.Single(o => o.Id == orderId);
            var actual = order.Items.Count;
            var expected = count;

            //Assert
            Assert.AreEqual(expected, actual);

        }
        
        [Test]
        [TestCaseSource(nameof(CancelItemsGenerator))]
        public void CancelItem_FromOrder_ItemCountInOrderIsCorrect(AddProductRequest requestProduct, int addCount, int cancelCount)
        {
            //Precondition         
            var productId1 = productProvider.AddProduct(requestProduct);
            var orderId = orderProvider.CreateOrder();
            var orderItemRequest = new OrderItemRequest
            {
                OrderId = orderId,
                Count = addCount,
                ProductId = productId1
            };
            orderProvider.AddItem(orderItemRequest);

            //Action
            orderProvider.CancelItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = cancelCount,
                ProductId = productId1
            });
            var _orderStorageField = typeof(OrderProvider)
                .GetField("_orderStorage", BindingFlags.Instance | BindingFlags.NonPublic);
            var orderStorage = (ConcurrentBag<Order>)_orderStorageField.GetValue(orderProvider);
            var order = orderStorage.Single(o => o.Id == orderId);
            var actual = order.Items.Where(i => i.IsCancelled == false).Count();
            var expected = addCount - cancelCount;
            
            //Assert
            Assert.AreEqual(expected, actual);

        }
        [Test]
        [TestCaseSource(nameof(IncorrectCancelItemCountGenerator))]
        public void CancelItem_IncorrectCancelItemCount_ShouldThrowArgumentOutOfRangeException(AddProductRequest requestProduct, int addCount, int cancelCount)
        {
            //Precondition
            var productId1 = productProvider.AddProduct(requestProduct);
            var orderId = orderProvider.CreateOrder();
            var orderItemRequest = new OrderItemRequest
            {
                OrderId = orderId,
                Count = addCount,
                ProductId = productId1
            };
            orderProvider.AddItem(orderItemRequest);
            var orderItemRequest1 = new OrderItemRequest
            {
                OrderId = orderId,
                Count = cancelCount,
                ProductId = productId1
            };
            //Action-Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => orderProvider.CancelItem(orderItemRequest1));

        }

        [Test]
        public void Ckeckout_TwoProductsInOrder_AmountIsCorrect()
        {
            // Precondition
            var requestProduct1 = new AddProductRequest
            {
                Name = "Starter",
                Price = 4
            };
            var requestProduct2 = new AddProductRequest
            {
                Name = "Main",
                Price = 7
            };
            var productId1 = productProvider.AddProduct(requestProduct1);
            var productId2 = productProvider.AddProduct(requestProduct2);

            // Action

            var orderId = orderProvider.CreateOrder();

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId1
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId2
            });

            var actual = orderProvider.Checkout(orderId);

            // Assert

            var expected = new BillExternal
            {
                Amount = 11,
                AmountDiscounted = 11,
                Discount = 0,
                OrderId = orderId,
                Service = 1.1m,
                Total = 12.1m,
                Items = new[]
                {
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        ProductName = requestProduct2.Name
                    }
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Ckeckout_ThreeProductsBy4Items_BillIsCorrect()
        {
            // Precondition


            var requestProduct1 = new AddProductRequest
            {
                Name = "Starter",
                Price = 4
            };

            var requestProduct2 = new AddProductRequest
            {
                Name = "Main",
                Price = 7
            };

            var requestProduct3 = new AddProductRequest
            {
                Name = "Drink",
                Price = 2.5m
            };

            var productId1 = productProvider.AddProduct(requestProduct1);
            var productId2 = productProvider.AddProduct(requestProduct2);
            var productId3 = productProvider.AddProduct(requestProduct3);

            // Action

            var orderId = orderProvider.CreateOrder();

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 4,
                ProductId = productId1
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 4,
                ProductId = productId2
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 4,
                ProductId = productId3
            });

            var actual = orderProvider.Checkout(orderId);

            //Assert

            var expected = new BillExternal
            {
                Amount = 54,
                AmountDiscounted = 54,
                Discount = 0,
                OrderId = orderId,
                Service = 5.4m,
                Total = 59.4m,
                Items = new[]
                {
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    }
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CancellAllProductsBy1QtyAndCheckout_ThreeProductsBy4Items_BillIsCorrect()
        {
            // Precondition

            var requestProduct1 = new AddProductRequest
            {
                Name = "Starter",
                Price = 4
            };

            var requestProduct2 = new AddProductRequest
            {
                Name = "Main",
                Price = 7
            };

            var requestProduct3 = new AddProductRequest
            {
                Name = "Drink",
                Price = 2.5m,
            };

            var productId1 = productProvider.AddProduct(requestProduct1);
            var productId2 = productProvider.AddProduct(requestProduct2);
            var productId3 = productProvider.AddProduct(requestProduct3);

            // Action

            var orderId = orderProvider.CreateOrder();

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 4,
                ProductId = productId1
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 4,
                ProductId = productId2,
                
            });

            orderProvider.AddItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 4,
                ProductId = productId3
            });

            orderProvider.CancelItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId1
            });

            orderProvider.CancelItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId2
            });

            orderProvider.CancelItem(new OrderItemRequest
            {
                OrderId = orderId,
                Count = 1,
                ProductId = productId3
            });

            var actual = orderProvider.Checkout(orderId);

            //Assert

            var expected = new BillExternal
            {
                Amount = 40.5m,
                AmountDiscounted = 40.5m,
                Discount = 0,
                OrderId = orderId,
                Service = 4.05m,
                Total = 44.55m,
                Items = new[]
                {
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = requestProduct1.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = requestProduct2.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = requestProduct3.Name
                    }
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }

        
    }

}