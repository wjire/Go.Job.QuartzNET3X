using Go.Job.IProvider;
using Go.Job.Provider;
using System;
using System.Collections.Generic;

namespace Go.Job.Service
{
    public class JobServicesContainer
    {
        private Dictionary<Type, object> _serviceContainer = new Dictionary<Type, object>();

        private static readonly JobServicesContainer _jobServicesContainer;

        private JobServicesContainer()
        {
            SetService<IJobInfoProvider>(new DefaultJobInfoProvider());
        }

        static JobServicesContainer()
        {
            _jobServicesContainer = new JobServicesContainer();
        }

        public static JobServicesContainer Instance => _jobServicesContainer;

        public object GetService(Type serviceType)
        {
            _serviceContainer.TryGetValue(serviceType, out object service);
            if (service != null)
            {
                return service;
            }

            throw new ArgumentException("Not Found");
        }

        public void ReplaceService(Type serviceType, object newService)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            _serviceContainer[serviceType] = newService;
        }


        private void SetService<T>(T instance) where T : class
        {
            _serviceContainer[typeof(T)] = instance;
        }
    }
}
