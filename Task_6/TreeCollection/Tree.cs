using System.Collections;

namespace TreeCollection
{
    public class Tree<T> : IEnumerable<T> 
    {

        public Tree(bool isReversed = false) 
        {
            throw new NotImplementedException();
        }

        public void Add(T newElement)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}