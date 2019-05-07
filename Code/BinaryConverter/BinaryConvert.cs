using System;

namespace BinaryConverter
{
    public static class BinaryConvert
    {
        public static T DeserializeObject<T>(byte[] buf)
        {
            return Serializer.DeserializeObject<T>(buf, null);
        }

        public static byte[] SerializeObject<T>(T value)
        {
            return Serializer.SerializeObject(value, null);
        }
    }
}
