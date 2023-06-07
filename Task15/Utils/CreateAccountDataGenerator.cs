using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task15.Models;

namespace Task15.Utils
{
    public class CreateAccountDataGenerator
    {
        private Faker _faker = new Faker();
        public CreateAccountData GenerateCreateAccountData()
        {
            return new CreateAccountData
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Password = _faker.Internet.Password()
            };
        }
    }
}
