using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.Models
{
    class PocoWithAllPrimitives
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

        public override bool Equals(object other)
        {
            var bufThis = BinaryConvert.SerializeObject(this);
            var bufOhter = BinaryConvert.SerializeObject(other as PocoWithAllPrimitives);

            return ArrayEqualityComparer<byte>.Equals(bufThis, bufOhter);
        }

        public override int GetHashCode()
        {
            var bufThis = BinaryConvert.SerializeObject(this);
            return ArrayEqualityComparer<byte>.GetHashCode(bufThis);
        }
    }
}
