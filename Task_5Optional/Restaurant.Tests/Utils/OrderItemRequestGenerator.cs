using RestaurantErp.Core.Models.Order;
using RestaurantErp.Core.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Utils
{
    public class OrderItemRequestGenerator
    {
        public OrderItemRequest GenerateOrderItemRequest(Guid orderID, int count, Guid productID)
        {
            return new OrderItemRequest()
            {
                OrderId = orderID,
                Count = count,
                ProductId = productID
            };
        }
    }
}
