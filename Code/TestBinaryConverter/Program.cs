using BinaryConverter;
using Newtonsoft.Json;
using System;

namespace TestBinaryConverter
{
    public enum TestEnum
    {
        Val0,
        Val1,
        Val10 = 10
    }

    public class SubRecord
    {
        public bool Bool0 { get; set; }
        public bool Bool1 { get; set; }
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
                Comment = "Bla!",
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
                Dec1 = 1234567890.987654321m,
                Real32 = 1234567890.987654321f,
                Real64 = 1234567890.987654321,
                //TestEnum = (TestEnum)(17)//.Val1,
                TestEnum = TestEnum.Val1,
            };

            var buf = BinaryConvert.SerializeObject(orig);

            var obj = BinaryConvert.DeserializeObject<Record>(buf);

            var str = JsonConvert.SerializeObject(orig);
        }
    }
}
