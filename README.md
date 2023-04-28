#  Task 6 - Collections

Here is a solution with Template for BinaryTree Collection and with unit tests for this class: 
https://github.com/TiAutoBootcamp/TreeCollection

1. Change ExamResult class in ‘TreeCollection.TestModels project’, currently it has fields:
- contains ID field – unique identifier entry
- contains Name field – student’s name
- contains Exam field- subject name
- contains Score field – test result
- contains Date field – test date

The ExamResult class must implement the IComparable interface, and the comparison must be based on the student's name. If the student's name is the same with the other one, than the comparison must be based on the date of the exam. If the student's name and the exam date are the same - on  the ID entry.

2. Implement all not implemented functions in the Tree<T> class (in ‘TreeCollection’ project). It should be generalized collection. The data inside should be stored as a binary tree (https://en.wikipedia.org/wiki/Binary_tree )
- it should be possible to create an initially empty tree with no elements.
- it should be possible when creating a Tree<T> to specify a method for traversing the tree, left-to-right, or right-to-left. Make a constructor with bool parameter isReversedReading. If its value is false - when subtracting the tree, traverse it recursively from left to right, if true - from right to left.
- Tree<T> must have an Add (T newElement) method that adds a new element to the collection.
- new elements should be added to the free branches of the tree. The new element is less than the value in the node - go to the left branch, greater - go to the right branch, equal - throw a ArgumentException.
- Tree<T> must throw exception if we try to add already added value to tree
- implement IEnumerable<T> interface in Tree<T> class

3. Check that all unit tests in ‘TreeCollection.Tests’ assembly passed.