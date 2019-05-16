using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.Models
{
    public class PocoSimple 
    {
        public int Int { get; set; }
        public string Str { get; set; }

        public override bool Equals(object other)
        {
            var bufThis = BinaryConvert.SerializeObject(this);
            var bufOhter = BinaryConvert.SerializeObject(other as PocoSimple);

            return ArrayEqualityComparer<byte>.Equals(bufThis, bufOhter);
        }

        public override int GetHashCode()
        {
            var bufThis = BinaryConvert.SerializeObject(this);
            return ArrayEqualityComparer<byte>.GetHashCode(bufThis);
        }
    }
}
