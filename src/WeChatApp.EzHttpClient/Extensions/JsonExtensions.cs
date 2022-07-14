using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatApp.EzHttpClient.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj) where T:class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T? ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
