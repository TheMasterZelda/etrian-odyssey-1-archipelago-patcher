using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey
{
    public static class YggdrasilHelper
    {
        public static void CopyTo(object obj, byte[] data, int offset)
        {
            byte[] bytes = null;
            Type type = obj.GetType();
            if (type == typeof(byte))
                bytes = new byte[1] { unchecked((byte)obj) };
            else if (type == typeof(sbyte))
                bytes = new byte[1] { unchecked((byte)((sbyte)obj)) };
            else
            {
                MethodInfo mi = typeof(BitConverter).GetMethod("GetBytes", new Type[] { type });
                if (mi == null) throw new ArgumentException(string.Format("Cannot get bytes from object of type {0}", type));
                bytes = (mi.Invoke(null, new object[] { obj }) as byte[]);
            }
            Buffer.BlockCopy(bytes, 0, data, offset, bytes.Length);
        }

        public static int Round(int value, int roundTo)
        {
            return ((value + roundTo - 1) / roundTo) * roundTo;
        }

        public static T1 GetByValue<T1, T2>(Dictionary<T1, T2> dict, T2 val)
        {
            if (!dict.ContainsValue(val)) throw new Exception("Value not found");
            return dict.FirstOrDefault(x => x.Value.Equals(val)).Key;
        }

        public static bool CompareElements<T>(T[] array1, T[] array2, IEqualityComparer<T> comparer = null)
        {
            if (ReferenceEquals(array1, array2)) return true;
            if (array1 == null || array2 == null) return false;
            if (array1.Length != array2.Length) return false;

            if (comparer == null) comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < array1.Length; i++)
            {
                if (!comparer.Equals(array1[i], array2[i])) return false;
            }

            return true;
        }
    }
}
