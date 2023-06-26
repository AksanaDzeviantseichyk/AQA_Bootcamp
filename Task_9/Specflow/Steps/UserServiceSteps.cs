using Autofac;
using System;
using System.Linq.Expressions;
using Task_9.Core.Models.Responses.Base;
using Task_9.Core.Utils;
using Task_9.Tests;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public class UserServiceSteps:BaseTest
    {
        private readonly DataContext _context;

        public UserServiceSteps(DataContext context)
        {
            _context = context;
        }

        [When(@"register valid user")]
        public async Task WhenRegisterValidUser()
        {
            _context.RegisterNewUserResponse = await _userProvider.RegisterValidUser();
        }

        [When(@"register valid user with (.*)")]
        public async Task WhenRegisterValidUserWith(string condition)
        {
            _context.RegisterNewUserResponse = await _userProvider.RegisterValidUser(condition);
        }

        [When(@"register (.*) valid user[s]?")]
        public async Task WhenRegisterValidUsers(int count)
        {
            _context.NewUserIds = Enumerable.Empty<int>();
            for (int i = 0; i < count; i++)
            {
                var response = await _userProvider.RegisterValidUser();
                _context.NewUserIds = _context.NewUserIds.Append(response.Body);
            }

        }

        [Given(@"get not active user id")]
        [When(@"get not active user id")]
        public async Task WhenGetNotActiveUserId()
        {
            _context.UserId = await _userProvider.GetNotActiveUserId();
        }

        [When(@"delete user")]
        public async Task WhenDeleteUser()
        {
            _context.DeleteUserResponse = await _userProvider.DeleteExistUser(_context.UserId);
        }

        
         [Given(@"get not exist user id")]
         public async Task GivenGetNotExistUserId()
         {
            _context.UserId = await _userProvider.GetNotExistUserId();
         }

         [When(@"get user status")]
         public async Task WhenGetUserStatus()
         {
            _context.GetUserStatusResponse = await _userProvider.GetStatusNotExistUser(_context.UserId);
         }
                
        [When(@"set (.*) user status")]
        public async Task WhenSetIsActiveUserStatus(string isActive)
        {
            var isActives = isActive.Split("-").Select(bool.Parse).ToArray(); ;
            foreach(var status in isActives)
                _context.SetUserStatusResponse = await _userProvider.SetUserStatus(_context.UserId, status);
        }

    }
}
