using System;
using System.Collections.Generic;
using System.Text;

namespace Wombat.Extensions.DataTypeExtensions
{
    public static partial class DataTypeExtensions
    {
        public static DataTypeEnum ToDataTypeEnum(this Type type)
        {
            if (type == null || !type.IsValueType || type.IsPrimitive || type == typeof(string))
                throw new ArgumentException("Input type must be a non-null struct or string.", nameof(type));

            if (type == typeof(bool)) return DataTypeEnum.Bool;
            if (type == typeof(byte)) return DataTypeEnum.Byte;
            if (type == typeof(short)) return DataTypeEnum.Int16;
            if (type == typeof(ushort)) return DataTypeEnum.UInt16;
            if (type == typeof(int)) return DataTypeEnum.Int32;
            if (type == typeof(uint)) return DataTypeEnum.UInt32;
            if (type == typeof(long)) return DataTypeEnum.Int64;
            if (type == typeof(ulong)) return DataTypeEnum.UInt64;
            if (type == typeof(float)) return DataTypeEnum.Float;
            if (type == typeof(double)) return DataTypeEnum.Double;
            if (type == typeof(string)) return DataTypeEnum.String;

            return DataTypeEnum.None;
        }

        public static Type ToStructType(this DataTypeEnum dataType)
        {
            switch (dataType)
            {
                case DataTypeEnum.Bool: return typeof(bool);
                case DataTypeEnum.Byte: return typeof(byte);
                case DataTypeEnum.Int16: return typeof(short);
                case DataTypeEnum.UInt16: return typeof(ushort);
                case DataTypeEnum.Int32: return typeof(int);
                case DataTypeEnum.UInt32: return typeof(uint);
                case DataTypeEnum.Int64: return typeof(long);
                case DataTypeEnum.UInt64: return typeof(ulong);
                case DataTypeEnum.Float: return typeof(float);
                case DataTypeEnum.Double: return typeof(double);
                case DataTypeEnum.String: return typeof(string);
                default: throw new ArgumentException("Invalid DataTypeEnum value.", nameof(dataType));
            }
        }

        public static T ToType<T>(this object value, DataTypeEnum dataType) where T : struct
        {
            try
            {
                // 获取目标类型
                Type targetType = ToStructType(dataType);

                // 检查目标类型是否为泛型T
                if (targetType != typeof(T))
                    throw new InvalidOperationException($"Target type {typeof(T).Name} does not match the DataTypeEnum provided.");

                // 转换对象
                return (T)Convert.ChangeType(value, targetType);
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Unable to cast value to type {typeof(T).Name}.");
            }
            catch (FormatException)
            {
                throw new FormatException($"The value provided is not in the correct format for type {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during conversion: {ex.Message}");
            }
        }

        public static T ConvertFromString<T>(this string value, DataTypeEnum dataType) where T : struct
        {
            try
            {
                // 获取目标类型
                Type targetType = ToStructType(dataType);

                // 检查目标类型是否为泛型T
                if (targetType != typeof(T))
                    throw new InvalidOperationException($"Target type {typeof(T).Name} does not match the DataTypeEnum provided.");

                // 将字符串转换为目标类型
                return (T)Convert.ChangeType(value, targetType);
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Unable to cast value to type {typeof(T).Name}.");
            }
            catch (FormatException)
            {
                throw new FormatException($"The value provided is not in the correct format for type {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during conversion: {ex.Message}");
            }
        }

        public static object ConvertFromStringToObject(this string value, DataTypeEnum dataType)
        {
            Type targetType = null;
            try
            {
                // 获取目标类型
                targetType = ToStructType(dataType);

                // 将字符串转换为目标类型
                return Convert.ChangeType(value, targetType);
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Unable to cast value to type {targetType?.Name}.");
            }
            catch (FormatException)
            {
                throw new FormatException($"The value provided is not in the correct format for type {targetType?.Name}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during conversion: {ex.Message}");
            }
        }

        public static DataTypeEnum GetDataTypeEnumFromObject(this object value)
        {
            if (value == null)
            {
                return DataTypeEnum.None;
            }

            Type type = value.GetType();

            if (type == typeof(bool)) return DataTypeEnum.Bool;
            if (type == typeof(byte)) return DataTypeEnum.Byte;
            if (type == typeof(short)) return DataTypeEnum.Int16;
            if (type == typeof(ushort)) return DataTypeEnum.UInt16;
            if (type == typeof(int)) return DataTypeEnum.Int32;
            if (type == typeof(uint)) return DataTypeEnum.UInt32;
            if (type == typeof(long)) return DataTypeEnum.Int64;
            if (type == typeof(ulong)) return DataTypeEnum.UInt64;
            if (type == typeof(float)) return DataTypeEnum.Float;
            if (type == typeof(double)) return DataTypeEnum.Double;
            if (type == typeof(string)) return DataTypeEnum.String;

            return DataTypeEnum.None; // 默认返回None，表示不支持的类型
        }
    }
}
