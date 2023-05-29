using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Task_9.Core.Models.Requests;

namespace Task_9.Core.Utils
{
    public class UserGenerator
    {
        private Faker _faker = new Faker();
        public RegisterNewUserRequest GenerateRegisterNewUserRequest()
        {

            return GenerateRegisterNewUserRequest(_faker.Name.FirstName(), _faker.Name.LastName());
        }

        public RegisterNewUserRequest GenerateEmptyUserFieldsRequest()
        {

            return GenerateRegisterNewUserRequest("", "");
        }

        public RegisterNewUserRequest GenerateNullUserFieldsRequest()
        {

            return GenerateRegisterNewUserRequest(null, null);
        }

        public RegisterNewUserRequest GenerateUserFieldsWithDigitsRequest()
        {
            string chars = "0123456789";
            return GenerateRegisterNewUserRequest(GenerateRandomString(chars), GenerateRandomString(chars));
        }

        public RegisterNewUserRequest GenerateUserFieldsWithSpecialCharactersRequest()
        {
            string chars = "!@#$%^&*()_+";
            return GenerateRegisterNewUserRequest(GenerateRandomString(chars), GenerateRandomString(chars));
        }
        public RegisterNewUserRequest GenerateUserFieldsWithLength1SymbolRequest()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return GenerateRegisterNewUserRequest(
                GenerateRandomString(chars, 1),
                GenerateRandomString(chars, 1));
        }

        public RegisterNewUserRequest GenerateUserFieldsWithLength100MoreSymbolRequest()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return GenerateRegisterNewUserRequest(
                GenerateRandomString(chars, 101),
                GenerateRandomString(chars, 101));
        }
        public RegisterNewUserRequest GenerateUpperCaseUserFieldsRequest()
        {

            return GenerateRegisterNewUserRequest(_faker.Name.FirstName().ToUpper(), _faker.Name.LastName().ToUpper());
        }

        public RegisterNewUserRequest GenerateRegisterNewUserRequest(string? firstName, string? lastName)
        {
            return new RegisterNewUserRequest()
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

        private string GenerateRandomString(string chars, int length = 5)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_faker.Random.Int(0, s.Length - 1)]).ToArray());
        }

    }
}
