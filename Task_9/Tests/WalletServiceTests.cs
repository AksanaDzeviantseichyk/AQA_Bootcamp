﻿using NUnit.Framework;
using System.Net;
using Bogus;
using Task_9.Core.Clients;
using Task_9.Core.Enum;
using Task_9.Core.Utils;
using NodaTime;
using System;

namespace Task_9.Tests
{
    public class WalletServiceTests: BaseTest
    {
        private readonly WalletServiceClient _walletServiceClient = WalletServiceClient.Instance;
        private readonly BalanceChargeGenerator _balanceChargeGenerator = new BalanceChargeGenerator();
        private readonly string _notActiveUserMessage = "not active user";

        [Test]
        //1,2,15
        public async Task T1_2_15_GetBalance_NewNotActiveUserNoTransaction_StatusCodeIsInternalServerError()
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            //Action
            var responseGetBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseGetBalance.Status);
                Assert.AreEqual(_notActiveUserMessage, responseGetBalance.Content);
            });
        }

        [Test]
        //3
        public async Task T3_GetBalance_NotExistUser_StatusCodeIsInternalServerError()
        {
            //Precondition
            var notExistUserId = await _userProvider.GetNotExistUserId();
            //Action
            var responseGetBalance = await _walletServiceClient.GetBalance(notExistUserId);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseGetBalance.Status);
                Assert.AreEqual(_notActiveUserMessage, responseGetBalance.Content);
            });
        }

        //10,49
        [TestCase(0.01)]
        //13
        [TestCase(10000000)]
        //12
        [TestCase(9999999.99)]
        public async Task T10_12_13_49_GetBalance_OneTransictionWithSomePositiveAmount_StatusCodeIsOk(decimal amount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest1 = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest1);
            var responseGetBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseBalanceCharge.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseGetBalance.Status);
                Assert.AreEqual(amount, responseGetBalance.Body);
            });
        }

        //11
        [TestCase(-0.01)]
        //14
        [TestCase(-10000000.01)]
        public async Task T11_14_BalanceCharge_OneTransactionWithSomeNegativeAmount_StatusCodeIsInternalServerError(decimal negativeAmount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(
                responseRegisterUser.Body, negativeAmount);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            var expectedMessage = $"User have '{0}', you try to charge '{negativeAmount}'.";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(expectedMessage, responseBalanceCharge.Content);
            });
        }

        [TestCase(100, -0.01)]
        //16, 35
        public async Task T16_35_GetBalance_RevertOneTransactionAndGetBalance_BalanceIsEqualPreviousValue(decimal balance, decimal amountRevert)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
           
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
            var responseRegisterUser = await _userProvider.RegisterValidUser();
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

        [Test]
        //44
        public async Task T44_BalanceCharge_NotExistUser_StatusCodeIsInternalServerError()
        {
            //Precondition
            var notExistUserId = await _userProvider.GetNotExistUserId();
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(notExistUserId, 100);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(_notActiveUserMessage, responseBalanceCharge.Content);
            });
        }

        [Test]
        //46
        public async Task T46_BalanceCharge_Charge0Amount_StatusCodeIsInternalServerError()
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, 0);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            var expectedMessage = "Amount cannot be '0'";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(expectedMessage, responseBalanceCharge.Content);
            });
        }

        [Test]
        //48
        public async Task T48_BalanceCharge_ChargeAmountWithPrecision2_StatusCodeIsInternalServerError()
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, 0.001m);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            var expectedMessage = "Amount value must have precision 2 numbers after dot";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(expectedMessage, responseBalanceCharge.Content);
            });
        }

        [TestCase(55, 30)]
        [TestCase(55, -30)]
        //53
        public async Task T53_BalanceCharge_BalanceNAndChargeSomeAmountMore_BalanceIsPositiveAndStatusCodeIsOk(decimal balanceN, decimal amount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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

        [TestCase(-30)]
        //55
        public async Task T55_BalanceCharge_Balance0MinusSomeAmount_StatusCodeIsInternalServerError(decimal negativeAmount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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

        //45,50
        [TestCase(100, -50, 0.01)]
        [TestCase(100, -50, -0.01)]
        //56
        [TestCase(100, 10, 100)]
        [TestCase(100, 10, -100)]

        public async Task T45_50_56_BalanceCharge_BalanceNCarge10AmountChargeNAmount_BalanceIsPositiveAndStatusCodeIsOk(
            decimal balanceN, decimal charge1, decimal charge2)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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

        [TestCase(10000000.01)]
        //47
        public async Task T47_BalanceCharge_Balance0ChargeAmountMoreThanMaxSum_StatusCodeIsInternalServerError(decimal inpossibleAmount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(
                responseRegisterUser.Body, inpossibleAmount);

            //Action
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Assert
            var expectedMessage = $"After this charge balance could be '{inpossibleAmount}', maximum user balance is '10000000'";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseBalanceCharge.Status);
                Assert.AreEqual(expectedMessage, responseBalanceCharge.Content);
            });
        }

        [Test]
        //33
        public async Task T33_RevertTransaction_RevertTransactionWithWrongId_StatusCodeIsNotFound()
        {
            //Action
            var responseRevertTransaction = await _walletServiceClient.RevertTransaction(new Guid());
            //Assert
            var expectedMessage = "The given key was not present in the dictionary.";
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, responseRevertTransaction.Status);
                Assert.AreEqual(expectedMessage, responseRevertTransaction.Content);
            });

        }

        //34
        [TestCase(0.01)]
        //36
        [TestCase(10000000)]
        //39
        [TestCase(999999.99)]
        public async Task T34_36_39_RevertTransaction_TransactionWithSomeAmount_StatusCodeIsOk(decimal amount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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

        [TestCase(20, -10)]
        //37
        public async Task T37_RevertTransaction_RevertTransactionWith10kkMore_StatusCodeIsOk(decimal amount1, decimal amount2)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount1);
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount2);
            await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            await _walletServiceClient.RevertTransaction(responseBalanceCharge.Body);
            var expectedBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
            balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, 10000000.01m);
            responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Action
            var responseRevertTransaction = await _walletServiceClient.RevertTransaction(responseBalanceCharge.Body);
            var actualBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseRevertTransaction.Status);
                Assert.AreEqual(expectedBalance.Body, actualBalance.Body);
            });
        }

        [TestCase(20)]
        //38
        public async Task T38_RevertTransaction_RevertOfRevert_StatusCodeIsOk(decimal amount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Action
            var responseRevertTransaction = await _walletServiceClient.RevertTransaction(responseBalanceCharge.Body);
            var responseRevertOfRevertTransaction = await _walletServiceClient.RevertTransaction(responseRevertTransaction.Body);
            var actualBalance = await _walletServiceClient.GetBalance(responseRegisterUser.Body);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseRevertTransaction.Status);
                Assert.AreEqual(HttpStatusCode.OK, responseRevertOfRevertTransaction.Status);
                Assert.AreEqual(amount, actualBalance.Body);
            });
        }

        [Test]
        //17
        public async Task T17_GetTransaction_UserIsActiveNotTransaction_StatusCodeIsOkTransactionCountIsZero()
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
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

        [TestCase(20)]
        //21
        public async Task T21_GetTransaction_CheckAllFields_AllFieldsIsCorrect(decimal amount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            var responseChargeBalance = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            DateTimeZone desiredTimeZone = DateTimeZoneProviders.Tzdb["Etc/GMT"];
            ZonedDateTime currentZonedDateTime = SystemClock.Instance.GetCurrentInstant().InZone(desiredTimeZone);
            LocalDate expectedDate = currentZonedDateTime.Date;
            
            //Action
            var responseTransaction = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);
            //Assert
            
            LocalDate actualDate = LocalDate.FromDateTime(responseTransaction.Body[0].Time.Date);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(responseRegisterUser.Body, responseTransaction.Body[0].UserId);
                Assert.AreEqual(amount, responseTransaction.Body[0].Amount);
                Assert.AreEqual(responseChargeBalance.Body, responseTransaction.Body[0].TransactionId);
                Assert.AreEqual(expectedDate, actualDate);
                Assert.AreEqual(TransactionStatus.NotReverted, responseTransaction.Body[0].Status);
                Assert.AreEqual(null, responseTransaction.Body[0].BaseTransactionId);
            });
        }

        [TestCase(100)]
        //22
        public async Task T22_GetTransaction_MakeRevertAndGetTransaction_CountOfTransactionIsTwo(decimal amount)
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            await _userProvider.SetTrueStatusExistUser(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, amount);
            var responseBalanceCharge = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            //Action
            var responseRevertTransaction = await _walletServiceClient.RevertTransaction(responseBalanceCharge.Body);
            var responseGetTransaction = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, responseGetTransaction.Body.Count);
                Assert.AreEqual(TransactionStatus.NotReverted, responseGetTransaction.Body[0].Status);
                Assert.AreEqual(TransactionStatus.Reverted, responseGetTransaction.Body[1].Status);
                Assert.AreEqual(responseGetTransaction.Body[1].TransactionId, responseGetTransaction.Body[0].BaseTransactionId);
                Assert.AreNotEqual(responseGetTransaction.Body[1].TransactionId, responseGetTransaction.Body[0].TransactionId);
                Assert.AreEqual(null, responseGetTransaction.Body[1].BaseTransactionId);
            });
        }

        [Test]
        //30
        public async Task T30_GetTransaction_NotActiveUser_StatusCodeIsOkEmtyListOfTransaction()
        {
            //Precondition
            var responseRegisterUser = await _userProvider.RegisterValidUser();
            //Action
            var responseGetBalance = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseGetBalance.Status);
                Assert.AreEqual(0, responseGetBalance.Body.Count());
            });
        }

        [Test]
        //31
        public async Task T31_GetTransaction_NotExistUser_StatusCodeIsOkEmtyListOfTransaction()
        {
            //Precondition
            var notExistUserId = await _userProvider.GetNotExistUserId();
            //Action
            var responseGetBalance = await _walletServiceClient.GetTransaction(notExistUserId);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responseGetBalance.Status);
                Assert.AreEqual(0, responseGetBalance.Body.Count());
            });
        }

    }
}
