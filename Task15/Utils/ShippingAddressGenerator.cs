using Bogus;
using Task15.Models;

namespace Task15.Utils
{
    public class ShippingAddressGenerator
    {
        private Faker _faker = new Faker();
        public ShippingAddress GenerateShippingAddress()
        {
            return new ShippingAddress()
            {
                Country = "United Kingdom",
                Street = "Spur Road",
                City = "London",
                PostalCode = "SW1A 1AA",
                PhoneNumber = _faker.Phone.PhoneNumber()
            };
        }
    }
}
