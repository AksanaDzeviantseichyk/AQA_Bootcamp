using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9.Tests
{
    public class DeleteAndChargeObserver : IObserver<Int32>
    {
        private readonly ConcurrentBag<Int32> _data = new ConcurrentBag<Int32>();
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Int32 value)
        {
            if (!_data.Contains(value))
            {
                _data.Add(value);
            }
        }

        public IEnumerable<Int32> GetAllUsers()
        {
            return _data.ToArray();
        }
    }
}
