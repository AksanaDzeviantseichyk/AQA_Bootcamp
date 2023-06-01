using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task_9.Core.Models.Requests;

namespace Task_9.Core.Utils
{
    public class BalanceChargeGenerator
    {
        private Random random = new Random();
        private decimal _max = 10000000;

        public BalanceChargeRequest GenerateBalanceCharge(int userId, decimal amount)
        {
            return new BalanceChargeRequest()
            {
                UserId = userId,
                Amount = amount
            };
        }

        public BalanceChargeRequest GenerateBalanceCharge(int userId)
        {
            return new BalanceChargeRequest()
            {
                UserId = userId,
                Amount = (decimal)random.NextDouble() * _max
        };

        //public IEnumerable<BalanceChargeRequest> GetSomeAmountBalanceCharge(int userId)
        //{
        //    ret
        //};
        }
    }
}
