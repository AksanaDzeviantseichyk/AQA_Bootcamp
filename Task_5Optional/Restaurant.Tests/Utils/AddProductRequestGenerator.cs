using RestaurantErp.Core.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Restaurant.Tests.Utils
{
    public class AddProductRequestGenerator
    {
        public AddProductRequest GenerateAddProductRequest(string name, decimal price )
        {
            return new AddProductRequest()
            {
                Name = name,
                Price = price
            };
        }


    }
}
