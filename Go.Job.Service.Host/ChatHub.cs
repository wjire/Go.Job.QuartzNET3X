using Microsoft.AspNet.SignalR;
using System;

namespace Go.Job.Service.Host
{
    public class ChatHub : Hub
    {

        public void GetMessage(int id)
        {
            Console.WriteLine($"hello , { id }");
            Clients.All.GetMessage(id);
        }
    }
}
