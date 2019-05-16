using BinaryConverter.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.TestCollections
{
    [TestClass]
    public class GenericListTests
    {
        [TestMethod]
        public void Test_ListNull()
        {
            List<string> val = null;

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<List<string>>(buf);

            Assert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_ListEmpty()
        {
            var val = new List<string>();

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<List<string>>(buf);

            CollectionAssert.AreEqual(val, cloned);
        }

        [TestMethod]
        public void Test_ListOfInt()
        {
            var val = new List<int>() {
                1 ,
                2 ,
                3
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<List<int>>(buf);

            CollectionAssert.AreEqual(val, cloned);

        }

        [TestMethod]
        public void Test_ListOfString()
        {
            var val = new List<string>() {
                "1" ,
                "2" ,
                "3" ,
                null ,
                string.Empty
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<List<string>>(buf);

            CollectionAssert.AreEqual(val, cloned);

        }

        [TestMethod]
        public void Test_ListOfStruct()
        {
            var val = new List<DateTime>() {
              new DateTime(2000, 1, 1) ,
              new DateTime(2002, 2, 2) ,
              new DateTime(2003, 3, 3) ,
              default(DateTime)
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<List<DateTime>>(buf);

            CollectionAssert.AreEqual(val, cloned);

        }

        [TestMethod]
        public void Test_ListOfObjects()
        {
            var val = new List<PocoSimple>() {
                new PocoSimple() {Int = 1, Str = "1" } ,
                new PocoSimple() {Int = 2, Str = "2" } ,
                new PocoSimple() {Int = 3, Str = "3" } ,
                 null
            };

            var buf = BinaryConvert.SerializeObject(val);
            var cloned = BinaryConvert.DeserializeObject<List<PocoSimple>>(buf);

            CollectionAssert.AreEqual(val, cloned);


        }
    }
}
