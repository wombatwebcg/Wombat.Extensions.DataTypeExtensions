using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wombat.Extensions.DataTypeExtensions
{
    public static partial class DataTypeExtensions
    {
        public static DataTypeEnums ToDataTypeEnum(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type == typeof(bool) ? DataTypeEnums.Bool :
                   type == typeof(byte) ? DataTypeEnums.Byte :
                   type == typeof(short) ? DataTypeEnums.Int16 :
                   type == typeof(ushort) ? DataTypeEnums.UInt16 :
                   type == typeof(int) ? DataTypeEnums.Int32 :
                   type == typeof(uint) ? DataTypeEnums.UInt32 :
                   type == typeof(long) ? DataTypeEnums.Int64 :
                   type == typeof(ulong) ? DataTypeEnums.UInt64 :
                   type == typeof(float) ? DataTypeEnums.Float :
                   type == typeof(double) ? DataTypeEnums.Double :
                   type == typeof(string) ? DataTypeEnums.String :
                   DataTypeEnums.None;
        }

        public static Type ToStructType(this DataTypeEnums dataType)
        {
            switch (dataType)
            {
                case DataTypeEnums.Bool: return typeof(bool);
                case DataTypeEnums.Byte: return typeof(byte);
                case DataTypeEnums.Int16: return typeof(short);
                case DataTypeEnums.UInt16: return typeof(ushort);
                case DataTypeEnums.Int32: return typeof(int);
                case DataTypeEnums.UInt32: return typeof(uint);
                case DataTypeEnums.Int64: return typeof(long);
                case DataTypeEnums.UInt64: return typeof(ulong);
                case DataTypeEnums.Float: return typeof(float);
                case DataTypeEnums.Double: return typeof(double);
                case DataTypeEnums.String: return typeof(string);
                default: throw new ArgumentException("Invalid DataTypeEnum value.", nameof(dataType));
            }
        }

        public static T ConvertFromString<T>(this string value, DataTypeEnums dataType) where T : struct
        {
            try
            {
                switch (dataType)
                {
                    case DataTypeEnums.Bool:
                        if (value == "0") return (T)(object)false;
                        if (value == "1") return (T)(object)true;
                        return (T)(object)bool.Parse(value);
                    case DataTypeEnums.Byte: return (T)(object)byte.Parse(value);
                    case DataTypeEnums.Int16: return (T)(object)short.Parse(value);
                    case DataTypeEnums.UInt16: return (T)(object)ushort.Parse(value);
                    case DataTypeEnums.Int32: return (T)(object)int.Parse(value);
                    case DataTypeEnums.UInt32: return (T)(object)uint.Parse(value);
                    case DataTypeEnums.Int64: return (T)(object)long.Parse(value);
                    case DataTypeEnums.UInt64: return (T)(object)ulong.Parse(value);
                    case DataTypeEnums.Float: return (T)(object)float.Parse(value);
                    case DataTypeEnums.Double: return (T)(object)double.Parse(value);
                    default:
                        throw new InvalidOperationException($"Unsupported DataTypeEnum {dataType} for conversion.");
                }
            }
            catch (FormatException)
            {
                throw new FormatException($"The value '{value}' is not in the correct format for type {typeof(T).Name}.");
            }
            catch (OverflowException)
            {
                throw new OverflowException($"The value '{value}' is out of range for type {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting value '{value}' to type {typeof(T).Name}: {ex.Message}");
            }
        }


        public static object ConvertFromStringToObject(this string value, DataTypeEnums dataType)
        {
            try
            {
                switch (dataType)
                {
                    case DataTypeEnums.Bool: return value == "1" || bool.Parse(value);
                    case DataTypeEnums.Byte: return byte.Parse(value);
                    case DataTypeEnums.Int16: return short.Parse(value);
                    case DataTypeEnums.UInt16: return ushort.Parse(value);
                    case DataTypeEnums.Int32: return int.Parse(value);
                    case DataTypeEnums.UInt32: return uint.Parse(value);
                    case DataTypeEnums.Int64: return long.Parse(value);
                    case DataTypeEnums.UInt64: return ulong.Parse(value);
                    case DataTypeEnums.Float: return float.Parse(value);
                    case DataTypeEnums.Double: return double.Parse(value);
                    case DataTypeEnums.String: return value;
                    default:
                        throw new InvalidOperationException($"Unsupported DataTypeEnum {dataType} for conversion.");
                }
            }
            
            catch (FormatException)
            {
                throw new FormatException($"The value '{value}' is not in the correct format for type {dataType}.");
            }
            catch (OverflowException)
            {
                throw new OverflowException($"The value '{value}' is out of range for type {dataType}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting value '{value}' to type {dataType}: {ex.Message}");
            }
        }
    }

}
