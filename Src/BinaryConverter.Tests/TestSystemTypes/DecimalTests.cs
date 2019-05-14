using BinaryConverter.Serializers;
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
            decimal val = 1234.717555m;

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<decimal>(buf);

            Assert.AreEqual(buf.Length, 6);
            Assert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_Decimal2Digits()
        {
            var settings = new SerializerSettings();
            settings.SerializerArgMap[typeof(Decimal)] = new FloatingPointSerializerArg() { DecimalDigits = 2 };
            decimal val = 1234.717555m;

            var buf = BinaryConvert.SerializeObject(val, settings);
            var cloned = BinaryConvert.DeserializeObject<decimal>(buf, settings);

            Assert.AreEqual(buf.Length, 3);
            Assert.AreNotEqual(val, cloned);
            Assert.AreEqual(decimal.Round(val, 2), cloned);
        }

        [TestMethod]
        public void Test_Decimal4Digits()
        {
            var settings = new SerializerSettings();
            settings.SerializerArgMap[typeof(Decimal)] = new FloatingPointSerializerArg() { DecimalDigits = 4 };
            decimal val = 1234.717555m;

            var buf = BinaryConvert.SerializeObject(val, settings);
            var cloned = BinaryConvert.DeserializeObject<decimal>(buf, settings);

            Assert.AreEqual(buf.Length, 4);
            Assert.AreNotEqual(val, cloned);
            Assert.AreEqual(decimal.Round(val, 4), cloned);
        }

        [TestMethod]
        public void Test_Decimal6Digits()
        {
            var settings = new SerializerSettings();
            settings.SerializerArgMap[typeof(Decimal)] = new FloatingPointSerializerArg() { DecimalDigits = 6 };
            decimal val = 1234.717555m;

            var buf = BinaryConvert.SerializeObject(val, settings);
            var cloned = BinaryConvert.DeserializeObject<decimal>(buf, settings);

            Assert.AreEqual(buf.Length, 5);
            Assert.AreEqual(decimal.Round(val, 6), cloned);
        }
    }
}
