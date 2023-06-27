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
            {
                _walletContext.BalanceChargeResponse = await _walletProvider.BalanceCharge(_userContext.UserId, charge);
                _walletContext.BalanceChargeDictionary.TryAdd(charge, _walletContext.BalanceChargeResponse.Body);
            }
        }
        [When(@"balance charge")]
        public async Task WhenBalanceCharge()
        {
            _walletContext.BalanceChargeResponse = await _walletProvider.BalanceCharge(_userContext.UserId);
        }

        [Given(@"get charge transaction id with (.*) amount")]
        public void GivenGetChargeTransactionIdWithAmount(decimal amount)
        {
           _walletContext.BalanceChargeDictionary.TryGetValue(amount, out _walletContext.TransactionId);
        }

        [Given(@"revert transaction with (.*)")]
        [When(@"revert transaction with (.*)")]
        public async Task GivenRevertTransactionWith(decimal revertAmount)
        {
            _walletContext.BalanceChargeDictionary
                .TryGetValue(revertAmount, out _walletContext.TransactionId);
            _walletContext.RevertTransactionResponse = await _walletProvider
                .RevertExistTransaction(_walletContext.TransactionId);
            _walletContext.RevertTransactionDictionary.TryAdd(revertAmount, _walletContext.RevertTransactionResponse.Body);
        }

        [Given(@"revert of revert transaction with (.*)")]
        [When(@"revert of revert transaction with (.*)")]
        public async Task GivenRevertOfRevertTransactionWith(decimal revertAmount)
        {
            _walletContext.RevertTransactionDictionary
                .TryGetValue(revertAmount, out _walletContext.TransactionId);
            _walletContext.RevertTransactionResponse = await _walletProvider
                .RevertExistTransaction(_walletContext.TransactionId);
            _walletContext.RevertTransactionDictionary.TryAdd(revertAmount, _walletContext.RevertTransactionResponse.Body);
        }

        [When(@"revert wrong transaction")]
        public async Task WhenRevertWrongTransaction()
        {
            _walletContext.RevertTransactionResponse = await _walletProvider.RevertWrongTransaction();
        }

        [When(@"get transaction")]
        public async Task WhenGetTransaction()
        {
            _walletContext.GetTransactionResponse = await _walletProvider.GetTransaction(_userContext.UserId);
        }
    }
}