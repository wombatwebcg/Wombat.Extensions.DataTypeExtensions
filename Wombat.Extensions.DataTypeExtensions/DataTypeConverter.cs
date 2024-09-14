using System;
using System.Collections.Generic;
using System.Text;

namespace Wombat.Extensions.DataTypeExtensions
{
    public static partial class DataTypeExtensions
    {
        public static DataTypeEnums ToDataTypeEnum(this Type type)
        {
            if (type == null || !type.IsValueType || type.IsPrimitive || type == typeof(string))
                throw new ArgumentException("Input type must be a non-null struct or string.", nameof(type));

            if (type == typeof(bool)) return DataTypeEnums.Bool;
            if (type == typeof(byte)) return DataTypeEnums.Byte;
            if (type == typeof(short)) return DataTypeEnums.Int16;
            if (type == typeof(ushort)) return DataTypeEnums.UInt16;
            if (type == typeof(int)) return DataTypeEnums.Int32;
            if (type == typeof(uint)) return DataTypeEnums.UInt32;
            if (type == typeof(long)) return DataTypeEnums.Int64;
            if (type == typeof(ulong)) return DataTypeEnums.UInt64;
            if (type == typeof(float)) return DataTypeEnums.Float;
            if (type == typeof(double)) return DataTypeEnums.Double;
            if (type == typeof(string)) return DataTypeEnums.String;

            return DataTypeEnums.None;
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

        public static T ToType<T>(this object value, DataTypeEnums dataType) where T : struct
        {
            try
            {
                // 获取目标类型
                Type targetType = dataType.ToStructType();

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
        public static T ToType<T>(this object value) where T : struct
        {
            try
            {
                // 获取目标类型
                Type targetType = typeof(T);

                // 检查目标类型是否为泛型T
                if (targetType != typeof(T))
                    throw new InvalidOperationException($"Target type {typeof(T).Name} does not match the DataTypeEnum provided.");

                // 检查value是否为值类型
                if (value != null && !value.GetType().IsValueType)
                    throw new ArgumentException("Value must be a value type (struct), not a class object.");
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
                throw new Exception($"An error occurred during conversion:{value} {typeof(T).Name} {ex.Message}");
            }
        }

        public static T ConvertFromString<T>(this string value, DataTypeEnums dataType) where T : struct
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

        public static object ConvertFromStringToObject(this string value, DataTypeEnums dataType)
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

        public static DataTypeEnums GetDataTypeEnumFromObject(this object value)
        {
            if (value == null)
            {
                return DataTypeEnums.None;
            }

            Type type = value.GetType();

            if (type == typeof(bool)) return DataTypeEnums.Bool;
            if (type == typeof(byte)) return DataTypeEnums.Byte;
            if (type == typeof(short)) return DataTypeEnums.Int16;
            if (type == typeof(ushort)) return DataTypeEnums.UInt16;
            if (type == typeof(int)) return DataTypeEnums.Int32;
            if (type == typeof(uint)) return DataTypeEnums.UInt32;
            if (type == typeof(long)) return DataTypeEnums.Int64;
            if (type == typeof(ulong)) return DataTypeEnums.UInt64;
            if (type == typeof(float)) return DataTypeEnums.Float;
            if (type == typeof(double)) return DataTypeEnums.Double;
            if (type == typeof(string)) return DataTypeEnums.String;

            return DataTypeEnums.None; // 默认返回None，表示不支持的类型
        }
    }
}
