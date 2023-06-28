using System.Collections.Concurrent;
using Task_9.Core.Contracts;
using Task_9.Core.Observers;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public sealed class WalletServiceSteps
    {
        private readonly UserDataContext _userContext;
        private readonly WalletDataContext _walletContext;
        private readonly IWalletServiceProvider _walletProvider;
        
        public WalletServiceSteps(UserDataContext userContext,
            WalletDataContext walletContext,
            IWalletServiceProvider walletProvider)
        {
            _userContext = userContext;
            _walletContext = walletContext;
            _walletProvider = walletProvider;
            
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
                _walletContext.BalanceChargeDictionary.TryAdd(_walletContext.BalanceChargeResponse.Body, charge);
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
            _walletContext.TransactionId = _walletContext.BalanceChargeDictionary
                .FirstOrDefault(x => x.Value == amount).Key;
        }

        [Given(@"revert transaction with (.*)")]
        [When(@"revert transaction with (.*)")]
        public async Task GivenRevertTransactionWith(decimal revertAmount)
        {
            await RevertTransaction(revertAmount, _walletContext.BalanceChargeDictionary);
        }

        [When(@"revert some transaction with (.*)")]
        public async Task GivenRevertTransactionWith(string revertAmount)
        {
            var revertCharges = revertAmount.Split(",").Select(decimal.Parse).ToArray(); ;
            foreach (var revertCharge in revertCharges)
            {
                await RevertTransaction(revertCharge, _walletContext.BalanceChargeDictionary);
            }
        }
        [Given(@"revert of revert transaction with (.*)")]
        [When(@"revert of revert transaction with (.*)")]
        public async Task GivenRevertOfRevertTransactionWith(decimal revertAmount)
        {
            await RevertTransaction(revertAmount, _walletContext.RevertTransactionDictionary);
        }

        private async Task RevertTransaction(decimal revertAmount, ConcurrentDictionary<Guid,decimal> input)
        {
            _walletContext.TransactionId = input.FirstOrDefault(x => x.Value == revertAmount).Key;
            _walletContext.RevertTransactionResponse = await _walletProvider
                .RevertExistTransaction(_walletContext.TransactionId);
            _walletContext.BalanceChargeDictionary
                .TryRemove(new KeyValuePair<Guid, decimal>(_walletContext.TransactionId, revertAmount));
            _walletContext.RevertTransactionDictionary
                .TryAdd(_walletContext.RevertTransactionResponse.Body, revertAmount);
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