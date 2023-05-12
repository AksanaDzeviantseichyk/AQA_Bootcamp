using RestaurantErp.Core.Models.Product;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests
{
    public class RequestProductsClass : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
                    new AddProductRequest
                    {
                         Name = "Starter",
                        Price = 4
                    },
                    new AddProductRequest
                    {
                        Name = "Main",
                        Price = 7
                    },
                    1

            };

        }
    }
}
