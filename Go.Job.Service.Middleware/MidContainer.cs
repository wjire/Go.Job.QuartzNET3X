using System;
using System.Collections.Generic;

namespace Go.Job.Service.Middleware
{
    public static class MidContainer
    {
        private static readonly Dictionary<Type, object> Container = new Dictionary<Type, object>();

        static MidContainer()
        {
            Init();
        }

        internal static void Init()
        {
            SetService<ILogWriter>(new DefaultLogWriter());
        }

        public static object GetService(Type serviceType)
        {
            Container.TryGetValue(serviceType, out object service);
            if (service != null)
            {
                return service;
            }

            throw new ArgumentException($"{serviceType.FullName} Not Found");
        }

        public static void ReplaceService(Type serviceType, object newService)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            Container[serviceType] = newService;
        }


        internal static void SetService<T>(T instance) where T : class
        {
            Container[typeof(T)] = instance;
        }
    }
}
