using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WeChatApp.EzHttpClient;
#pragma warning disable
public class HttpClientWrapper
{
    private static readonly HttpClientHandler httpClientHandler = new HttpClientHandler
    {
        AllowAutoRedirect = true,
        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
        MaxAutomaticRedirections = 5,
    };

    private static readonly HttpClient HttpClientInner = new HttpClient(httpClientHandler);

    private static readonly MediaTypeHeaderValue DEFAULT_MEDIATYPE = new MediaTypeHeaderValue("application/json");

    private static readonly string[] DEFAULT_ACCEPT =
    {
            "application/json",
            "text/plain",
            "*/*"
        };

    private static readonly string[] DEFAULT_ACCEPT_ENCODING = { "gzip", "deflate" };

    static HttpClientWrapper()
    {
        try
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 200;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            HttpClientInner.Timeout = TimeSpan.FromSeconds(60);
            _ = HttpClientInner.GetAsync("http://127.0.0.1").Result;
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// 启用请求内容gzip压缩 自动使用gzip压缩body并设置Content-Encoding为gzip
    /// </summary>
    public static bool EnableCompress { get; set; } = false;

    private string _url;
    private HttpContent _httpContent;
    private NameValueCollection _headers;
    private MediaTypeHeaderValue _mediaType;
    private string[] _accept;
    private AuthenticationHeaderValue _authenticationHeaderValue;

    protected HttpClientWrapper()
    {
        _mediaType = DEFAULT_MEDIATYPE;
        _accept = DEFAULT_ACCEPT;
    }

    public static HttpClientWrapper Create()
    {
        return new HttpClientWrapper();
    }

    protected HttpClientWrapper Authentication(AuthenticationHeaderValue authentication)
    {
        _authenticationHeaderValue = authentication;
        return this;
    }

    public HttpClientWrapper Authentication(string scheme, string parameter)
    {
        _authenticationHeaderValue = new AuthenticationHeaderValue(scheme, parameter);
        return this;
    }

    /// <summary>
    /// 默认为 application/json, text/plain, */*
    /// </summary>
    /// <param name="accept"> </param>
    /// <returns> </returns>
    public HttpClientWrapper Accept(string[] accept)
    {
        _accept = accept;
        return this;
    }

    public HttpClientWrapper ContentType(string mediaType, string charSet = null)
    {
        _mediaType = new MediaTypeHeaderValue(mediaType);
        if (!string.IsNullOrEmpty(charSet))
        {
            _mediaType.CharSet = charSet;
        }

        return this;
    }

    /// <summary>
    /// 默认为 gzip, deflate
    /// </summary>
    /// <param name="acceptEncoding"> </param>
    /// <returns> </returns>
    public HttpClientWrapper AcceptEncoding(string[] acceptEncoding)
    {
        //_acceptEncoding = acceptEncoding;
        return this;
    }

    public HttpClientWrapper Url(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        _url = url;
        return this;
    }

    public HttpClientWrapper Body(object body)
    {
        return Body(Newtonsoft.Json.JsonConvert.SerializeObject(body));
    }

    public HttpClientWrapper Body(string body)
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

    /// <summary>
    /// 仅支持POST和PUT
    /// </summary>
    /// <param name="path">     </param>
    /// <param name="name">     </param>
    /// <param name="filename"> </param>
    /// <returns> </returns>
    public HttpClientWrapper File(string path, string name, string filename)
    {
        if (_httpContent == null)
        {
            _httpContent = new MultipartFormDataContent();
        }

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var bac = new StreamContent(stream);
        ((MultipartFormDataContent)_httpContent).Add(bac, name, filename);

        return this;
    }

    /// <summary>
    /// 仅支持POST和PUT
    /// </summary>
    /// <param name="content"> </param>
    /// <param name="name">    </param>
    /// <returns> </returns>
    public HttpClientWrapper File(string content, string name)
    {
        if (_httpContent == null)
        {
            _httpContent = new MultipartFormDataContent();
        }

        ((MultipartFormDataContent)_httpContent).Add(new StringContent(content), name);
        return this;
    }

    public HttpClientWrapper Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
    {
        if (_httpContent == null)
        {
            _httpContent = new FormUrlEncodedContent(nameValueCollection);
        }

        return this;
    }

    public HttpClientWrapper Headers(NameValueCollection headers)
    {
        CheckHeaderIsNull();

        foreach (string key in headers.Keys)
        {
            _headers.Add(key, headers.Get(key));
        }

        return this;
    }

    private void CheckHeaderIsNull()
    {
        if (_headers == null)
        {
            _headers = new NameValueCollection();
        }
    }

    public HttpClientWrapper Header(string key, string value)
    {
        CheckHeaderIsNull();
        _headers.Add(key, value);
        return this;
    }

    public T GetData<T>(HttpResponseMessage res)
    {
        string str = string.Empty;

        Task.Run(async () => str = await res.Content.ReadAsStringAsync()).Wait();

        if (IsStringOrDecimalOrPrimitiveType(typeof(T)))
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }

        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
    }

    protected async Task<T> RequestAsync<T>(HttpMethod method)
    {
        using (var res = await GetOriginHttpResponse(method))
        {
            return GetData<T>(res);
        }
    }

    private bool IsStringOrDecimalOrPrimitiveType(Type t)
    {
        var typename = t.Name;
        return t.IsPrimitive || typename == "String" || typename == "Decimal";
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
                Task.Run(async () => res = await HttpClientInner.SendAsync(requestMessage)).Wait();
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

    /// <summary>
    /// 自动将请求返回值反序列化为 <typeparam name="T"> </typeparam> 类型 若返回响应体无法反序列化则抛出异常
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <returns> </returns>
    public async Task<T> GetAsync<T>()
    {
        return await RequestAsync<T>(HttpMethod.Get);
    }

    [Obsolete("建议使用异步方法")]
    public T Get<T>()
    {
        return GetAsync<T>().Result;
    }

    public Task<T> PostAsync<T>()
    {
        return RequestAsync<T>(HttpMethod.Post);
    }

    [Obsolete("建议使用异步方法")]
    public T Post<T>()
    {
        return PostAsync<T>().Result;
    }

    public Task<T> PutAsync<T>()
    {
        return RequestAsync<T>(HttpMethod.Put);
    }

    [Obsolete("建议使用异步方法")]
    public T Put<T>()
    {
        return PutAsync<T>().Result;
    }

    public Task<T> DeleteAsync<T>()
    {
        return RequestAsync<T>(HttpMethod.Delete);
    }

    [Obsolete("建议使用异步方法")]
    public T Delete<T>()
    {
        return DeleteAsync<T>().Result;
    }

    public override string ToString()
    {
        return $"{_url}";
    }
}
#pragma warning restore