using Task_9.Tests;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public sealed class WalletServiceSteps: BaseTest
    {
        private readonly UserDataContext _userContext;
        private readonly WalletDataContext _walletContext;

        public WalletServiceSteps(UserDataContext userContext,
            WalletDataContext walletContext)
        {
            _userContext = userContext;
            _walletContext = walletContext;
        }
        [When(@"get user balance")]
        public async Task WhenGetUserBalance()
        {
            _walletContext.GetBalanceResponse = await _walletProvider.GetBalance(_userContext.UserId);
        }
        [Given(@"balance charge (.*)")]
        [When(@"balance charge (.*)")]
        public async Task WhenBalanceCharge(string chargeAmount)
        {
            var charges = chargeAmount.Split(",").Select(decimal.Parse).ToArray(); ;
            foreach (var charge in charges)
                _walletContext.BalanceChargeResponse = await _walletProvider.BalanceCharge(_userContext.UserId, charge);
        }
        [When(@"balance charge")]
        public async Task WhenBalanceCharge()
        {
            _walletContext.BalanceChargeResponse = await _walletProvider.BalanceCharge(_userContext.UserId);
        }

        [Given(@"get charge transaction id with '([^']*)' amount")]
        public async Task GivenGetChargeTransactionIdWithAmount(decimal amount)
        {
            _walletContext.TransactionId = await _walletProvider
                .GetChargeTransactionId(_userContext.UserId, amount);
        }

        [Given(@"revert exist transaction")]
        public async Task GivenRevertExistTransaction()
        {
            _walletContext.RevertTransactionResponse = await _walletProvider
                .RevertExistTransaction(_walletContext.TransactionId);
        }
    }
}