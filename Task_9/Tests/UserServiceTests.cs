using NUnit.Framework;
using System.Net;
using Task_9.Core.Clients;
using Task_9.Core.Models.Requests;
using Task_9.Core.Utils;

namespace Task_9.Tests
{
    public class UserServiceTests: BaseTest
    {
        private readonly string _noElementsMessage = "Sequence contains no elements";
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
        public async Task T1to7_RegisterNewUser_NewValidUser_StatusCodeIsSuccsses(RegisterNewUserRequest request)
        {
            // Action
            var response = await _userProvider.RegisterValidUser(request);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Status,
                $"User with FirstName = {request.FirstName} and LastName = {request.LastName} IS NOT register");
        }

        [Test]
        //8
        public async Task T8_RegisterNewUser_SomeNewValidUsers_ReturningUserIdIsAutoincremented()
        {
            // Action
            var responseRegisterUser1 = await _userProvider.RegisterValidUser();
            var responseRegisterUser2 = await _userProvider.RegisterValidUser();
            var responseRegisterUser3 = await _userProvider.RegisterValidUser();

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
        public async Task T9_DeleteUser_RegisterNewUser_DeleteExistUserAndRegisterNewValidUser_NewUserIdIsIncremented()
        {
            // Action
            var userId1 = await _userProvider.GetNotActiveUserId();
            var responseDeleteUser1 = await _userProvider.DeleteExistUser(userId1);
            var responseGetStatusDeletedUser1 = await _userProvider.GetStatusNotExistUser(userId1);
            var responseRegisterUser2 = await _userProvider.RegisterValidUser();
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseDeleteUser1.Status);
                Assert.AreEqual(HttpStatusCode.NotFound, responseGetStatusDeletedUser1.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseRegisterUser2.Status);
                Assert.IsTrue(responseRegisterUser2.Body > userId1);
            });
        }

        [Test]
        //10
        public async Task T10_GetUserStatus_NotExistRegisterUser_StatusCodeIsNotFound()
        {
            //Precondition
            var notExistUserId = await _userProvider.GetNotExistUserId();
            // Action
            var responseGetUserStatus = await _userProvider.GetStatusNotExistUser(notExistUserId);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, responseGetUserStatus.Status);
                Assert.AreEqual(_cannotFindUserIdMessage, responseGetUserStatus.Content);
            });
        }

        [TestCase(false)]
        //11
        public async Task T11_RegisterNewUser_GetUserStatus_DefaultUserStatus_UserStatusIsFalse(bool defaultUserStatus)
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId();
            // Action
            var responseGetUserStatus = await _userProvider.GetStatusExistUser(userId);
            // Assert
            Assert.AreEqual(defaultUserStatus, responseGetUserStatus.Body);
        }

        [Test]
        //13,15
        public async Task T13_15_SetUserStatus_GetUserStatus_ChangeUserStatusFromFalseToTrue_UserStatusIsTrue()
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId();
            // Action
            var responseSetUserStatus = await _userProvider.SetTrueStatusExistUser(userId);
            var responseGetUserStatus = await _userProvider.GetStatusExistUser(userId);
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
        public async Task T12_16_SetUserStatus_GetUserStatus_ChangeUserStatusFalseTrueFalse_UserStatusIsFalse()
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId();
            // Action
            await _userProvider.SetTrueStatusExistUser(userId);
            var responseSetUserStatus = await _userProvider.SetFalseStatusExistUser(userId);
            var responseGetUserStatus = await _userProvider.GetStatusExistUser(userId);
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
        public async Task T14_SetUserStatus_NotExistRegisterUser_StatusCodeNotFound()
        {
            // Precondition
            var notExistUserId = await _userProvider.GetNotExistUserId();
            // Action
            var responseSetUserStatus = await _userProvider.SetTrueStatusNotExistUser(notExistUserId);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, responseSetUserStatus.Status);
                Assert.AreEqual(_cannotFindUserIdMessage, responseSetUserStatus.Content);
            });
        }

        [Test]
        //17
        public async Task T17_SetUserStatus_GetUserStatus_ChangeUserStatusFalseTrueFalseTrue_UserStatusIsFalse()
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId();
            // Action
            await _userProvider.SetTrueStatusExistUser(userId);
            await _userProvider.SetFalseStatusExistUser(userId);
            var responseSetUserStatus = await _userProvider.SetTrueStatusExistUser(userId);
            var responseGetUserStatus = await _userProvider.GetStatusExistUser(userId);
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
        public async Task T18_SetUserStatus_GetUserStatus_ChangeUserStatusFalseToFalse_UserStatusIsFalse()
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId();
            // Action
            var responseSetUserStatus = await _userProvider.SetFalseStatusExistUser(userId);
            var responseGetUserStatus = await _userProvider.GetStatusExistUser(userId);
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
        public async Task T19_SetUserStatus_GetUserStatus_ChangeUserStatusTrueToTrue_UserStatusIsTrue()
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId(); ;
            // Action
            await _userProvider.SetTrueStatusExistUser(userId);
            var responseSetUserStatus = await _userProvider.SetTrueStatusExistUser(userId);
            var responseGetUserStatus = await _userProvider.GetStatusExistUser(userId);
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
        public async Task T20_DeleteUser_NotActiveUser_StatusCodeIsOK()
        {
            // Precondition
            var userId = await _userProvider.GetNotActiveUserId();
            // Action
            var responseDeleteUser = await _userProvider.DeleteExistUser(userId);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, responseDeleteUser.Status);

        }

        [Test]
        //21
        public async Task T21_DeleteUser_NotExistRegisterUser_StatusCodeIsInternalServerError()
        {
            // Precondition
            var notExistUserId = await _userProvider.GetNotExistUserId();
            // Action
            var responseDeleteUser = await _userProvider.DeleteNotExistUser(notExistUserId);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseDeleteUser.Status);
                Assert.AreEqual(_noElementsMessage, responseDeleteUser.Content);
            });

        }

    }
}