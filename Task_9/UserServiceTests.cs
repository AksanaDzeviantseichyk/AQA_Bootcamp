using NUnit.Framework;
using System.Net;
using Task_9.Clients;
using Task_9.Utils;

namespace Task_9
{
    public class UserServiceTests
    {
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        [Test]
        public async Task ValidUserInfo_RegisterNewUser_StatusCodeIsSuccsses()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            // Action
            var response = await _userServiceClient.RegisterNewUser(request);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Status);

        }
        [Test]
        public async Task ValidUserInfo_RegisterNewUser_DeleteUser_StatusCodeIsSuccsses()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            // Action
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            var responseDeleteUser = await _userServiceClient.DeleteUser(Convert.ToInt32(responseRegisterUser.Body));
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, responseDeleteUser.Status);

        }
        [Test]
        //10
        public async Task T10_NotExistRegister_GetUserStatus_StatusCodeIsSuccsses()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body + 1);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetUserStatus.Status);

        }

        [Test]
        [TestCase(false)]
        //11
        public async Task T11_DefaultUserStatus_RegisterNewUser_GetUserStatus_UserStatusIsFalse(bool defaultUserStatus)
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.AreEqual(defaultUserStatus, responseGetUserStatus.Body);

        }

        [Test]
        [TestCase(true)]
        //13,15
        public async Task T13_15_ChangeUserStatusFromFalseToTrue_SetUserStatus_GetUserStatus_UserStatusIsTrue(bool newUserStatus)
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, newUserStatus);
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.AreEqual(newUserStatus, responseGetUserStatus.Body);

        }

        [Test]
        //14
        public async Task T14_NotExistRegisterUser_SetUserStatus_StatusCodeNotFound()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body+1);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetUserStatus.Status);

        }

        [Test]
        //21
        public async Task T21_NotExistRegisterUser_DeleteUser_StatusCodeIsInternalServerError()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseDeleteUser = await _userServiceClient.DeleteUser(responseRegisterUser.Body+1);
            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, responseDeleteUser.Status);

        }

    }
}