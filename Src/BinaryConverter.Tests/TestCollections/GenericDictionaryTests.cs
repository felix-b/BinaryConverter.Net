using BinaryConverter.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryConverter.Tests.TestCollections
{
    [TestClass]
    public class GenericDictionaryTests
    {
        [TestMethod]
        public void Test_DictionaryNull()
        {
            Dictionary<string, int> val = null;

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<Dictionary<string, int>>(buf);

            Assert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_DictionaryEmpty()
        {
            var val = new Dictionary<string, int>();

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<Dictionary<string, int>>(buf);

            CollectionAssert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_DictionaryOfInt()
        {
            var val = new Dictionary<string, int>() {
                { "1", 1 },
                { "2", 2 },
                { "3", 3 }
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<Dictionary<string, int>>(buf);

            CollectionAssert.AreEqual(
                val.OrderBy(x => x.Key).ToList(),
                cloned.OrderBy(x => x.Key).ToList());
        }

        [TestMethod]
        public void Test_DictionaryOfString()
        {
            var val = new Dictionary<string, string>() {
                { "1", "1" },
                { "2", "2" },
                { "3", "3" },
                { "4", null },
                { "5", string.Empty }
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<Dictionary<string, string>>(buf);

            CollectionAssert.AreEqual(
                val.OrderBy(x => x.Key).ToList(),
                cloned.OrderBy(x => x.Key).ToList());

        }

        [TestMethod]
        public void Test_DictionaryOfStruct()
        {
            var val = new Dictionary<string, DateTime>() {
                { "1", new DateTime(2000, 1, 1) },
                { "2", new DateTime(2002, 2, 2) },
                { "3", new DateTime(2003, 3, 3) },
                { "4", default(DateTime) }
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<Dictionary<string, DateTime>>(buf);

            CollectionAssert.AreEqual(
                val.OrderBy(x => x.Key).ToList(),
                cloned.OrderBy(x => x.Key).ToList());

        }

        [TestMethod]
        public void Test_DictionaryOfObjects()
        {
            var val = new Dictionary<string, PocoSimple>() {
                { "1", new PocoSimple() {Int = 1, Str = "1" } },
                { "2", new PocoSimple() {Int = 2, Str = "2" } },
                { "3", new PocoSimple() {Int = 3, Str = "3" } },
                { "4",  null}
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<Dictionary<string, PocoSimple>>(buf);

            CollectionAssert.AreEqual(
                val.OrderBy(x => x.Key).ToList(), 
                cloned.OrderBy(x => x.Key).ToList());


        }
    }
}
