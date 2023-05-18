using RestaurantErp.Core.Models.Bill;
using RestaurantErp.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Utils
{
    public class ExpectedBillExternal
    {
        public BillExternal IntegrationDisountServiceRate(Guid orderId, params string[] productName)
        {
            return new BillExternal
            {
                Amount = 16,
                AmountDiscounted = 15.2m,
                Discount = 0.8m,
                OrderId = orderId,
                Service = 1.52m,
                Total = 16.72m,
                Items = new[]
                {
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0.4m,
                        AmountDiscounted = 3.6m,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0.4m,
                        AmountDiscounted = 3.6m,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    }
                }
            };
        }
        public BillExternal CheckDiscount(Guid orderId, params string[] productName)
        {
            return new BillExternal
            {
                Amount = 18,
                AmountDiscounted = 17.6m,
                Discount = 0.4m,
                OrderId = orderId,
                Service = 1.76m,
                Total = 19.36m,
                Items = new[]
                {
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0.4m,
                        AmountDiscounted = 3.6m,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 5,
                        Discount = 0,
                        AmountDiscounted = 5,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 5,
                        Discount = 0,
                        AmountDiscounted = 5,
                        PersonId = 0,
                        ProductName = productName[1]
                    }
                }
            };
        }

        public BillExternal TwoProductsInOrder(Guid orderId, params string[] productName)
        {
            return new BillExternal
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
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        ProductName = productName[1]
                    }
                }
            };
        }

        public BillExternal ThreeProductsBy4Items(Guid orderId, params string[] productName)
        {
            return new BillExternal
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
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    }
                }
            };
        }

        public BillExternal CancellAllProductsBy1QtyAndCheckout(Guid orderId, params string[] productName)
        {
            return new BillExternal
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
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 4,
                        Discount = 0,
                        AmountDiscounted = 4,
                        PersonId = 0,
                        ProductName = productName[0]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 7,
                        Discount = 0,
                        AmountDiscounted = 7,
                        PersonId = 0,
                        ProductName = productName[1]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    },
                    new BillItemExternal
                    {
                        Amount = 2.5m,
                        Discount = 0,
                        AmountDiscounted = 2.5m,
                        PersonId = 0,
                        ProductName = productName[2]
                    }
                }
            };
        }

    }
}
