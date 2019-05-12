using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace BinaryConverter
{
    public class SerializationUtils
    {
        public delegate T ReadDataDelegate<T>(BinaryTypesReader br, object objParam);
        public delegate void WriteDataDelegate<T>(BinaryTypesWriter bw, T instance, object objParam);

        ///////////////////////////////////////////////////////////////////////

        public static void SerializeList<T>(BinaryTypesWriter bw, List<T> list, WriteDataDelegate<T> itemWriter, object objParam)
        {
            if (list != null)
            {
                bw.Write7BitInt(list.Count);
                foreach (T item in list)
                {
                    itemWriter(bw, item, objParam);
                }
            }
            else
            {
                bw.Write7BitInt((Int32)0);
            }
        }

        public static void DeserializeList<T>(BinaryTypesReader br, List<T> list, ReadDataDelegate<T> itemReader, object objParam)
        {
            //Clear the list before work starts. In case the list is null it will throw exception
            list.Clear();
            int count = br.Read7BitInt();
            for (int i = 0; i < count; i++)
            {
                T item = itemReader(br, objParam);
                list.Add(item);
            }
        }

        ///////////////////////////////////////////////////////////////////////

        public static void SerializeDataTable(BinaryTypesWriter bw, DataTable table)
        {
            Type[] columnTypes = new Type[table.Columns.Count];

            //write table name
            bw.Write(table.TableName);

            //write columns count
            bw.Write7BitInt(table.Columns.Count);

            for (int i = 0; i < columnTypes.Length; i++)
            {
                //write column name and type
                bw.Write(table.Columns[i].ColumnName);
                bw.Write(table.Columns[i].DataType.FullName);

                columnTypes[i] = table.Columns[i].DataType;
            }

            //write rows count
            bw.Write7BitInt(table.Rows.Count);

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < columnTypes.Length; i++)
                {
                    if (row.IsNull(i))
                    {
                        bw.Write("");
                        continue;
                    }

                    if (columnTypes[i] == typeof(System.String))
                        bw.Write((string)row[i]);
                    else if (columnTypes[i] == typeof(System.Int32))
                        bw.Write7BitInt((int)row[i]);
                    else if (columnTypes[i] == typeof(System.Int64))
                        bw.Write7BitLong((long)row[i]);
                    else if (columnTypes[i] == typeof(System.Decimal))
                        bw.WriteCompactDecimal((decimal)row[i]);
                    else if (columnTypes[i] == typeof(System.DateTime))
                        bw.WriteCompactDateTime((DateTime)row[i], TimeSpan.TicksPerMillisecond * 100);
                    else if (columnTypes[i] == typeof(bool))
                        bw.Write((bool)row[i]);
                }
            }
        }

        public static DataTable DeserializeDataTable(BinaryTypesReader br)
        {
            DataTable table = new DataTable();
            table.TableName = br.ReadString();

            int columnCount = br.Read7BitInt();

            Type[] columnTypes = new Type[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                string columnName = br.ReadString();
                string typeName = br.ReadString();
                Type columnType = Type.GetType(typeName);

                DataColumn col = new DataColumn(columnName, columnType);
                table.Columns.Add(col);

                columnTypes[i] = columnType;
            }

            int rowsCount = br.Read7BitInt();
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                DataRow row = table.NewRow();
                table.Rows.Add(row);

                for (int i = 0; i < columnCount; i++)
                {
                    if (columnTypes[i] == typeof(System.String))
                        row[i] = br.ReadString();
                    else if (columnTypes[i] == typeof(System.Int32))
                        row[i] = br.Read7BitInt();
                    else if (columnTypes[i] == typeof(System.Int64))
                        row[i] = br.Read7BitLong();
                    else if (columnTypes[i] == typeof(System.Decimal))
                        row[i] = br.ReadCompactDecimal();
                    else if (columnTypes[i] == typeof(System.DateTime))
                        row[i] = br.ReadCompactDateTime(TimeSpan.TicksPerMillisecond * 100);
                    else if (columnTypes[i] == typeof(bool))
                        row[i] = br.ReadBoolean();
                }
            }

            return table;
        }

        ///////////////////////////////////////////////////////////////////////

        public static void SerializeDictionary<K, V>(
            BinaryTypesWriter bw,
            IDictionary<K, V> map,
            WriteDataDelegate<K> keyWriter,
            object keyObjParam,
            WriteDataDelegate<V> ValWriter,
            object valObjParam)
        {
            bw.Write7BitInt(map.Count);
            foreach (KeyValuePair<K, V> keyValue in map)
            {
                keyWriter(bw, keyValue.Key, keyObjParam);
                ValWriter(bw, keyValue.Value, valObjParam);
            }
        }

        public static void DeserializeDictionary<K, V>(
            BinaryTypesReader br,
            IDictionary<K, V> map,
            ReadDataDelegate<K> keyReader,
            object keyObjParam,
            ReadDataDelegate<V> valReader,
            object valObjParam)
        {
            int count = br.Read7BitInt();

            for (int i = 0; i < count; i++)
            {
                K key = keyReader(br, keyObjParam);
                V val = valReader(br, valObjParam);
                map.Add(key, val);
            }
        }

        ///////////////////////////////////////////////////////////////////////

        public static string ReadString(BinaryTypesReader br, object objParam)
        {
            return br.ReadString();
        }

        public static void WriteString(BinaryTypesWriter bw, string val, object objParam)
        {
            bw.Write(val);
        }

        ///////////////////////////////////////////////////////////////////////

        public static int ReadInt(BinaryTypesReader br, object objParam)
        {
            return br.Read7BitInt();
        }

        public static void WriteInt(BinaryTypesWriter bw, int val, object objParam)
        {
            bw.Write7BitInt(val);
        }

        ///////////////////////////////////////////////////////////////////////

        // Uses reflection - relatively slow!!!
        public static T ReadEnum<T>(BinaryTypesReader br, object objParam) where T : IConvertible
        {
            long val = br.Read7BitLong();
            return (T)Enum.ToObject(typeof(T), val);
        }

        public static void WriteEnum<T>(BinaryTypesWriter bw, T val, object objParam) where T : IConvertible
        {
            bw.Write7BitLong((long)Convert.ChangeType(val, typeof(long)));
        }

        ///////////////////////////////////////////////////////////////////////

        public static long ReadLong(BinaryTypesReader br, object objParam)
        {
            return br.Read7BitLong();
        }

        public static void WriteLong(BinaryTypesWriter bw, long val, object objParam)
        {
            bw.Write7BitLong(val);
        }

        ///////////////////////////////////////////////////////////////////////

        public static byte[] ReadBytesArray(BinaryTypesReader br, object objParam)
        {
            return br.ReadBytesArray();
        }

        public static void WriteBytesArray(BinaryTypesWriter bw, byte[] val, object objParam)
        {
            bw.WriteBytesArray(val);
        }

        ///////////////////////////////////////////////////////////////////////
        // Can be used by Disctionary<X, List<long>>
        public static List<long> ReadLongsList(BinaryTypesReader br, object objParam)
        {
            List<long> result = new List<long>();
            SerializationUtils.DeserializeList(br, result, SerializationUtils.ReadLong, null);
            return result;
        }

        public static void WriteLongsList(BinaryTypesWriter bw, List<long> val, object objParam)
        {
            SerializationUtils.SerializeList(bw, val, SerializationUtils.WriteLong, null);
        }

        ///////////////////////////////////////////////////////////////////////

        public static decimal ReadCompactDecimal(BinaryTypesReader br, object objParam)
        {
            return br.ReadCompactDecimal();
        }

        public static void WriteCompactDecimal(BinaryTypesWriter bw, decimal val, object objParam)
        {
            bw.WriteCompactDecimal(val);
        }

        ///////////////////////////////////////////////////////////////////////

        public static bool ReadBool(BinaryTypesReader br, object objParam)
        {
            return br.ReadBoolean();
        }

        public static void WriteBool(BinaryTypesWriter bw, bool val, object objParam)
        {
            bw.Write(val);
        }

        ///////////////////////////////////////////////////////////////////////

        public static ulong BooleanList2ULong(List<bool> boolsList)
        {
            if (boolsList.Count > 64)
            {
                throw new Exception("SerializationUtils.BooleanList2Long() Error: max list count is 64");
            }
            ulong encodedBools = 0;
            int index = 0;
            foreach (bool b in boolsList)
            {
                ulong bit = (ulong)(b ? 1 : 0);
                bit = bit << index;
                encodedBools = encodedBools | bit;
                index++;
            }

            return encodedBools;
        }

        public static List<bool> ULong2BooleanList(ulong encodedBools, int numOfWantedBooleans)
        {
            if (numOfWantedBooleans > 64)
            {
                throw new Exception("SerializationUtils.ULong2BooleanList() Error: max booleans to extract is 64");
            }
            List<bool> res = new List<bool>(numOfWantedBooleans);
            for (int index = 0; index < numOfWantedBooleans; index++)
            {
                ulong bit = 1;
                bit = bit << index;
                res.Add((encodedBools & bit) > 0);
            }

            return res;
        }
    }
}
