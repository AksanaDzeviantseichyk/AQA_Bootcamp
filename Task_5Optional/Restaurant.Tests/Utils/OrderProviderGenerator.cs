using RestaurantErp.Core.Contracts;
using RestaurantErp.Core.Helpers;
using RestaurantErp.Core.Models;
using RestaurantErp.Core.Models.Order;
using RestaurantErp.Core.Models.Product;
using RestaurantErp.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Utils
{
    public class OrderProviderGenerator
    {
        public OrderProvider GenerateOrderProvider(ProductProvider productProvider, 
            IDiscountByTimeProvider discountManager, IDiscountCalculator calculator, decimal serviceRate)
        {
            return new OrderProvider((IPriceStorage)productProvider,
                new[] { (IDiscountProvider)discountManager },
                calculator,
                new TimeHelper(),
                new BillHelper(productProvider),
                new ServiceChargeProvider(new ServiceChargeProviderSettings { ServiceRate = serviceRate }));
        }
    }
}
