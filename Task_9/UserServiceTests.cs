using NUnit.Framework;
using System.Net;
using Task_9.Clients;
using Task_9.Models.Requests;
using Task_9.Utils;

namespace Task_9
{
    public class UserServiceTests
    {
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        private readonly string _noElementsMessage = "Sequence contains no matching element";
        private readonly string _cannotFindUserIdMessage = "Specified argument was out of the range of valid values. (Parameter 'cannot find user with this id')";

        private static IEnumerable<TestCaseData> ValidUserInfo()
        {
            UserGenerator userGenerator = new UserGenerator();
            //1
            yield return new TestCaseData(userGenerator.GenerateEmptyUserFieldsRequest());
            //2
            yield return new TestCaseData(userGenerator.GenerateNullUserFieldsRequest());
            //3
            yield return new TestCaseData(userGenerator.GenerateUserFieldsWithLength1SymbolRequest());
            //4
            yield return new TestCaseData(userGenerator.GenerateUserFieldsWithLength100MoreSymbolRequest());
            //5
            yield return new TestCaseData(userGenerator.GenerateUpperCaseUserFieldsRequest());
            //6
            yield return new TestCaseData(userGenerator.GenerateUserFieldsWithDigitsRequest());
            //7
            yield return new TestCaseData(userGenerator.GenerateUserFieldsWithSpecialCharactersRequest());
        }

        [TestCaseSource(nameof(ValidUserInfo))]
        //1,2,3,4,5,6,7
        public async Task T1to7_ValidUser_RegisterNewUser_StatusCodeIsSuccsses(RegisterNewUserRequest request)
        {
            // Action
            var response = await _userServiceClient.RegisterNewUser(request);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Status, 
                $"User with FirstName = {request.FirstName} and LastName = {request.LastName} IS NOT register");
        }

        [Test]
        //8
        public async Task T8_ValidSomeUsers_RegisterNewUser_ReturningUserIdIsAutoincremented()
        {
            // Precondition
            var request1 = _userGenerator.GenerateRegisterNewUserRequest();
            
            // Action
            var responseRegisterUser1 = await _userServiceClient.RegisterNewUser(request1);
            var responseRegisterUser2 = await _userServiceClient.RegisterNewUser(request1);
            var responseRegisterUser3 = await _userServiceClient.RegisterNewUser(request1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(responseRegisterUser2.Body > responseRegisterUser1.Body);
                Assert.IsTrue(responseRegisterUser3.Body > responseRegisterUser1.Body);
                Assert.IsTrue(responseRegisterUser3.Body > responseRegisterUser2.Body);
            });
        }

        [Test]
        //9
        public async Task T9_DeleteExistUserAndRegisterNewValidUser_DeleteUser_RegisterNewUser_NewUserIdIsIncremented()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            // Action
            var responseRegisterUser1 = await _userServiceClient.RegisterNewUser(request);
            await _userServiceClient.DeleteUser(Convert.ToInt32(responseRegisterUser1.Body));
            var responseRegisterUser2 = await _userServiceClient.RegisterNewUser(request);
            // Assert
            Assert.IsTrue(responseRegisterUser2.Body > responseRegisterUser1.Body);
        }

        [Test]
        //10
        public async Task T10_NotExistRegisterUser_GetUserStatus_StatusCodeIsNotFound()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body + 1);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, responseGetUserStatus.Status);
                Assert.AreEqual(_cannotFindUserIdMessage, responseGetUserStatus.Content);
            });
        }

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
        //13,15
        public async Task T13_15_ChangeUserStatusFromFalseToTrue_SetUserStatus_GetUserStatus_UserStatusIsTrue()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseSetUserStatus = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseSetUserStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseGetUserStatus.Status);
                Assert.AreEqual(true, responseGetUserStatus.Body);
            });

        }

        [Test]
        //12,16
        public async Task T12_16_ChangeUserStatusFalseTrueFalse_SetUserStatus_GetUserStatus_UserStatusIsFalse()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var responseSetUserStatus = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, false);
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseSetUserStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseGetUserStatus.Status);
                Assert.AreEqual(false, responseGetUserStatus.Body);
            });
        }

        [Test]
        //14
        public async Task T14_NotExistRegisterUser_SetUserStatus_StatusCodeNotFound()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseSetUserStatus = await _userServiceClient.SetUserStatus(responseRegisterUser.Body+1, true);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, responseSetUserStatus.Status);
                Assert.AreEqual(_cannotFindUserIdMessage, responseSetUserStatus.Content);
            });
        }

        [Test]
        //17
        public async Task T17_ChangeUserStatusFalseTrueFalseTrue_SetUserStatus_GetUserStatus_UserStatusIsFalse()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, false);
            var responseSetUserStatus = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseSetUserStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseGetUserStatus.Status);
                Assert.AreEqual(true, responseGetUserStatus.Body);
            });
        }

        [Test]
        //18
        public async Task T18_ChangeUserStatusFalseToFalse_SetUserStatus_GetUserStatus_UserStatusIsFalse()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseSetUserStatus = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, false);
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseSetUserStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseGetUserStatus.Status);
                Assert.AreEqual(false, responseGetUserStatus.Body);
            });
        }

        [Test]
        //19
        public async Task T19_ChangeUserStatusTrueToTrue_SetUserStatus_GetUserStatus_UserStatusIsTrue()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var responseSetUserStatus = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var responseGetUserStatus = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseSetUserStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseGetUserStatus.Status);
                Assert.AreEqual(true, responseGetUserStatus.Body);
            });
        }

        [Test]
        //20
        public async Task T20_NotActiveUser_DeleteUser_StatusCodeIsOK()
        {
            // Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            // Action
            var responseDeleteUser = await _userServiceClient.DeleteUser(responseRegisterUser.Body);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, responseDeleteUser.Status);

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
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseDeleteUser.Status);
                Assert.AreEqual(_noElementsMessage, responseDeleteUser.Content);
            });

        }

    }
}