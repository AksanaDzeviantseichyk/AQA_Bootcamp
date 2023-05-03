using NUnit.Framework;
using Queries;
using System.Collections.Generic;

namespace Tests
{
    public class QueriesTests
    {
        [TestCase(new[] { "1qwerty", "cqwertyc", "cqwe", "c" }, "1qwertycqwertyccqwec")]
        [TestCase(new[] { "", "c" }, "c")]
        public void Test2(string[] collection, string expected)
        {
            var result = QueriesStore.Query2(collection);
            Assert.AreEqual(expected, result);
        }

        [TestCase(8, new[] { "1qwerty", "2qwerty", "7qwe" }, "Not found")]
        [TestCase(7, new[] { "1qwerty", "2qwerty", "7qwe" }, "1qwerty")]
        [TestCase(7, new[] { "1qwert", "2qwerty", "7qwe" }, "2qwerty")]
        [TestCase(7, new[] { "1qwert", "tqwerty", "7qwe" }, "Not found")]
        public void Test3(int l, string[] collection, string expected)
        {
            var result = QueriesStore.Query3(l, collection);
            Assert.AreEqual(expected, result);
        }

        [TestCase(new[] { "1qwerty", "cqwertyc", "cqwe", "c" }, 'c', 1)]
        [TestCase(new[] { "1qwerty", "cqwertyc", "fqwc", "c" }, 'c', 1)]
        [TestCase(new[] { "cqwertc", "cqwertyc", "fqwc" }, 'c', 2)]
        [TestCase(new[] { "1qwerty", "9qwertyc", "gcqwe", "c" }, 'c', 0)]
        public void Test4(string[] collection, char startSymbol, int expected)
        {
            var result = QueriesStore.Query4(startSymbol, collection);
            Assert.AreEqual(expected, result);
        }

        [TestCase(new[] { "1qwerty", "cqwertyc", "cqwe", "c" }, 20)]
        [TestCase(new[] { "", "", "" }, 0)]
        [TestCase(new[] { "", "1", "" }, 1)]
        public void Test5(string[] collection, int expected)
        {
            var result = QueriesStore.Query5(collection);
            Assert.AreEqual(expected, result);
        }

        [TestCase(new[] { "1qwerty", "cqwertyc", "cqwe", "c" }, "1ccc")]
        [TestCase(new[] { "1", "2", "3", "4" }, "1234")]
        [TestCase(new[] { "", "2", "3", "4" }, "234")]
        public void Test6(string[] collection, string expected)
        {
            var result = QueriesStore.Query6(collection);
            Assert.AreEqual(expected, result);
        }

