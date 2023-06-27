using NUnit.Framework;
using System.Net;
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
            //string formattedExpected = string.Format(expected, 0, -0.01);
            Assert.AreEqual(expected, _walletContext.BalanceChargeResponse.Content);
        }
    }
}