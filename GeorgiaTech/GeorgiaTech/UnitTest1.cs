using GTL_Server;
using NUnit.Framework;

namespace GeorgiaTech
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Program testObject = new Program();
            var result = testObject.TestThisMethod();
            Assert.AreEqual("Test string", result);
        }
    }
}