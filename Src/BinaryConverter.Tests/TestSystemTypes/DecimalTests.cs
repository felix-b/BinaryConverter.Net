using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.TestSystemTypes
{
    [TestClass]
    public class DecimalTests
    {
        [TestMethod]
        public void Test_Decimal()
        {
            decimal val = 1234.1m;
            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<decimal>(buf);
            Assert.AreEqual(val, cloned);
        }
    }
}