        [TestCase(8, new[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90 }, new[] { 6, 4, 88, 12 })]
        [TestCase(0, new[] { 12, 88, 1, 3 }, new int[0])]
        [TestCase(1, new[] { 12, 88, 1, 3 }, new[] { 12 })]
        public void Test7(int k, int[] collection, int[] expected)
        {
            var result = QueriesStore.Query7(k, collection);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(5, new[] { "1C22", "6CF", "GC454545", "005E3", "005EE" }, new[] { "005E3" })]
        [TestCase(5, new[] { "1C224", "6CF", "GC454545", "005E3", "005EE" }, new[] { "005E3", "1C224" })]
        [TestCase(5, new[] { "1C22", "6CF", "GC454545", "005EY", "005EE" }, new string[0])]
        public void Test8(int k, string[] collection, string[] expected)
        {
            var result = QueriesStore.Query8(k, collection);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(20, 8, new[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90, 5 }, new int[] { 90, 12, 9, 8, 6, 5, 2, 0 })]
        [TestCase(87, 8, new[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90, 5 }, new int[] { 90, 12, 9, 8, 6, 5, 2, 0 })]
        [TestCase(88, 8, new[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90, 5 }, new int[] { 90, 88, 12, 9, 8, 6, 5, 4, 3, 2, 1, 0 })]
        [TestCase(20, 15, new[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90, 5 }, new int[] { 12, 5 })]
        [TestCase(10, 3, new[] { 12, 88, 1 }, new int[] { 1 })]
        [TestCase(20, 4, new[] { 12, 88, 1 }, new int[] { 12 })]
        [TestCase(10, 4, new[] { 12, 88, 1 }, new int[0])]
        public void Test9(int d, int k, int[] a, int[] expected)
        {
            var result = QueriesStore.Query9(d, k, a);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(new[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90 }, new[] { "1", "3", "5", "5", "9" })]
        [TestCase(new[] { 1 }, new[] { "1" })]
        [TestCase(new[] { 2 }, new string[0])]
        [TestCase(new int[0], new string[0])]
        public void Test10(int[] collection, string[] expected)
        {
            var result = QueriesStore.Query10(collection);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(new[] { "1C22", "6CF", "GC454545", "L05E3", "005EE" }, new[] { 'L', '6', '5', '2', '0' })]
        [TestCase(new[] { "1C22", "L05E", "005E" }, new[] { 'E', 'E', '2' })]
        [TestCase(new[] { "1C2", "L05", "005" }, new[] { 'L', '1', '0' })]
        [TestCase(new string[0], new char[0])]
        public void Test11(string[] collection, char[] expected)
        {
            var result = QueriesStore.Query11(collection);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(5, 8, new int[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90 }, new int[] { 12, 88, 1, 3, 5, 4, 6, 6, 2, 5, 8, 9, 0, 90 }, new[] { 0, 1, 2, 3, 4, 5, 5, 6, 6, 6, 6, 8, 9, 12, 88, 90 })]
        [TestCase(5, 8, new int[] { 12, 88, 1 }, new int[] { 30, 50, 40 }, new[] { 12, 88 })]
        [TestCase(90, 80, new int[] { 12, 88, 1 }, new int[] { 30, 50, 40 }, new[] { 30, 40, 50 })]
        [TestCase(90, 8, new int[] { 12, 88, 1 }, new int[] { 30, 50, 40 }, new int[0])]
        public void Test12(int k1, int k2, int[] a, int[] b, int[] expected)
        {
            var result = QueriesStore.Query12(k1, k2, a, b);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(new int[] { 12, 88, 11, 3, 55, 679, 222, 845, 9245 }, new [] { 123, 888, 551, 443, 69, 222, 780 }, new[] {"12 - 222", "88 - 888", "11 - 551", "3 - 123", "3 - 443", "679 - 69", "222 - 222" })]
        [TestCase(new int[] { 12, 88}, new [] {222, 780 }, new[] {"12 - 222" })]
        [TestCase(new int[] { 12, 88}, new [] {2223, 780 }, new string[0])]
        [TestCase(new int[] { 12, 88, 11, 3, 55, 679, 222, 845, 9245 }, new int[0], new string[0])]
        [TestCase(new int[0], new[] { 123, 888, 551, 443, 69, 222, 780 }, new string[0])]
        [TestCase(new int[0], new int[0], new string[0])]
        public void Test13(int[] a, int[] b, string[] expected)
        {
            var result = QueriesStore.Query13(a, b);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(new[] { "1C22", "6CF", "GC454545", "L05E3", "005EE" }, new[] { "1C22", "6CF", "GC454", "L05E3", "005EE", "1111" }, new[] { "005EE:L05E3", "005EE:GC454", "005EE:005EE", "1C22:1C22", "1C22:1111", "6CF:6CF", "L05E3:L05E3", "L05E3:GC454", "L05E3:005EE" })]
        public void Test14(string[] a, string[] b, string[] expected)
        {
            var result = QueriesStore.Query14(a, b);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(new [] { 12, 88, 11, 3, 55, 679, 222, 845, 9245 }, new[] { "1: 11", "2: 234", "3: 3", "5: 10145", "8: 88", "9: 679" })]
        [TestCase(new [] { 21, 31, 1 }, new[] { "1: 53" })]
        [TestCase(new [] { 0, 0, 0 }, new[] { "0: 0" })]
        [TestCase(new int[0], new string[0])]
        public void Test15(int[] a, string[] expected)
        {
            var result = QueriesStore.Query15(a);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Test16()
        {
            var enrollees = new List<Enrollee>
                    {
                      new Enrollee { SchoolNumber = 7, YearGraduate = 2013 , LastName = "Mykhailov"},
                      new Enrollee { SchoolNumber = 3, YearGraduate = 2012 , LastName = "Ivanov"},
                      new Enrollee { SchoolNumber = 1, YearGraduate = 1999 , LastName = "Petrov"},
                      new Enrollee { SchoolNumber = 2, YearGraduate = 2017 , LastName = "Sidorov"},
                      new Enrollee { SchoolNumber = 66, YearGraduate = 1980 , LastName = "Pyatochkin"},
                      new Enrollee { SchoolNumber = 37, YearGraduate = 2012 , LastName = "Matroskin"},
                      new Enrollee { SchoolNumber = 7, YearGraduate = 2013 , LastName = "Korolev"}
                    };

            var expected = new Dictionary<uint, int>
            {
                [1980] = 1,
                [1999] = 1,
                [2017] = 1,
                [2012] = 2,
                [2013] = 2,
            };

            var result = QueriesStore.Query16(enrollees);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}