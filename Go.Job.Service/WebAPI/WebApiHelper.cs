using System;
using Microsoft.Owin.Hosting;

namespace Go.Job.Service.WebAPI
{
    public static class WebApiHelper
    {
        public static void Start(string address)
        {
            string baseAddress = "http://localhost:25250/";
            using (WebApp.Start(url: baseAddress))
            {
                Console.WriteLine("webapi监听地址已启动");
                Console.ReadLine();
            }
        }
    }
}
