using NUnit.Framework;
using System.Net;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public class UserServiceAssertSteps
    {
        private readonly DataContext _context;

        public UserServiceAssertSteps(DataContext context)
        {
            _context = context;
        }
        [Then(@"register new user response Status is '([^']*)'")]
        public void ThenRegisterNewUserResponseStatusIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _context.RegisterNewUserResponse.Status);
        }

        [Then(@"user id should be incremented")]
        public void ThenUserIdShouldBeIncremented()
        {
            bool isSorted = _context.NewUserIds
                .Zip(_context.NewUserIds
                    .Skip(1), (current, next) => current < next)
                .All(x => x);
         
            Assert.IsTrue(isSorted);

        }
        [Then(@"new user id after deleting should be incremented")]
        public void ThenNewUserIdAfterDeletingShouldBeIncremented()
        {
            Assert.IsTrue(_context.UserId < _context.RegisterNewUserResponse.Body);
           
        }

        [Then(@"get user status response Status is '([^']*)'")]
        public void ThenGetUserStatusResponseStatusCodeIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _context.GetUserStatusResponse.Status);
            
        }

        [Then(@"get user status response Content is '(.*)'")]
        public void ThenGetUserStatusResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _context.GetUserStatusResponse.Content);
        }
        [Then(@"user isActive status should be '([^']*)'")]
        public void ThenUserIsActiveStatusShouldBe(bool expected)
        {
            Assert.AreEqual(expected, _context.GetUserStatusResponse.Body);
        }
        [Then(@"delete user response Status is '([^']*)'")]
        public void ThenDeleteUserResponseStatusCodeIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _context.DeleteUserResponse.Status);

        }

        [Then(@"delete user response Content is '(.*)'")]
        public void ThenDeleteUserResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _context.DeleteUserResponse.Content);
        }

        [Then(@"set user status response Status is '([^']*)'")]
        public void ThenSetUserStatusResponseStatusCodeIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _context.SetUserStatusResponse.Status);

        }
        
        [Then(@"set user status response Content is '(.*)'")]
        public void ThenSetUserStatusResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _context.SetUserStatusResponse.Content);
        }
    }
}