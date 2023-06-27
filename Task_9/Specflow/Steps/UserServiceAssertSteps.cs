using NUnit.Framework;
using System.Net;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public class UserServiceAssertSteps
    {
        private readonly UserDataContext _userContext;

        public UserServiceAssertSteps(UserDataContext userContext)
        {
            _userContext = userContext;
        }
        [Then(@"register new user response Status is '([^']*)'")]
        public void ThenRegisterNewUserResponseStatusIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _userContext.RegisterNewUserResponse.Status);
        }

        [Then(@"user id should be incremented")]
        public void ThenUserIdShouldBeIncremented()
        {
            bool isSorted = _userContext.NewUserIds
                .Zip(_userContext.NewUserIds
                    .Skip(1), (current, next) => current < next)
                .All(x => x);
         
            Assert.IsTrue(isSorted);

        }
        [Then(@"new user id after deleting should be incremented")]
        public void ThenNewUserIdAfterDeletingShouldBeIncremented()
        {
            Assert.IsTrue(_userContext.UserId < _userContext.RegisterNewUserResponse.Body);
           
        }

        [Then(@"get user status response Status is '([^']*)'")]
        public void ThenGetUserStatusResponseStatusCodeIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _userContext.GetUserStatusResponse.Status);
            
        }

        [Then(@"get user status response Content is '(.*)'")]
        public void ThenGetUserStatusResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _userContext.GetUserStatusResponse.Content);
        }
        [Then(@"user isActive status should be '([^']*)'")]
        public void ThenUserIsActiveStatusShouldBe(bool expected)
        {
            Assert.AreEqual(expected, _userContext.GetUserStatusResponse.Body);
        }
        [Then(@"delete user response Status is '([^']*)'")]
        public void ThenDeleteUserResponseStatusCodeIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _userContext.DeleteUserResponse.Status);

        }

        [Then(@"delete user response Content is '(.*)'")]
        public void ThenDeleteUserResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _userContext.DeleteUserResponse.Content);
        }

        [Then(@"set user status response Status is '([^']*)'")]
        public void ThenSetUserStatusResponseStatusCodeIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _userContext.SetUserStatusResponse.Status);

        }
        
        [Then(@"set user status response Content is '(.*)'")]
        public void ThenSetUserStatusResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _userContext.SetUserStatusResponse.Content);
        }
    }
}