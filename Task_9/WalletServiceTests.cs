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
        public async Task T1_2_15_NewNotActiveUserNoTransaction_GetBalance_StatusCodeIsInternalServerError()
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

        [Test]
        //
        public async Task T16_RevertTransaction_GetBalance_BalanceIsEqualPreviousValue()
        {
            //Precondition
            var userRequest = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(userRequest);
            await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, 100);
            await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            var expectedBalanceAfterRevert = await _walletServiceClient.GetBalance(responseRegisterUser.Body);
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
        //43
        public async Task T43_NotActiveUser_BalanceCharge_StatusCodeIsInternalServerError()
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

        [Test]
        public async Task GetTransaction()
        {
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            var responseRegisterUser = await _userServiceClient.RegisterNewUser(request);
            var activeUser = await _userServiceClient.SetUserStatus(responseRegisterUser.Body, true);
            var active = await _userServiceClient.GetUserStatus(responseRegisterUser.Body);
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(responseRegisterUser.Body, 100);
            var response = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            response = await _walletServiceClient.BalanceCharge(balanceChargeRequest);
            var responseTransaction = await _walletServiceClient.GetTransaction(responseRegisterUser.Body);
            Assert.AreEqual(HttpStatusCode.OK, responseTransaction.Status);
        }
    }
}
