using Go.Job.Service.Log;
using System;
using System.Collections.Generic;

namespace Go.Job.Service
{
    public static class ServiceContainer
    {
        private static Dictionary<Type, object> _serviceContainer = new Dictionary<Type, object>();

        static ServiceContainer()
        {
            Init();
        }

        internal static void Init()
        {
            SetService<ILogWriter>(new DefaultLogWriter());

        }

        public static object GetService(Type serviceType)
        {
            _serviceContainer.TryGetValue(serviceType, out object service);
            if (service != null)
            {
                return service;
            }

            throw new ArgumentException("Not Found");
        }

        public static void ReplaceService(Type serviceType, object newService)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            _serviceContainer[serviceType] = newService;
        }


        internal static void SetService<T>(T instance) where T : class
        {
            _serviceContainer[typeof(T)] = instance;
        }
    }
}
