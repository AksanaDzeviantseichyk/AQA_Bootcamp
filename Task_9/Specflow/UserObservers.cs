using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Observers;

namespace Task_9.Specflow
{
    public class UserObservers
    {
        public ConcurrentBag<RegisterUserObserver> RegisterUserObservers { get; }
        public ConcurrentBag<DeleteAndChargeObserver> DeleteUserObservers { get; }

        public UserObservers(
            ConcurrentBag<RegisterUserObserver> registerUserObservers,
            ConcurrentBag<DeleteAndChargeObserver> deleteUserObservers)
        {
            RegisterUserObservers = registerUserObservers;
            DeleteUserObservers = deleteUserObservers;
        }
       public RegisterUserObserver GetRegisterUserObserver()
        {
            return RegisterUserObservers.FirstOrDefault();
        }
        public DeleteAndChargeObserver GetDeleteAndChargeObserver()
        {
            return DeleteUserObservers.FirstOrDefault();
        }
    }
    
}
