using BinaryConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TestBinaryConverter
{
    public enum TestEnum
    {
        Val0,
        Val1,
        Val10 = 10
    }

    public class SubRecordBase
    {
        public bool Bool0 { get; set; }
        public bool Bool1 { get; set; }
    }

    public class SubRecord : SubRecordBase
    {
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Int16 { get; set; }
        public ushort UInt16 { get; set; }
        public int Int32 { get; set; }
        public uint UInt32 { get; set; }
        public long Int64 { get; set; }
        public ulong UInt64 { get; set; }
        public char Char { get; set; }
    }

    //todo: 
    // - nullable
    // - Array
    // - []
    // - HashSet
    public class Record
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }
        public SubRecord SubRecord { get; set; }
        public decimal Dec1 { get; set; }
        public float Real32 { get; set; }
        public double Real64 { get; set; }
        public TestEnum TestEnum { get; set; }

        public List<int> IntList { get; set; }
        public List<SubRecord> SubRecordList { get; set; }
        public Dictionary<int, int> IntDict { get; set; }
        public Dictionary<string, SubRecord> SubRecordDict { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var orig = new Record
            {
                Id = 7,
                Time = DateTime.UtcNow.Date,
                Comment = null,//"Bla!",
                SubRecord = new SubRecord
                {
                    Bool0 = false,
                    Bool1 = true,
                    Byte = 1,
                    SByte = -2,
                    Int16 = -3,
                    UInt16 = 4,
                    Int32 = 5,
                    UInt32 = 6,
                    Int64 = 7,
                    UInt64 = 8,
                    Char = 'a',
                },
                Dec1 = 1234567890.987654m,
                Real32 = 17890.987654321f,
                Real64 = 167890.987654321,
                //TestEnum = (TestEnum)(17)//.Val1,
                TestEnum = TestEnum.Val1,
                IntList = new List<int> { 1, 4, 9, 16 },
                SubRecordList = new List<SubRecord>
                {
                    new SubRecord
                    {
                        Bool0 = false,
                        Bool1 = true,
                        Byte = 11,
                        SByte = -12,
                        Int16 = -13,
                        UInt16 = 14,
                        Int32 = 15,
                        UInt32 = 16,
                        Int64 = 17,
                        UInt64 = 18,
                        Char = 'b',
                    }
                },
                IntDict = new Dictionary<int, int>
                {
                    { 0 , 0 },
                    { 1 , 1 },
                    { 2 , 4 },
                    { 3 , 9 },
                    { 4 , 16 },
                },
                SubRecordDict = new Dictionary<string, SubRecord>
                {
                    { "key1" , new SubRecord{Bool0 = true , Char = (char)11}},
                    { "key2" , new SubRecord{Bool1 = true , Char = 'x'} },
                }
            };

            //orig.SubRecordDict = null;

            var buf = BinaryConvert.SerializeObject(orig);

            var cloned = BinaryConvert.DeserializeObject<Record>(buf);

            var strOrig = JsonConvert.SerializeObject(orig);
            var strCloned = JsonConvert.SerializeObject(cloned);

            Console.WriteLine($"==================================================================");
            Console.WriteLine($"buf len:\r\n{buf.Length}");
            Console.WriteLine($"==================================================================");
            Console.WriteLine($"strOrig:\r\n {strOrig}");
            Console.WriteLine($"==================================================================");
            Console.WriteLine($"strCloned:\r\n {strCloned}");
            Console.WriteLine($"==================================================================");
        }
    }
}
