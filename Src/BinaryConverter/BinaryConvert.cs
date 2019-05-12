using System;

namespace BinaryConverter
{
    public static class BinaryConvert
    {
        public static T DeserializeObject<T>(byte[] buf, SerializerSettings settings = null)
        {
            return Serializer.DeserializeObject<T>(buf, settings);
        }

        public static byte[] SerializeObject<T>(T value, SerializerSettings settings = null)
        {
            return Serializer.SerializeObject(value, settings);
        }
    }
}
