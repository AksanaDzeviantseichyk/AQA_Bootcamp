using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Models.Requests;

namespace Task_9.Utils
{
    public class BalanceChargeGenerator
    {

        public BalanceChargeRequest GenerateBalanceCharge(Int32 userId, decimal amount)
        {
            return new BalanceChargeRequest()
            {
                UserId = userId,
                Amount = amount
            };
        }
    }
}
