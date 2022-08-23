using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using WeChatApp.EzHttpClient;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.GlobalVars;

namespace WeChatApp.AdminClient.Services
{
#pragma warning disable // 可能返回 null 引用。

    public class HttpFunc : IHttpFunc
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _client = null!;

        private string[] _accept;
        private NameValueCollection _headers;
        private MediaTypeHeaderValue _mediaType;
        private string _url;
        private AuthenticationHeaderValue _authenticationHeaderValue;
        private HttpContent _httpContent;

        private string Token = string.Empty;

        public void SetToken(string token)
        {
            Token = token;
        }

        //private readonly ILocalStorageService _localStorage;
        public HttpFunc()
        {
            //Token = SyncLocalStorageService.GetItem<string>(GlobalVars.ClientTokenKey);
            //_localStorage = localStorageService;
        }

        private static readonly string[] DEFAULT_ACCEPT =
        {
            "application/json",
            "text/plain",
            "*/*"
        };

        private static readonly string[] DEFAULT_ACCEPT_ENCODING = { "gzip", "deflate" };

        private static readonly MediaTypeHeaderValue DEFAULT_MEDIATYPE = new MediaTypeHeaderValue("application/json");

        /// <summary>
        /// 启用请求内容gzip压缩 自动使用gzip压缩body并设置Content-Encoding为gzip
        /// </summary>
        public static bool EnableCompress { get; set; } = false;

        /// <summary>
        /// </summary>
        /// <param name="httpClientFactory"> </param>
        public HttpFunc(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _mediaType = DEFAULT_MEDIATYPE;
            _accept = DEFAULT_ACCEPT;
        }

        public IHttpFunc Accept(string[] accept)
        {
            _accept = accept;
            return this;
        }

        public IHttpFunc Authentication(AuthenticationHeaderValue authenticationHeaderValue)
        {
            _authenticationHeaderValue = authenticationHeaderValue;
            return this;
        }

        public IHttpFunc Authentication(string scheme, string parameter)
        {
            _authenticationHeaderValue = new AuthenticationHeaderValue(scheme, parameter);

            return this;
        }

        public IHttpFunc Body(object body)
        {
            return Body(body.ToJson());
        }

        public IHttpFunc Body(string body)
        {
            if (string.IsNullOrEmpty(body))
            {
                return this;
            }

            var sc = new StringContent(body);

            if (EnableCompress)
            {
                _httpContent = new CompressedContent(sc, CompressedContent.CompressionMethod.GZip);
                sc.Headers.ContentEncoding.Add("gzip");
            }
            else
            {
                _httpContent = sc;
            }

            //sc.Headers.ContentLength = sc..Length;
            sc.Headers.ContentType = _mediaType;

            return this;
        }

        public IHttpFunc ContentType(string mediaType, string charSet = "")
        {
            _mediaType = new MediaTypeHeaderValue(mediaType);
            if (!string.IsNullOrEmpty(charSet))
            {
                _mediaType.CharSet = charSet;
            }

            return this;
        }

        public IHttpFunc Create()
        {
            _client = _httpClientFactory.CreateClient(ApiVars.ApiBase);

            _httpContent = null;

            return this;
        }

        public async Task<T> DeleteAsync<T>()
        {
            return await RequestAsync<T>(HttpMethod.Delete);
        }

        public IHttpFunc File(string path, string name, string fileName)
        {
            if (_httpContent == null)
            {
                _httpContent = new MultipartFormDataContent();
            }

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            var bac = new StreamContent(stream);
            ((MultipartFormDataContent)_httpContent).Add(bac, name, fileName);

            return this;
        }

        public IHttpFunc File(IBrowserFile file)
        {
            if (_httpContent == null)
            {
                _httpContent = new MultipartFormDataContent();
            }
            //_httpContent = new MultipartFormDataContent();

            var stream = file.OpenReadStream(maxAllowedSize: 50_000_000L);
            var bac = new StreamContent(stream);
            ((MultipartFormDataContent)_httpContent).Add(bac, "file", file.Name);

            return this;
        }

        public IHttpFunc File(string content, string name)
        {
            if (_httpContent == null)
            {
                _httpContent = new MultipartFormDataContent();
            }

            ((MultipartFormDataContent)_httpContent).Add(new StringContent(content), name);
            return this;
        }

