using Newtonsoft.Json;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.RequestBody.WeCom;

namespace WeChatApp.Shared.Extensions;

public static class HttpExtensions
{
    /// <summary>
    /// 拼接参数
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="parameter"> </param>
    /// <returns> </returns>
    public static string GetQueryString<T>(this T parameter) where T : IParameterBase
    {
        var props = parameter.GetType().GetProperties();

        var querys = new List<string>();

        foreach (var prop in props)
        {
            querys.Add($"{prop.Name}={prop.GetValue(parameter)}");
        }

        return "?" + string.Join("&", querys);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? ToObject<T>(string json) where T : class
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}