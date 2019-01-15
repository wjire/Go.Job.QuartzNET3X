using Go.Job.IProvider;
using System;

namespace Go.Job.Provider
{
    public class DefaultLogProvider : ILogProvider
    {
        public void LogError()
        {
            throw new NotImplementedException();
        }

        public void LogMessage()
        {
            throw new NotImplementedException();
        }
    }
}
