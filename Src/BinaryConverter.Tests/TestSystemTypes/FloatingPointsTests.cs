using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.TestSystemTypes
{
    [TestClass]
    public class FloatingPointsTests
    {
        [TestMethod]
        public void Test_Single()
        {
            float val = 1234.1f;
            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<float>(buf);
            Assert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_Double()
        {
            double val = 1234.1f;
            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<double>(buf);
            Assert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_SingleNegative()
        {
            float val = -1234.1f;
            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<float>(buf);
            Assert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_DoubleNegative()
        {
            double val = -1234.1f;
            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<double>(buf);
            Assert.AreEqual(val, cloned);
        }
    }
}
