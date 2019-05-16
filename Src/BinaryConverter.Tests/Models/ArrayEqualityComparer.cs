using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Tests.Models
{
    //https://stackoverflow.com/questions/7244699/gethashcode-on-byte-array
    public static class ArrayEqualityComparer<T>
    {
        private static readonly EqualityComparer<T> _elementComparer = EqualityComparer<T>.Default;

        public static bool Equals(T[] first, T[] second)
        {
            if (first == second)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            if (first.Length != second.Length)
            {
                return false;
            }
            for (int i = 0; i < first.Length; i++)
            {
                if (!_elementComparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static int GetHashCode(T[] array)
        {
            unchecked
            {
                if (array == null)
                {
                    return 0;
                }
                int hash = 17;
                foreach (T element in array)
                {
                    hash = hash * 31 + _elementComparer.GetHashCode(element);
                }
                return hash;
            }
        }
    }

}
