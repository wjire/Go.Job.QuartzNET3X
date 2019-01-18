using System;
using Microsoft.Owin.Hosting;

namespace Go.Job.Service.WebAPI
{
    public static class WebApiHelper
    {
        public static void Start(string address)
        {
            using (WebApp.Start(url: address))
            {
                Console.WriteLine($"webapi监听已启动, address : {address}");
                Console.ReadLine();
            }
        }
    }
}
