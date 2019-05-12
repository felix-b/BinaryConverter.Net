using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryConverter
{
    public class BinaryTypesWriter : BinaryWriter
    {
        public BinaryTypesWriter(Stream output)
            : base(output)
        {

        }

        public BinaryTypesWriter(Stream output, Encoding encoding)
            : base(output, encoding)
        {

        }

        public void Write7BitInt(int value)
        {
            Write7BitLong(value);
        }

        public void WriteCompactDecimal(decimal val)
        {
            //val = Decimal.Round(val, 5);
            string asString = val.ToString();
            // TODO: -=-= Check this on a French localized computer
            string[] split = asString.Split(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);

            int exponent = 0;
            if (split.Length > 1)
            {
                exponent = split[1].Length;
            }
            else
            {
                int index = split[0].Length - 1;
                while (index > 0)
                {
                    if (split[0][index] == '0')
                    {
                        exponent--;
                    }
                    else
                    {
                        break;
                    }
                    index--;
                }
            }
            if (exponent > 6) exponent = 6;//no more than 6 digits
            long mantisa = (long)((val * (decimal)Math.Pow(10, exponent))); //InstrumentPrice.ConvertToIntegerPrice(, );
            Write7BitInt(exponent);
            Write7BitLong(mantisa);
        }

        //Note: if we know the digits in advance - we save the reading/writing of the exponent
        public void WriteCompactDecimal(decimal value, int digits)
        {
            int sign = value > 0 ? 1 : -1;
            Write7BitLong((long)((value * (decimal)Math.Pow(10, digits)) + (sign * 0.5M)));
        }




        public new void Write(string value)
        {
            base.Write(value == null ? "" : value);
        }

        public void Write7BitLong(long value)
        {
            bool isNeg = value < 0;
            bool isMinValue = false;
            if (value == long.MinValue)
            {
                isMinValue = true;
                value += 1; //Note: long.MinValue has no equivalent positive value
            }
            value = Math.Abs(value);
            byte lowestByte;
            byte mask = 0x3F; //first mask: msb for 'has more' msb-1 for sign
            int shift = 6;
            int loopCount = 0;
            do
            {
                lowestByte = (byte)(value & mask);
                value >>= shift;
                if (value != 0)
                    lowestByte |= 0x80;
                if (shift == 6 && isNeg) //shift == 6 only in first byte
                    lowestByte |= 0x40;
                if (loopCount == 9 && isMinValue)
                    lowestByte |= 0x40;
                mask = 0x7F;
                Write(lowestByte);
                shift = 7;
                loopCount++;
            }
            while (value != 0);
        }

        public void Write7BitULong(ulong value)
        {
            byte lowestByte;
            byte mask = 0x7F; //
            int shift = 7;
            do
            {
                lowestByte = (byte)(value & mask);
                value >>= shift;
                if (value != 0)
                    lowestByte |= 0x80;
                Write(lowestByte);
            }
            while (value != 0);
        }

        public void WriteDateTime(DateTime dateTime)
        {
            Write(dateTime.Ticks);
        }

        //ticksResolution: e.g. TimeSpan.TicksPerSecond or TimeSpan.TicksPerhour
        public void WriteCompactDateTime(DateTime dateTime, long ticksResolution)
        {
            Write7BitLong(dateTime.Ticks / ticksResolution);
        }

        public void WriteTimeSpan(TimeSpan timeSpan)
        {
            Write7BitLong(timeSpan.Ticks);
        }

        public void WriteBytesArray(byte[] array)
        {
            if (array == null)
            {
                Write7BitInt(0);
            }
            else
            {
                Write7BitInt(array.Length);
                Write(array);
            }
        }
    }
}
