using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace Hishop.WeiBo.Api
{
    // Token: 0x02000004 RID: 4
    public abstract class OpenAuthenticationClientBase
    {
        protected HttpClient http;
        protected bool isAccessTokenSet;

        public OpenAuthenticationClientBase(string clientId, string clientSecret, string callbackUrl, string accessToken = null)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.CallbackUrl = callbackUrl;
            this.AccessToken = accessToken;
            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };
            HttpClient client = new HttpClient(handler)
            {
                BaseAddress = new Uri(this.BaseApiUrl)
            };
            this.http = client;
            if (!string.IsNullOrEmpty(accessToken))
            {
                this.isAccessTokenSet = true;
            }
        }

        public abstract void GetAccessTokenByCode(string code);
        public abstract string GetAuthorizationUrl();
        private string GetNonceString(int length = 8)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                builder.Append((char)random.Next(0x61, 0x7b));
            }
            return builder.ToString();
        }

        public HttpResponseMessage HttpGet(string api, Dictionary<string, object> parameters = null) =>
            this.HttpGetAsync(api, parameters).Result;

        public HttpResponseMessage HttpGet(string api, object parameters) =>
            ((HttpResponseMessage)this.HttpGetAsync(api, (dynamic)parameters).Result);

        public virtual Task<HttpResponseMessage> HttpGetAsync(string api, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            string str = string.Join("&", (IEnumerable<string>)(from p in parameters.Where<KeyValuePair<string, object>>(delegate (KeyValuePair<string, object> p) {
                if (!((p.Value == null) || p.Value.GetType().IsValueType))
                {
                    return p.Value.GetType() == typeof(string);
                }
                return true;
            })
                                                                select $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString($"{p.Value}")}"));
            if (api.IndexOf("?") < 0)
            {
                api = $"{api}?{str}";
            }
            else
            {
                api = $"{api}&{str}";
            }
            api = api.Trim(new char[] { '&', '?' });
            return this.http.GetAsync(api);
        }

        public Task<HttpResponseMessage> HttpGetAsync(string api, object parameters)
        {
            Type type = parameters.GetType();
            if ((!type.Name.Contains("AnonymousType") || !type.IsSealed) || (type.Namespace != null))
            {
                throw new ArgumentException("只支持匿名类型参数");
            }
            Dictionary<string, object> dictionary = type.GetProperties().Where<PropertyInfo>(delegate (PropertyInfo p) {
                if (!p.PropertyType.IsValueType)
                {
                    return (p.PropertyType == typeof(string));
                }
                return true;
            }).ToDictionary<PropertyInfo, string, object>(k => k.Name, v => string.Format("{0}", v.GetValue((dynamic)parameters, (dynamic)null)));
            return this.HttpGetAsync(api, dictionary);
        }

        public HttpResponseMessage HttpPost(string api, Dictionary<string, object> parameters) =>
            this.HttpPostAsync(api, parameters).Result;

        public HttpResponseMessage HttpPost(string api, object parameters) =>
            ((HttpResponseMessage)this.HttpPostAsync(api, (dynamic)parameters).Result);

        public virtual Task<HttpResponseMessage> HttpPostAsync(string api, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            Dictionary<string, object> source = new Dictionary<string, object>(parameters.ToDictionary<KeyValuePair<string, object>, string, object>(k => k.Key, v => v.Value));
            HttpContent content = null;
            if (source.Count<KeyValuePair<string, object>>(delegate (KeyValuePair<string, object> p) {
                if (!(p.Value.GetType() == typeof(byte[])))
                {
                    return (p.Value.GetType() == typeof(FileInfo));
                }
                return true;
            }) > 0)
            {
                MultipartFormDataContent content2 = new MultipartFormDataContent();
                foreach (KeyValuePair<string, object> pair in source)
                {
                    Type type = pair.Value.GetType();
                    if (type == typeof(byte[]))
                    {
                        content2.Add(new ByteArrayContent((byte[])pair.Value), pair.Key, this.GetNonceString(8));
                    }
                    else if (type == typeof(MemoryFileContent))
                    {
                        MemoryFileContent content3 = (MemoryFileContent)pair.Value;
                        content2.Add(new ByteArrayContent(content3.Content), pair.Key, content3.FileName);
                    }
                    else if (type == typeof(FileInfo))
                    {
                        FileInfo info = (FileInfo)pair.Value;
                        content2.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(info.FullName)), pair.Key, info.Name);
                    }
                    else
                    {
                        content2.Add(new StringContent($"{pair.Value}"), pair.Key);
                    }
                }
                content = content2;
            }
            else
            {
                FormUrlEncodedContent content4 = new FormUrlEncodedContent(source.ToDictionary<KeyValuePair<string, object>, string, string>(k => k.Key, v => $"{v.Value}"));
                content = content4;
            }
            return this.http.PostAsync(api, content);
        }

        public Task<HttpResponseMessage> HttpPostAsync(string api, object parameters)
        {
            Type type = parameters.GetType();
            if ((!type.Name.Contains("AnonymousType") || !type.IsSealed) || (type.Namespace != null))
            {
                throw new ArgumentException("只支持匿名类型参数");
            }
            Dictionary<string, object> dictionary = type.GetProperties().ToDictionary<PropertyInfo, string, object>(k => k.Name, v => v.GetValue(parameters, null));
            return this.HttpPostAsync(api, dictionary);
        }

        public string AccessToken { get; set; }

        protected abstract string AccessTokenUrl { get; }

        protected abstract string AuthorizationCodeUrl { get; }

        protected abstract string BaseApiUrl { get; }

        public string CallbackUrl { get; protected set; }

        public string ClientId { get; protected set; }

        public string ClientName { get; protected set; }

        public string ClientSecret { get; protected set; }

        public bool IsAuthorized =>
            (this.isAccessTokenSet && !string.IsNullOrEmpty(this.AccessToken));
    }
}
