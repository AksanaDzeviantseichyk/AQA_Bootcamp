using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Task_9.Clients;
using Task_9.Models.Requests;
using Task_9.Utils;

namespace Task_9
{
    public class WalletServiceTests
    {
        private readonly WalletServiceClient _walletServiceClient = new WalletServiceClient();
        private readonly BalanceChargeGenerator _balanceChargeGenerator = new BalanceChargeGenerator();
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        private readonly string _notActiveUserMessage = "not active user";

        [Test]
        //1,2,15
        public async Task T1_2_15_GetBalance_NewNotActiveUserNoTransaction_StatusCodeIsInternalServerError()
        {
            //Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            //Action
            var responseGetBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseGetBalance.Status);
                Assert.AreEqual(_notActiveUserMessage, responseGetBalance.Content);
            });
        }

        [TestCase(100, -0.01)]
        //16, 35
        public async Task T16_35_GetBalance_RevertOneTransactionAndGetBalance_BalanceIsEqualPreviousValue(decimal balance, decimal amountRevert)
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest1 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, balance);
            await _walletServiceClient.BalanceCharge(balanceChargeRequest1);
            var expectedBalanceAfterRevert = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            var balanceChargeRequest2 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amountRevert);
            var revertRequest = await _walletServiceClient.BalanceCharge(balanceChargeRequest2);
            //Action
            await _walletServiceClient.RevertTransaction(revertRequest.Body);
            var actualBalanceAfterRevert = await _walletServiceClient.GetBalance(responseRegisterUser.Body);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, actualBalanceAfterRevert.Status);
                Assert.AreEqual(expectedBalanceAfterRevert.Body, actualBalanceAfterRevert.Body);
            });

        }

        [Test]
        //43
        public async Task T43_BalanceCharge_NotActiveUser_StatusCodeIsInternalServerError()
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, 100);

            //Action
            var responseGetBalance = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseGetBalance.Status);
                Assert.AreEqual(_notActiveUserMessage, responseGetBalance.Content);
            });
        }

        [TestCase(55, 30)]
        [TestCase(55, -30)]
        //53
        public async Task T53_BalanceCharge_BalanceNAndChargeSomeAmountMore_BalanceIsPositiveAndStatusCodeIsOk(decimal balanceN, decimal amount)
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest1 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, balanceN);
            await _walletServiceClient.BalanceCharge(balanceChargeRequest1);
            var balanceChargeRequest2 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest2);
            var responseGetBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseBalanceCharge.Status);
                Assert.IsTrue(responseGetBalance.Body > 0);
            });
        }

        [TestCase(50, -70)]
        //54
        public async Task T54_BalanceCharge_BalanceNAndChargeNegativeAmountMoreThanBalance_StatusCodeIsInternalServerError(decimal balanceN, decimal amount)
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest1 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, balanceN);
            await _walletServiceClient.BalanceCharge(balanceChargeRequest1);
            var balanceChargeRequest2 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest2);
            //Assert
            var expectedMessage = $"User have '{balanceN:0.0}', you try to charge '{amount:0.0}'.";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(expectedMessage, responseBalanceCharge.Content);
            });
        }

        [TestCase(-50)]
        //55
        public async Task T55_BalanceCharge_Balance0MinusSomeAmount_StatusCodeIsInternalServerError(decimal negativeAmount)
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(
                responseRegisterUser.Body, negativeAmount);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            var expectedMessage = $"User have '{0}', you try to charge '{negativeAmount:0.0}'.";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(expectedMessage, responseBalanceCharge.Content);
            });
        }

        //45
        [TestCase(100, -50, 0.01)]
        //56
        [TestCase(100, 10, 100)]
        [TestCase(100, 10, -100)]
        
        public async Task T45_56_BalanceCharge_BalanceNCarge10AmountChargeNAmount_BalanceIsPositiveAndStatusCodeIsOk(
            decimal balanceN, decimal charge1, decimal charge2)
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceNRequest = _balanceChargeGenerator.GenerateBalanceCharge(
                responseRegisterUser.Body, balanceN);
            await _walletServiceClient.BalanceCharge(balanceNRequest);
            var balanceChargeRequest1 = _balanceChargeGenerator.GenerateBalanceCharge(
                responseRegisterUser.Body, charge1);
            await _walletServiceClient.BalanceCharge(balanceChargeRequest1);
            var balanceChargeRequest2 = _balanceChargeGenerator.GenerateBalanceCharge(
                responseRegisterUser.Body, charge2);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest2);
            var responseGetBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseBalanceCharge.Status);
                Assert.IsTrue(responseGetBalance.Body > 0);
            });
        }

        [TestCase(0.01)]
        [TestCase(10000000)]
        [TestCase(999999.99)]
        
        //34, 36, 39
        public async Task T34_36_39_RevertTransaction_TransactionWithSomeAmount_StatusCodeIsOk(decimal amount)
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var expectedBalanceAfterRevert = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            var revertRequest = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Action
            await _walletServiceClient.RevertTransaction(revertRequest.Body);
            var actualBalanceAfterRevert = await _walletServiceClient.GetBalance(responseRegisterUser.Body);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, actualBalanceAfterRevert.Status);
                Assert.AreEqual(expectedBalanceAfterRevert.Body, actualBalanceAfterRevert.Body);
            });
        }

        [Test]
        //17
        public async Task T17_GetTransaction_UserIsActiveNotTransaction_StatusCodeIsOkTransactionCountIsZero()
        {
            //Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            var activeUser = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            //Action
            var responseTransaction = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseTransaction.Status);
                Assert.IsTrue(responseTransaction.Body.Count() == 0);
            });
        }

        [TestCase(100)]
        //18
        public async Task T18_GetTransaction_ChargeAmountOneTime_StatusCodeIsOkTransactionCountIsOne(decimal amount)
        {
            //Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            var activeUser = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            var response = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Action
            var responseTransaction = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseTransaction.Status);
                Assert.IsTrue(responseTransaction.Body.Count() == 1);
            });
        }

        [TestCase(100, 2)]
        //19
        public async Task T19_GetTransaction_ChargeAmountTwoTimes_StatusCodeIsOkTransactionCountIsTwo(decimal amount, int count)
        {
            //Precondition
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            var activeUser = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            var response = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            response = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Action
            var responseTransaction = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseTransaction.Status);
                Assert.IsTrue(responseTransaction.Body.Count() == count);
            });
        }
    }
}
