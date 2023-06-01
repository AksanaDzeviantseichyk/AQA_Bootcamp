﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9.Tests
{
    public class DeleteAndChargeObserver : IObserver<int>
    {
        private readonly ConcurrentBag<int> _data = new ConcurrentBag<int>();
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(int value)
        {
            if (!_data.Contains(value))
            {
                _data.Add(value);
            }
        }

        public IEnumerable<int> GetAllUsers()
        {
            return _data.ToArray();
        }
    }
}
