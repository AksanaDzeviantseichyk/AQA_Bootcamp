using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TreeCollection.TestModels.Enums;
using TreeCollection.TestModels.Models;

namespace TreeCollection.Tests
{
    public class Tests
    {
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, false)] // pathological tree where each parent node has only one associated child node, not reversed
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, true)] // pathological tree where each parent node has only one associated child node, reversed
        [TestCase(new int[] { 3, 4, 2, 1, 6, 7, 5, 9, 15, 12, 14, 13, 10, 0, -7, -1, -9, -2, -3, -8 }, false)] // random tree with negative and positive numbers, not reversed
        [TestCase(new int[] { 3, 4, 2, 1, 6, 7, 5, 9, 15, 12, 14, 13, 10, 0, -7, -1, -9, -2, -3, -8 }, true)] // random tree with negative and positive numbers, reversed
        [TestCase(new int[] { 0, 10, -20, 5, 3, 7, 15, 17, 13, -10, -30, -28, -32, -8, -12 }, false)] // perfect binary tree in which all interior nodes have two children and all leaves have the same depth or same level, not reversed
        [TestCase(new int[] { 0, 10, -20, 5, 3, 7, 15, 17, 13, -10, -30, -28, -32, -8, -12 }, true)] // perfect binary tree in which all interior nodes have two children and all leaves have the same depth or same level, reversed
        public void AddDataToIntegerTree_GetAllElements_ResultContainsAddedElementsAndIsSorted(int[] source, bool isReversed)
        {
            // Precondition

            var tree = new Tree<int>(isReversed);

            foreach (var item in source)
            {
                tree.Add(item);
            }

            // Action-Assert

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEquivalent(source, tree);
                CollectionAssert.IsOrdered(isReversed ? tree.Reverse() : tree);
            });
        }

        [Test]
        public void CreateTreeWithoutOrderParamater_AddElements_ElementsAreSortedByAscending()
        {
            // Precondition

            var tree = new Tree<int>();

            // Action

            foreach (var item in new[] { 3, 4, 2, 1, 6, 7, 5, 9, 15, 12, 14, 13, 10, 0, -7, -1, -9, -2, -3, -8 })
            {
                tree.Add(item);
            }

            // Assert

            CollectionAssert.IsOrdered(tree);
        }

        [TestCase(new string[] { "3", "4", "2", "1", "6", "7", "5", "9", "15", "12", "14", "13", "10", "0", "-7", "-1", "-9", "-2", "-3", "-8" }, false)] // random tree with negative and positive numbers, not reversed
        public void AddDataToStringTree_GetAllElements_ResultContainsAddedElementsAndIsSorted(string[] source, bool isReversed)
        {
            // Precondition

            var tree = new Tree<string>(isReversed);

            foreach (var item in source)
            {
                tree.Add(item);
            }

            // Action-Assert

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEquivalent(source, tree);
                CollectionAssert.IsOrdered(isReversed ? tree.Reverse() : tree);
            });
        }

        [Test]
        public void CreateTree_NotAddElement_ElementIsEmpty()
        {
            // Precondition
            var tree = new Tree<char>();
            // Assert
            CollectionAssert.IsEmpty(tree);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void AddDataToExamResultsTree_GetAllElements_ResultContainsAddedElementsAndIsSorted(bool isReversed)
        {
            // Precondition

            var tree = new Tree<ExamResult>(isReversed);

            var source = new[]
            {
                new ExamResult(2, "John", Exams.Sharp, Score.A, DateTime.Parse("2023-06-14T13:45Z")),
                new ExamResult(4, "Frank", Exams.Sharp, Score.B, DateTime.Parse("2023-06-15T13:45Z")),
                new ExamResult(1, "John", Exams.English, Score.C, DateTime.Parse("2023-06-15T13:45Z")),
                new ExamResult(5, "John", Exams.Testing, Score.D, DateTime.Parse("2023-06-15T13:45Z")),
                new ExamResult(3, "Serhii", Exams.Sharp, Score.F, DateTime.Parse("2023-06-15T13:45Z"))
            };

            foreach (var item in source)
            {
                tree.Add(item);
            }

            // Action-Assert

            var expected = source.OrderBy(i => i.Name)
                .ThenBy(i => i.Date)
                .ThenBy(i => i.Id)
                .ToArray();

            if (isReversed)
                expected = expected
                    .Reverse()
                    .ToArray();

            CollectionAssert.AreEqual(expected, tree);
        }

        [Test]
        public void CreateTree_TryAddSameElement_Exception()
        {
            // Precondition
            var tree = new Tree<int>();

            tree.Add(1);
            tree.Add(2);

            // Action-Assert

            Assert.Throws<Exception>(() => tree.Add(2));
        }

        [Test]
        public void FillTree_UseItInTwoLoopsInARow_EnumeratorWasReseted()
        {
            // Precondition

            var source = new[] { 3, 4, 2, 1, 6, 7, 5, 9, 15, 12, 14, 13, 10, 0, -7, -1, -9, -2, -3, -8 };
            var tree = new Tree<int>();

            foreach (var item in source)
            {
                tree.Add(item);
            }

            // Action

            var actual = new List<int>();

            foreach (var item in tree)
            {
                actual.Add(item);
            }

            foreach (var item in tree)
            {
                actual.Add(item);
            }

            // Assert

            var expected = source.Concat(source);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FillTree_UseItInInnerLoop_EnumeratorsAreIndependent()
        {
            // Precondition

            var source = new[] { 3, 4, 2 };
            var tree = new Tree<int>();

            foreach (var item in source)
            {
                tree.Add(item);
            }

            // Action

            var actual = new List<int>();

            foreach (var _ in tree)
            {
                foreach (var item in tree)
                {
                    actual.Add(item);
                }
            }

            // Assert

            var expected = source
                .Concat(source)
                .Concat(source);

            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}