using System.Collections;

namespace TreeCollection
{
    public class Tree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private TreeNode<T>? root;
        private readonly bool isReversedReading;
        public Tree(bool isReversedReading = false) 
        {
            root = null;
            this.isReversedReading = isReversedReading;
        }

        public void Add(T newElement)
        {
            if (root == null)
            {
                root = new TreeNode<T>(newElement);
                return;
            }

            AddToNode(newElement, root);
        }

        private void AddToNode(T newElement, TreeNode<T> node)
        {
            int compareResult = newElement.CompareTo(node.Data);

            if (compareResult < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new TreeNode<T>(newElement);
                }
                else
                {
                    AddToNode(newElement, node.Left);
                }
            }
            else if (compareResult > 0)
            {
                if (node.Right == null)
                {
                    node.Right = new TreeNode<T>(newElement);
                }
                else
                {
                    AddToNode(newElement, node.Right);
                }
            }
            else
            {
                throw new ArgumentException("New element already exists in the tree");
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return isReversedReading ? 
                ReverseTraversal(root).GetEnumerator() 
                : InOrderTraversal(root).GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> InOrderTraversal(TreeNode<T>? node)
        {
            if (node != null)
            {
                foreach (T data in InOrderTraversal(node.Left))
                {
                    yield return data;
                }

                yield return node.Data;

                foreach (T data in InOrderTraversal(node.Right))
                {
                    yield return data;
                }
            }
        }

        private IEnumerable<T> ReverseTraversal(TreeNode<T>? node)
        {
            if (node != null)
            {
                foreach (T data in ReverseTraversal(node.Right))
                {
                    yield return data;
                }

                yield return node.Data;

                foreach (T data in ReverseTraversal(node.Left))
                {
                    yield return data;
                }
            }
        }
        
    }
}