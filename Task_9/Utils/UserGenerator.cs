using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Models.Requests;
using Bogus;

namespace Task_9.Utils
{
    public class UserGenerator
    {
        private Faker _faker = new Faker();
        public RegisterNewUserRequest GenerateRegisterNewUserRequest()
        {
            return GenerateRegisterNewUserRequest(_faker.Name.FirstName(), _faker.Name.LastName());
        }

        public RegisterNewUserRequest GenerateRegisterNewUserRequest(string firstName, string lastName)
        {
            return new RegisterNewUserRequest()
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

    }
}
