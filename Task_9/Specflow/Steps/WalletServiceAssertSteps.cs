using NodaTime;
using NUnit.Framework;
using System.Net;
using Task_9.Core.Enum;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public sealed class WalletServiceAssertSteps
    {
        private readonly UserDataContext _userContext;
        private readonly WalletDataContext _walletContext;

        public WalletServiceAssertSteps(UserDataContext userContext,
            WalletDataContext walletContext)
        {
            _userContext = userContext;
            _walletContext = walletContext;
        }

        [Then(@"get user balance response Status is '([^']*)'")]
        public void ThenGetUserBalanceResponseStatusIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _walletContext.GetBalanceResponse.Status);
        }

        [Then(@"get user balance response Content is '([^']*)'")]
        public void ThenGetUserBalanceResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _walletContext.GetBalanceResponse.Content);
        }
        [Then(@"user balance is more than (.*)")]
        public void ThenUserBalanceShouldBeMoreThan(decimal expected)
        {
            Assert.IsTrue(_walletContext.GetBalanceResponse.Body > expected);
        }
        [Then(@"user balance should be (.*)")]
        public void ThenUserBalanceShouldBe(decimal expected)
        {
            Assert.AreEqual(expected, _walletContext.GetBalanceResponse.Body);
        }
        [Then(@"charge balance response Status is '([^']*)'")]
        public void ThenChargeBalanceResponseStatusIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _walletContext.BalanceChargeResponse.Status);
        }

        [Then(@"charge balance response Content is: (.*)")]
        public void ThenChargeBalanceResponseContentIs(string expected)
        {
           Assert.AreEqual(expected, _walletContext.BalanceChargeResponse.Content);
        }

        [Then(@"revert transaction response Status is '([^']*)'")]
        public void ThenRevertTransactionResponseStatusIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _walletContext.RevertTransactionResponse.Status);
        }

        [Then(@"revert transaction response Content is: (.*)")]
        public void ThenRevertTransactionResponseContentIs(string expected)
        {
            Assert.AreEqual(expected, _walletContext.RevertTransactionResponse.Content);
        }
        [Then(@"get transaction response Status is '([^']*)'")]
        public void ThenGetTransactionResponseStatusIs(HttpStatusCode expected)
        {
            Assert.AreEqual(expected, _walletContext.GetTransactionResponse.Status);
        }

        [Then(@"count of transactions should be (.*)")]
        public void ThenCountOfTransactionsShouldBe(int expected)
        {
            Assert.AreEqual(expected, _walletContext.GetTransactionResponse.Body.Count);
        }

        [Then(@"check all get transaction response fields")]
        public void ThenCheckAllGetTransactionResponseFields()
        {
            DateTimeZone desiredTimeZone = DateTimeZoneProviders.Tzdb["Etc/GMT"];
            ZonedDateTime currentZonedDateTime = SystemClock.Instance.GetCurrentInstant().InZone(desiredTimeZone);
            LocalDate expectedDate = currentZonedDateTime.Date;
            LocalDate actualDate = LocalDate.FromDateTime(_walletContext.GetTransactionResponse.Body[0].Time.Date);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(_userContext.UserId, _walletContext.GetTransactionResponse.Body[0].UserId);
                Assert.AreEqual(100, _walletContext.GetTransactionResponse.Body[0].Amount);
                Assert.AreEqual(_walletContext.TransactionId, _walletContext.GetTransactionResponse.Body[0].TransactionId);
                Assert.AreEqual(expectedDate, actualDate);
                Assert.AreEqual(TransactionStatus.NotReverted, _walletContext.GetTransactionResponse.Body[0].Status);
                Assert.AreEqual(null, _walletContext.GetTransactionResponse.Body[0].BaseTransactionId);
            });
        }
        [Then(@"check get transaction response fields after revert")]
        public void ThenCheckGetTransactionResponseFieldsAfterRevert()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, _walletContext.GetTransactionResponse.Body.Count);
                Assert.AreEqual(TransactionStatus.NotReverted, _walletContext.GetTransactionResponse.Body[0].Status);
                Assert.AreEqual(TransactionStatus.Reverted, _walletContext.GetTransactionResponse.Body[1].Status);
                Assert.AreEqual(_walletContext.GetTransactionResponse.Body[1].TransactionId, _walletContext.GetTransactionResponse.Body[0].BaseTransactionId);
                Assert.AreNotEqual(_walletContext.GetTransactionResponse.Body[1].TransactionId, _walletContext.GetTransactionResponse.Body[0].TransactionId);
                Assert.AreEqual(null, _walletContext.GetTransactionResponse.Body[1].BaseTransactionId);
            });
        }
    }
}