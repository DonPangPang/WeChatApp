using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Specialized;
using System.Net.Http.Headers;

namespace WeChatApp.AdminClient.Services
{
    public interface IHttpFunc
    {
        void SetToken(string token);

        Task<T> GetAsync<T>();

        Task<T> PostAsync<T>();

        Task<T> PutAsync<T>();

        Task<T> DeleteAsync<T>();

        IHttpFunc Body(object body);

        IHttpFunc Body(string body);

        IHttpFunc Url(string url);

        IHttpFunc Create();

        IHttpFunc Authentication(AuthenticationHeaderValue authenticationHeaderValue);

        IHttpFunc Authentication(string scheme, string parameter);

        IHttpFunc Accept(string[] accept);

        IHttpFunc ContentType(string mediaType, string charSet = "");

        IHttpFunc File(string path, string name, string fileName);

        IHttpFunc File(string content, string name);

        IHttpFunc Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection);

        IHttpFunc Headers(NameValueCollection headers);

        IHttpFunc Header(string key, string value);

        IHttpFunc Query(IEnumerable<KeyValuePair<string, string>> nameValueCollection);
        IHttpFunc File(IBrowserFile file);
    }

#pragma warning restore CS8603 // 可能返回 null 引用。
}