﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Wombat.Extensions.DataTypeExtensions
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static class JsonTypeExtension
    {
        static JsonTypeExtension()
        {
            JsonConvert.DefaultSettings = () => DefaultJsonSetting;
        }

        public static JsonSerializerSettings DefaultJsonSetting = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss.fff"
        };

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }


        public static string ToLowercaseJson(this object obj)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj,formatting:Formatting.None,settings: serializerSettings);
        }


        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object ToObject(this string jsonStr, Type type)
        {
            return JsonConvert.DeserializeObject(jsonStr, type);
        }
    }
}
