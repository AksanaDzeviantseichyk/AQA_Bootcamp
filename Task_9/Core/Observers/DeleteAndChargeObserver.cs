using System.Collections.Concurrent;

namespace Task_9.Core.Observers
{
    public class DeleteAndChargeObserver : IObserver<int>
    {
        private readonly ConcurrentBag<int> _data = new ConcurrentBag<int>();
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
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}
