using System.IO;
using System.Net.Http;
using Newtonsoft.Json;


namespace Go.Job.Web.Helper
{
    /// <summary>
    /// 需要安装 nuget Microsoft.AspNet.WebApi.Client
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// get请求返回model
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T GetModel<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string res = client.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<T>(res);
            }
        }

        /// <summary>
        /// get请求返回string
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetString(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(url).Result;
            }
        }

        /// <summary>
        /// get请求返回字节
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public byte[] GetBytes(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new byte[0];
            }
            url = url.Replace("\\", "/");
            using (HttpClient client = new HttpClient())
            {
                return client.GetByteArrayAsync(url).Result;
            }
        }


        /// <summary>
        /// get请求返回流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetStream(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }
            using (HttpClient client = new HttpClient())
            {
                return client.GetStreamAsync(url).Result;
            }
        }


        /// <summary>
        /// post请求,格式json,返回字节
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value">请求体model</param>
        /// <returns></returns>
        public static byte[] GetByteArrayByPost(string url, object value)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsJsonAsync(url, value).Result;
                return response.Content.ReadAsByteArrayAsync().Result;
            }
        }



        /// <summary>
        /// post请求,格式json,返回流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value">请求体model</param>
        /// <returns></returns>
        public static Stream GetStreamByPost(string url, object value)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsJsonAsync(url, value).Result;
                return response.Content.ReadAsStreamAsync().Result;
            }
        }



        /// <summary>
        /// post请求,格式json,返回model
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value">请求体model</param>
        /// <returns></returns>
        public static T PostJson<T>(string url, object value)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage response = client.PostAsJsonAsync(url, value).Result;
                return response.Content.ReadAsAsync<T>().Result;
            }
        }


        /// <summary>
        /// post请求,格式json,返回string
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PostJson(string url, object value)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage response = client.PostAsJsonAsync(url, value).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }


        /// <summary>
        /// post请求,格式string,返回string
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PostString(string url, string value)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync(url, new StringContent(value)).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

    }
}
