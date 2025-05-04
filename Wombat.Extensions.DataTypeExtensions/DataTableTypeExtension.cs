using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Wombat.Extensions.DataTypeExtensions
{
    public static partial class DataTypeExtensions
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();

            // 确认参数有效, 若无效则返回空列表
            if (dt == null || dt.Rows.Count == 0)
                return list;

            Dictionary<string, FieldInfo> dicField = new Dictionary<string, FieldInfo>();
            Dictionary<string, PropertyInfo> dicProperty = new Dictionary<string, PropertyInfo>();
            Type type = typeof(T);

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                dicField[field.Name.ToLower()] = field;
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                dicProperty[property.Name.ToLower()] = property;
            }

            // 预编译构造函数表达式，替代 Activator.CreateInstance<T>()
            var constructor = Expression.New(typeof(T));
            var compiledConstructor = Expression.Lambda<Func<T>>(constructor).Compile();

            foreach (DataRow row in dt.Rows)
            {
                T instance = compiledConstructor();
                foreach (DataColumn column in dt.Columns)
                {
                    string memberKey = column.ColumnName.ToLower();
                    object dbValue = row[column];

                    if (dbValue is DBNull)
                        dbValue = null;

                    // 赋值到字段
                    if (dicField.TryGetValue(memberKey, out FieldInfo field))
                    {
                        if (dbValue != null)
                        {
                            dbValue = Convert.ChangeType(dbValue, field.FieldType);
                        }
                        field.SetValue(instance, dbValue);
                    }

                    // 赋值到属性
                    if (dicProperty.TryGetValue(memberKey, out PropertyInfo property))
                    {
                        if (dbValue != null)
                        {
                            dbValue = Convert.ChangeType(dbValue, property.PropertyType);
                        }
                        property.SetValue(instance, dbValue);
                    }
                }
                list.Add(instance);
            }
            return list;
        }

        /// <summary>
        ///将DataTable转换为标准的CSV字符串
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>返回标准的CSV</returns>
        public static string ToCsvString(this DataTable dt)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colum = dt.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
