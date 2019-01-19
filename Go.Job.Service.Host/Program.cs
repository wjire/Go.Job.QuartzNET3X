using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

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
        
    }
}
