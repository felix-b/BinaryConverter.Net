using BinaryConverter.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.TestSystemTypes
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void Test_DateTime()
        {
            var settings = new SerializerSettings();
            settings.SerializerArgMap[typeof(DateTime)] = new DateTimeSerializerArg() { TickResolution = 1 };

            var val = DateTime.Now;
            var buf = BinaryConvert.SerializeObject(val, settings);
            var cloned = BinaryConvert.DeserializeObject<DateTime>(buf, settings);
            Assert.AreEqual(val, cloned);
        }
    }
}
