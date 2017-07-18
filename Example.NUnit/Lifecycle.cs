using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Example.NUnit
{
    [TestFixture]
    public class Lifecycle : IDisposable
    {
        public Lifecycle()
        {
            // 1
            Debug.WriteLine("Constructor");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // 2
            Debug.WriteLine("OneTimeSetUp");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // 10
            Debug.WriteLine("OneTimeTearDown");
        }

        [SetUp]
        public void TestSetUp()
        {
            // 3, 6
            Debug.WriteLine("SetUp");
        }

        [TearDown]
        public void TestTearDown()
        {
            // 5, 8
            Debug.WriteLine("TearDown");
        }

        [Test]
        public void FirstTest()
        {
            // 4
            Debug.WriteLine("FirstTest");
        }

        [Test]
        public void SecondTest()
        {
            // 7
            Debug.WriteLine("SecondTest");
            Assert.Fail();
        }

        public void Dispose()
        {
            // 9
            Debug.WriteLine("Dispose");
        }
    }
}