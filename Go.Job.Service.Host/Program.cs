using System;
using System.Net;
using System.Net.NetworkInformation;
using Go.Job.Service.api;

namespace Go.Job.Service.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                SchedStartHelper.StartSched();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();

        }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
    }
}
