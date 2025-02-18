using System;
using System.Linq;
using System.Text;

namespace Wombat.Extensions.DataTypeExtensions
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class DataTypeExtensions
    {

        /// <summary>
        /// byte[]转string
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <param name="encoding">指定编码</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, Encoding encoding)
            => encoding.GetString(bytes);

        /// <summary>
        /// 将byte[]转为Base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] bytes)
               => Convert.ToBase64String(bytes);

        /// <summary>
        /// 转为二进制字符串
        /// </summary>
        /// <param name="aByte">字节</param>
        /// <returns></returns>
        public static string ToBinString(this byte aByte)
            => Convert.ToString(aByte, 2).PadLeft(8, '0');

        /// <summary>
        /// 转为二进制字符串
        /// 注:一个字节转为8位二进制
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ToBinString(this byte[] bytes)
            => string.Concat(bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

        /// <summary>
        /// Byte数组转为对应的16进制字符串
        /// </summary>
        /// <param name="bytes">Byte数组</param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();

        /// <summary>
        /// Byte数组转为对应的16进制字符串
        /// </summary>
        /// <param name="aByte">一个Byte</param>
        /// <returns></returns>
        public static string ToHexString(this byte aByte)
            => aByte.ToString("x2");

        /// <summary>
        /// 转为ASCII字符串（一个字节对应一个字符）
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ToASCIIString(this byte[] bytes)
            => Encoding.ASCII.GetString(bytes);

        /// <summary>
        /// 转为ASCII字符串（一个字节对应一个字符）
        /// </summary>
        /// <param name="aByte">字节数组</param>
        /// <returns></returns>
        public static string ToASCIIString(this byte aByte)
            => ((char)aByte).ToString();

        /// <summary>
        /// 获取异或值
        /// 注：每个字节异或
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static byte GetXOR(this byte[] bytes)
            => bytes.Aggregate((byte)0, (current, b) => (byte)(current ^ b));



        #region Get Value From Bytes

        /// <summary>
        /// 从缓存中提取出bool结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">位的索引</param>
        /// <returns>bool对象</returns>
        public static bool ToBool(this byte[] buffer, int index = 0)
            => (buffer[index] & 0x01) == 0x01;


        /// <summary>
        /// 从缓存中提取出bool数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">位的索引</param>
        /// <param name="length">bool长度</param>
        /// <returns>bool数组</returns>
        public static bool[] ToBoolArray(this byte[] buffer, int index, int length, bool reverse = false)
        {
            int byteLength = (int)Math.Ceiling(length / 8.0);
            byte[] bytesArray = new byte[byteLength];
            buffer.AsSpan(index, byteLength).CopyTo(bytesArray);

            if (reverse)
            {
                Array.Reverse(bytesArray);
            }

            return Enumerable.Range(0, length)
                .Select(i => (bytesArray[i / 8] & (1 << (7 - i % 8))) != 0)
                .ToArray();
        }

        /// <summary>
        /// 从缓存中提取byte结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>byte对象</returns>
        public static byte ToByte(this byte[] buffer, int index) => buffer[index];

        /// <summary>
        /// 从缓存中提取byte数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>byte数组对象</returns>
        public static byte[] ToBytes(this byte[] buffer, int index, int length)
            => buffer.AsSpan(index, length).ToArray();


        private static T[] ConvertNumerics<T>(this byte[] buffer, int index, int count, int typeSize, Func<byte[], int, T> converter, bool reverse = false)
        {
            var result = new T[count];
            for (int i = 0; i < count; i++)
            {
                var span = buffer.AsSpan(index + i * typeSize, typeSize);
                if (reverse) span.Reverse();
                result[i] = converter(span.ToArray(), 0);
            }
            return result;
        }
        /// <summary>
        /// 从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public static short ToInt16(this byte[] buffer, int index = 0, bool reverse = false)
       => ConvertNumerics(buffer, index, 1, 2, BitConverter.ToInt16, reverse)[0];

        /// <summary>
        /// 从缓存中提取short数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>short数组对象</returns>
        public static short[] ToInt16(this byte[] buffer, int index, int length, bool reverse = false)
        => ConvertNumerics(buffer, index, length, 2, BitConverter.ToInt16, reverse);


        /// <summary>
        /// 从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public static ushort ToUInt16(this byte[] buffer, int index = 0, bool reverse = false)
        => ConvertNumerics(buffer, index, 1, 2, BitConverter.ToUInt16, reverse)[0];
        /// <summary>
        /// 从缓存中提取ushort数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>ushort数组对象</returns>
        public static ushort[] ToUInt16(this byte[] buffer, int index, int length, bool reverse = false)
        => ConvertNumerics(buffer, index, length, 2, BitConverter.ToUInt16, reverse);



        private static byte[] AdjustEndian(byte[] bytes, EndianFormat format)
        {
            // 基本的合法性检查
            if (bytes.Length % 4 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "Byte array length does not match.");
            }

            // 根据字节序处理不同格式
            switch (format)
            {
                case EndianFormat.ABCD:
                    if (bytes.Length == 4)
                    {
                        return new byte[] { bytes[3], bytes[2], bytes[1], bytes[0] };
                    }
                    else if (bytes.Length == 8)
                    {
                        return new byte[] { bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0]};
                    }

                    break;

                case EndianFormat.BADC:
                    // 交换字节顺序：适用于 4 字节 或 8 字节
                    if (bytes.Length == 4)
                    {
                        return new byte[] { bytes[2], bytes[3], bytes[0], bytes[1] };
                    }
                    else if (bytes.Length == 8)
                    {
                        return new byte[] { bytes[6], bytes[7], bytes[4], bytes[5], bytes[2], bytes[3], bytes[0], bytes[1] };
                    }
                    break;

                case EndianFormat.CDAB:
                    // 交换字节顺序：适用于 4 字节 或 8 字节
                    if (bytes.Length == 4)
                    {
                        return new byte[] { bytes[1], bytes[0], bytes[3], bytes[2] };
                    }
                    else if (bytes.Length == 8)
                    {
                        return new byte[] { bytes[1], bytes[0], bytes[3], bytes[2] ,bytes[5], bytes[4], bytes[7], bytes[6]};
                    }
                    break;

                case EndianFormat.DCBA:
                    // 反转字节数组：适用于 4 字节 或 8 字节
                    if (bytes.Length == 4)
                    {
                        return new byte[] { bytes[0], bytes[1], bytes[2], bytes[3] };
                    }
                    else if (bytes.Length == 8)
                    {
                        return new byte[] { bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7]};
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), "Unsupported Endian format.");
            }

            // 如果没有匹配的格式，抛出异常
            throw new ArgumentOutOfRangeException(nameof(format), "Unsupported format or size.");
        }


        private static T ConvertWithEndian<T>(byte[] bytes, EndianFormat format, Func<byte[], int, T> converter)
        {
            var adjusted = AdjustEndian(bytes, format);
            var result = converter(adjusted, 0);            
            return result;
        }

        /// <summary>
        /// 从缓存中提取int结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>int对象</returns>
        public static int ToInt32(this byte[] buffer, int index = 0, EndianFormat format = EndianFormat.ABCD)
        => ConvertWithEndian(buffer.AsSpan(index, 4).ToArray(), format, BitConverter.ToInt32);


        /// <summary>
        /// 从缓存中提取int数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>int数组对象</returns>
        public static int[] ToInt32(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD)
        {
            if (length % 4 != 0)
            {
                throw new ArgumentException("Length must be a multiple of 4 for ToInt32 conversion.");
            }

            var result = new int[length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                var byteSegment = buffer.AsSpan(index + i * 4, 4).ToArray();
                result[i] = ConvertWithEndian(byteSegment, format, BitConverter.ToInt32);
            }
            return result;
        }



        /// <summary>
        /// 从缓存中提取uint结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>uint对象</returns>
        public static uint ToUInt32(this byte[] buffer, int index = 0, EndianFormat format = EndianFormat.ABCD)
        => ConvertWithEndian(buffer.AsSpan(index, 4).ToArray(), format, BitConverter.ToUInt32);


        /// <summary>
        /// 从缓存中提取uint数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>uint数组对象</returns>
        public static uint[] ToUInt32(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD)
        {
            if (length % 4 != 0)
            {
                throw new ArgumentException("Length must be a multiple of 4 for ToUInt32 conversion.");
            }

            var result = new uint[length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                var byteSegment = buffer.AsSpan(index + i * 4, 4).ToArray();
                result[i] = ConvertWithEndian(byteSegment, format, BitConverter.ToUInt32);
            }
            return result;
        }

        /// <summary>
        /// 从缓存中提取long结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>long对象</returns>
        public static long ToInt64(this byte[] buffer, int index = 0, EndianFormat format = EndianFormat.ABCD)
        => ConvertWithEndian(buffer.AsSpan(index, 8).ToArray(), format, BitConverter.ToInt64);

        /// <summary>
        /// 从缓存中提取long数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>long数组对象</returns>
        public static long[] ToInt64(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD)
        {
            if (length % 8 != 0)
            {
                throw new ArgumentException("Length must be a multiple of 8 for ToInt64 conversion.");
            }

            var result = new long[length / 8];
            for (int i = 0; i < result.Length; i++)
            {
                var byteSegment = buffer.AsSpan(index + i * 8, 8).ToArray();
                result[i] = ConvertWithEndian(byteSegment, format, BitConverter.ToInt64);
            }
            return result;
        }



        /// <summary>
        /// 从缓存中提取ulong结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ulong对象</returns>
        public static ulong ToUInt64(this byte[] buffer, int index, EndianFormat format = EndianFormat.ABCD)
        => ConvertWithEndian(buffer.AsSpan(index, 8).ToArray(), format, BitConverter.ToUInt64);

        /// <summary>
        /// 从缓存中提取ulong数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>ulong数组对象</returns>
        public static ulong[] ToUInt64(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD)
        {
            if (length % 8 != 0)
            {
                throw new ArgumentException("Length must be a multiple of 8 for ToInt64 conversion.");
            }

            var result = new ulong[length / 8];
            for (int i = 0; i < result.Length; i++)
            {
                var byteSegment = buffer.AsSpan(index + i * 8, 8).ToArray();
                result[i] = ConvertWithEndian(byteSegment, format, BitConverter.ToUInt64);
            }
            return result;
        }

        /// <summary>
        /// 从缓存中提取float结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>float对象</returns>
        public static float ToFloat(this byte[] buffer, int index = 0, EndianFormat format = EndianFormat.ABCD)
        => ConvertWithEndian(buffer.AsSpan(index, 4).ToArray(), format, BitConverter.ToSingle);

        /// <summary>
        /// 从缓存中提取float数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>float数组对象</returns>
        public static float[] ToFloat(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD)
        {
            if (length % 4 != 0)
            {
                throw new ArgumentException("Length must be a multiple of 4 for float conversion.");
            }

            var result = new float[length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                var byteSegment = buffer.AsSpan(index + i * 4, 4).ToArray();
                result[i] = ConvertWithEndian(byteSegment, format, BitConverter.ToSingle);
            }
            return result;
        }


        /// <summary>
        /// 从缓存中提取double结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>double对象</returns>
        public static double ToDouble(this byte[] buffer, int index = 0, EndianFormat format = EndianFormat.ABCD)
        => ConvertWithEndian(buffer.AsSpan(index, 8).ToArray(), format, BitConverter.ToDouble);

        /// <summary>
        /// 从缓存中提取double数组结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>double数组对象</returns>
        public static double[] ToDouble(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD)
        {
            if (length % 8 != 0)
            {
                throw new ArgumentException("Length must be a multiple of 8 for double conversion.");
            }

            var result = new double[length / 8];
            for (int i = 0; i < result.Length; i++)
            {
                var byteSegment = buffer.AsSpan(index + i * 8, 8).ToArray();
                result[i] = ConvertWithEndian(byteSegment, format, BitConverter.ToDouble);
            }
            return result;
        }



        /// <summary>
        /// 从缓存中提取string结果，使用指定的编码
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">byte数组长度</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        public static string ToString(this byte[] buffer, int index, int length, Encoding encoding)
        {
            var bytes = buffer.AsSpan(index, length);
            return encoding.GetString(bytes.ToArray());
        }


        #endregion

        #region Get Bytes From Value


        private static byte[] NumericToBytes<T>(T[] values, Func<T, byte[]> converter, EndianFormat format = EndianFormat.ABCD)
        {
            var result = new byte[values.Length * converter(default(T)).Length];
            for (int i = 0; i < values.Length; i++)
            {
                var bytes = converter(values[i]);
                AdjustEndian(bytes, format).CopyTo(result, i * bytes.Length);
            }
            return result;
        }

        public static byte[] NumericToBytes<T>(T[] values, Func<T, byte[]> converter, bool reverse = false)
        {
            var result = new byte[values.Length * converter(default(T)).Length];
            for (int i = 0; i < values.Length; i++)
            {
                var bytes = converter(values[i]);
                if (reverse) Array.Reverse(bytes);
                bytes.CopyTo(result, i * bytes.Length);
            }
            return result;
        }

        /// <summary>
        /// bool变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this bool value)
         => BitConverter.GetBytes(value);

        /// <summary>
        /// bool数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this bool[] values)
         => values.SelectMany((b, i) => BitConverter.GetBytes(b)).ToArray();





        /// <summary>
        /// short变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this short value, bool reverse = false)
            => NumericToBytes(new short[] { value }, BitConverter.GetBytes, reverse);

        /// <summary>
        /// short数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this short[] values, bool reverse = false)
           => NumericToBytes(values, BitConverter.GetBytes, reverse);

        /// <summary>
        /// ushort变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this ushort value, bool reverse = false)
            => NumericToBytes(new ushort[] { value }, BitConverter.GetBytes, reverse);


        /// <summary>
        /// ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this ushort[] values, bool reverse = false)
           => NumericToBytes(values, BitConverter.GetBytes, reverse);



        /// <summary>
        /// int变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this int value, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(new int[1] { value }, BitConverter.GetBytes, format);


        /// <summary>
        /// int数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this int[] values, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(values, BitConverter.GetBytes, format);


        /// <summary>
        /// uint变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this uint value, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(new uint[1] { value }, BitConverter.GetBytes, format);


        /// <summary>
        /// uint数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this uint[] values, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(values, BitConverter.GetBytes, format);


        /// <summary>
        /// long变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this long value, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(new long[1] { value }, BitConverter.GetBytes, format);

        /// <summary>
        /// long数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this long[] values, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(values, BitConverter.GetBytes, format);


        /// <summary>
        /// ulong变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this ulong value, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(new ulong[1] { value }, BitConverter.GetBytes, format);

        /// <summary>
        /// ulong数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this ulong[] values, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(values, BitConverter.GetBytes, format);


        /// <summary>
        /// float变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this float value, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(new float[1] { value }, BitConverter.GetBytes, format);


        /// <summary>
        /// float数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this float[] values, EndianFormat format = EndianFormat.ABCD)
            => NumericToBytes(values, BitConverter.GetBytes, format);


        /// <summary>
        /// double变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this double value, EndianFormat format = EndianFormat.ABCD)
           => NumericToBytes(new double[1] { value }, BitConverter.GetBytes, format);

        /// <summary>
        /// double数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this double[] values, EndianFormat format = EndianFormat.ABCD)
               => NumericToBytes(values, BitConverter.GetBytes, format);


        /// <summary>
        /// 使用指定的编码字符串转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        public static byte[] ToByte(this string values, Encoding encoding, EndianFormat format = EndianFormat.ABCD)
        {
            //if (values == null) return null;
            //byte[] buffer = new byte[values.Length * 4];
            //for (int i = 0; i < values.Length; i++)
            //{
            //    if (reverse)
            //    {
            //        byte[] tmp = encoding.GetBytes(values.Substring(i, 4));
            //        Array.Reverse(tmp);
            //        ByteToDataFormat4(tmp, format: format).CopyTo(buffer, 4 * i);

            //    }
            //    else
            //    {
            //        ByteToDataFormat4(encoding.GetBytes(values.Substring(i, 4)), format: format).CopyTo(buffer, 4 * i);

            //    }
            //}
            //return buffer;
            return encoding.GetBytes(values);
        }


        #endregion





    }
}