        public IHttpFunc Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (_httpContent == null)
            {
                _httpContent = new FormUrlEncodedContent(nameValueCollection);
            }

            return this;
        }

        protected virtual async Task<HttpResponseMessage> GetOriginHttpResponse(HttpMethod method)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var sw = new Stopwatch();
            try
            {
                using (var requestMessage = new HttpRequestMessage(method, _url))
                {
                    switch (method.Method)
                    {
                        case "PUT":
                        case "POST":
                            if (_httpContent != null)
                            {
                                requestMessage.Content = _httpContent;
                            }
                            break;
                    }

                    foreach (var acc in _accept)
                    {
                        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acc));
                    }

                    foreach (var accenc in DEFAULT_ACCEPT_ENCODING)
                    {
                        requestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(accenc));
                    }

                    if (_authenticationHeaderValue != null)
                    {
                        requestMessage.Headers.Authorization = _authenticationHeaderValue;
                    }

                    if (_headers != null)
                    {
                        foreach (string header in _headers.AllKeys)
                        {
                            requestMessage.Headers.Add(header, _headers.Get(header));
                        }
                    }

                    sw.Start();
                    HttpResponseMessage res = null;
                    res = await _client.SendAsync(requestMessage);
                    httpStatusCode = res.StatusCode;
                    sw.Stop();
                    return res;
                }
            }
            catch (Exception e)
            {
                //Logger.Error(e.Message);
                throw;
            }
            finally
            {
                //Logger.Info($"{method.Method} {_url} {httpStatusCode} {sw.ElapsedMilliseconds}ms");
            }
        }

        private async Task<T> GetData<T>(HttpResponseMessage res)
        {
            var str = await res.Content.ReadAsStringAsync();

            if (IsStringOrDecimalOrPrimitiveType(typeof(T)))
            {
                return (T)Convert.ChangeType(str, typeof(T));
            }

            return JsonConvert.DeserializeObject<T>(str);
        }

        protected async Task<T> RequestAsync<T>(HttpMethod method)
        {
            using (var res = await GetOriginHttpResponse(method))
            {
                return await GetData<T>(res);
            }
        }

        private bool IsStringOrDecimalOrPrimitiveType(Type t)
        {
            var typename = t.Name;
            return t.IsPrimitive || typename == "String" || typename == "Decimal";
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> GetHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponse(HttpMethod.Get);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage PostHttpResponseMessage()
        {
            return GetOriginHttpResponse(HttpMethod.Post).Result;
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> PostHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponse(HttpMethod.Post);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage PutHttpResponseMessage()
        {
            return GetOriginHttpResponse(HttpMethod.Put).Result;
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> PutHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponse(HttpMethod.Put);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage DeleteHttpResponseMessage()
        {
            return GetOriginHttpResponse(HttpMethod.Delete).Result;
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> DeleteHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponse(HttpMethod.Delete);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage GetHttpResponseMessage()
        {
            return GetOriginHttpResponse(HttpMethod.Get).Result;
        }

        public async Task<T> GetAsync<T>()
        {
            return await RequestAsync<T>(HttpMethod.Get);
        }

        public IHttpFunc Header(string key, string value)
        {
            CheckHeaderIsNull();
            _headers.Add(key, value);
            return this;
        }

        private void CheckHeaderIsNull()
        {
            if (_headers == null)
            {
                _headers = new NameValueCollection();
            }
        }

        public IHttpFunc Headers(NameValueCollection headers)
        {
            CheckHeaderIsNull();

            foreach (string key in headers.Keys)
            {
                _headers.Add(key, headers.Get(key));
            }

            return this;
        }

        public async Task<T> PostAsync<T>()
        {
            return await RequestAsync<T>(HttpMethod.Post);
        }

        public async Task<T> PutAsync<T>()
        {
            return await RequestAsync<T>(HttpMethod.Put);
        }

        public IHttpFunc Url(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            _url = url;
            //Token = (_localStorage.GetItemAsStringAsync(GlobalVars.ClientTokenKey)).Result ?? "";
            this.Authentication("Bearer", Token);

            return this;
        }

        public IHttpFunc Query(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection.Any())
            {
                _url = _url + "?" + string.Join("&", nameValueCollection.Select(x => x.Key + "=" + x.Value));
            }

            return this;
        }
    }

#pragma warning restore // 可能返回 null 引用。
}