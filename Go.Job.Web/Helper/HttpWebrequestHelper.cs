using System.IO;
using System.Net;
using System.Text;


namespace Go.Job.Web.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpWebrequestHelper
    {

        /// <summary>
        /// Get 请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="encoding">接收编码格式,默认 UTF8</param>
        /// <returns></returns>
        public static string Get(string url, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
            {
                return reader.ReadToEnd();
            }
        }


        /// <summary>
        /// Json格式Post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="body">请求参数</param>
        /// <param name="encoding">请求参数的编码方式,默认 UTF8</param>
        /// <returns></returns>
        public static string PostJson(string url, string body, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);

            //设置请求体的格式
            request.ContentType = "application/json";
            return Post(request, body, encoding);
        }


        /// <summary>
        /// form表单格式Post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="body">请求参数</param>
        /// <param name="encoding">请求参数的编码方式,默认 UTF8</param>
        /// <returns></returns>
        public static string PostForm(string url, string body, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);

            //设置请求体的格式
            request.ContentType = "application/x-www-form-urlencoded";
            return Post(request, body, encoding);
        }


        private static string Post(WebRequest request, string body, Encoding encoding)
        {
            request.Method = "POST";
            //将参数按指定的编码方式转换成字节数组
            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;

            //将字节数组写入到请求流中
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
