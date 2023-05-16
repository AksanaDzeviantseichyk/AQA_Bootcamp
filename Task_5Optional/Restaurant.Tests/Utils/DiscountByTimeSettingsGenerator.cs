using RestaurantErp.Core.Models.Discount;
using RestaurantErp.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Utils
{
    public class DiscountByTimeSettingsGenerator
    {
        public DiscountByTimeSettings GenerateDiscountByTimeSettings(Guid productID, int discountMinutes, decimal discountValue)
        {
            return new DiscountByTimeSettings()
            {
                ProductId = productID,
                StartTime = TimeOnly.FromDateTime(DateTime.UtcNow),
                EndTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddMinutes(discountMinutes),
                DiscountValue = discountValue
        }
    }
}
