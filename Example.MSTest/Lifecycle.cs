using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example.MSTest
{
    [TestClass]
    public class Lifecycle : IDisposable
    {
        public Lifecycle()
        {
            // 2, 7
            Debug.WriteLine("Constructor");
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // 1
            Debug.WriteLine("ClassInitialize");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // 12
            Debug.WriteLine("ClassCleanup");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // 3, 8
            Debug.WriteLine("TestInitialize");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // 5, 10
            Debug.WriteLine("TestCleanup");
        }

        [TestMethod]
        public void FirstTest()
        {
            // 4 or 9
            Debug.WriteLine("FirstTest passes!");
        }

        [TestMethod]
        public void SecondTest()
        {
            // 4 or 9
            Debug.WriteLine("SecondTest fails!");
            Assert.Fail();
        }

        public void Dispose()
        {
            // 6, 11
            Debug.WriteLine("Dispose");
        }
    }
}