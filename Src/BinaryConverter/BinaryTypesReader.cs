using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryConverter
{
    public class BinaryTypesReader : BinaryReader
    {
        public BinaryTypesReader(Stream input)
            : base(input)
        {

        }

        public BinaryTypesReader(Stream input, Encoding encoding)
            : base(input, encoding)
        {

        }

        public int Read7BitInt()
        {
            return (int)Read7BitLong();
        }


        public decimal ReadCompactDecimal()
        {
            int exponent = Read7BitInt();
            long mantisa = Read7BitLong();
            return mantisa / (decimal)Math.Pow(10, exponent);
        }

        //Note: if we know the digits in advance - we save the reading/writing of the exponent
        public decimal ReadCompactDecimal(int digits)
        {
            return Read7BitLong() / (decimal)Math.Pow(10, digits);
        }

        public long Read7BitLong()
        {
            long result = 0;
            bool isNeg = false;
            long curByte;
            int loopCount = 0;
            bool done = false;
            int curShift = 0;
            int nextShiftDiff = 6;
            byte mask = 0x3F; //first mask: msb for 'has more' msb-1 for sign
            bool isMinValue = false; //Note: long.MinValue has no equivalent positive value
            do
            {
                curByte = ReadByte();
                if (loopCount == 0)
                    isNeg = (curByte & 0x40) > 0;
                if ((curByte & 0x80) == 0)
                    done = true;
                if (loopCount == 9 && (curByte & 0x40) > 0)
                    isMinValue = true;
                curByte &= mask;
                mask = 0x7F;
                result |= (curByte << curShift);
                curShift += nextShiftDiff;
                nextShiftDiff = 7;
                ++loopCount;
            }
            while (!done);

            if (isMinValue)
                return long.MinValue;
            return result * (isNeg ? -1 : 1);
        }

        public ulong Read7BitULong()
        {
            ulong result = 0;
            ulong curByte;
            bool done = false;
            int curShift = 0;
            int nextShiftDiff = 7;
            byte mask = 0x7F; 
            do
            {
                curByte = ReadByte();
                if ((curByte & 0x80) == 0)
                    done = true;
                curByte &= mask;
                result |= (curByte << curShift);
                curShift += nextShiftDiff;
            }
            while (!done);

            return result;
        }

        public DateTime ReadDateTime()
        {
            return new DateTime(ReadInt64());
        }

        //ticksResolution: e.g. TimeSpan.TicksPerSecond or TimeSpan.TicksPerhour
        public DateTime ReadCompactDateTime(long ticksResolution)
        {
            return new DateTime(Read7BitLong() * ticksResolution);
        }

        public TimeSpan ReadTimeSpan()
        {
            return new TimeSpan(Read7BitLong());
        }

        public byte[] ReadBytesArray()
        {
            int count = Read7BitInt();
            return (count > 0 ? ReadBytes(count) : null);
        }
    }
}
