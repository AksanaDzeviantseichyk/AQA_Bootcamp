using System.Collections.Concurrent;
using Task_9.Core.Enum;

namespace Task_9.Core.Observers
{
    public class UserActionObserver : IObserver<UserActionInfo>
    {
        private readonly ConcurrentDictionary<int, bool> _usersToDelete = new ConcurrentDictionary<int,bool>();

        public void OnNext(UserActionInfo info)
        {
            if(info.Action == UserAction.Created)
            {
                _usersToDelete[info.Id] = true;
            }
            else if (info.Action == UserAction.Deleted || info.Action == UserAction.Charged)
            {
                _usersToDelete[info.Id] = false;
            }
            else 
            { 
                throw new InvalidOperationException($"Wrong User Action Type: {info.Action}"); 
            }
        }
        public IEnumerable<int> GetAllUsersToDelete()
        {

            return _usersToDelete
                .Where(i => i.Value)
                .Select(i => i.Key);
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
