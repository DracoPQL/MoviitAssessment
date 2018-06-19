using AutoMapper;
using CommonServiceLocator;
using Moviit.Domain.Business.Sandwichs.Contracts;
using Moviit.Infrastructure.CrossCutting.Utils.Configuration;
using Moviit.Services.BaseServices.Unity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Unity;
using Unity.Injection;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using Unity.ServiceLocation;

namespace Moviit.Services.MinisServices.Unity
{

    /// <summary>
    /// Class that creates a Unity container instance and passes it in to the constructor of the UnityServiceHost class
    /// </summary>
    public class UnityServiceHostFactory : ServiceHostFactory, IDisposable
    {

        #region Variables

        /// <summary>
        /// Container used to register the types
        /// </summary>
        private readonly IUnityContainer _container;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UnityServiceHostFactory()
        {
            //Create the container
            _container = new UnityContainer();

            //Add the Interceptor extension
            _container.AddNewExtension<Interception>();

            //Register the types
            RegisterTypes(_container);

            //Register the Unity Service Locator as the Service Locator.
            //Warning: Use only to get the current container instance in sites were is otherwise unaccesible (like Attributes, IEndpointBehaviors or IClientMessageInspector implementations)
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(_container));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a UnityServiceHost for a specified type of service with a specific base address
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host</param>
        /// <param name="baseAddresses">The System.Array of type System.Uri that contains the base addresses for the service hosted</param>
        /// <returns>A UnityServiceHost for the type of service specified with a specific base address</returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new UnityServiceHost(this._container, serviceType, baseAddresses);
        }

        /// <summary>
        /// Register the types for the interfaces and adds the inteceptors for everyone of them
        /// </summary>
        /// <param name="container">Container to hold the type registration</param>
        private void RegisterTypes(IUnityContainer container)
        {
            //Get the automapper configuration to be injected
            IMapper mapperConfiguration = GetMapperConfiguration();

            //Get the Entity Framework DbContext to be injected
            DbContext dbContext = GetEntityFrameworkDbContext();

            //Load the configured assembly
            Assembly assembly = Assembly.Load(ConfigUtilities.GetValue("SERVICE_BUSINESS_IMPLEMENTATION"));

            //Register the types inside the loaded assembly that contains the implementations for the interfaces
            container.RegisterType(typeof(ISandwichManagement),
                                   assembly.GetTypes().Where(x => typeof(ISandwichManagement).IsAssignableFrom(x)).FirstOrDefault(),
                                   new InjectionConstructor(dbContext, mapperConfiguration),
                                   new Interceptor<VirtualMethodInterceptor>(),
                                   new InterceptionBehavior<TracingInterceptionBehavior>());
            container.RegisterType(typeof(IOrderOps),
                                   assembly.GetTypes().Where(x => typeof(IOrderOps).IsAssignableFrom(x)).FirstOrDefault(),
                                   new InjectionConstructor(dbContext, mapperConfiguration),
                                   new Interceptor<VirtualMethodInterceptor>(),
                                   new InterceptionBehavior<TracingInterceptionBehavior>());
        }

        /// <summary>
        /// Gets the automapper configuration defined for the services and to be injected in every implementation
        /// </summary>
        /// <returns>The automapper configuration</returns>
        private IMapper GetMapperConfiguration()
        {
            //Load the configured assembly
            Assembly assembly = Assembly.Load(ConfigUtilities.GetValue("AUTOMAPPER_CONFIGURATION_ASSEMBLY"));

            //Get the type
            Type configuration = assembly.GetType(ConfigUtilities.GetValue("AUTOMAPPER_CONFIGURATION_CLASS"));

            //Configure and return the Automapper
            MapperConfiguration config = (MapperConfiguration)configuration.GetMethod("Configure").Invoke(null, null);
            IMapper mapperConfiguration = config.CreateMapper();

            return mapperConfiguration;
        }

        /// <summary>
        /// Gets the Entity Framework DbContext defined for the services and to be injected in every implementation
        /// </summary>
        /// <returns>The Entity Framework DbContext</returns>
        private DbContext GetEntityFrameworkDbContext()
        {
            //Load the configured assembly
            Assembly assembly = Assembly.Load(ConfigUtilities.GetValue("ENTITY_FRAMEWORK_DBCONTEXT"));

            //Create the dbContext and return it
            Type contextType = assembly.GetTypes().Where(x => typeof(DbContext).IsAssignableFrom(x)).FirstOrDefault();
            DbContext context = (DbContext)Activator.CreateInstance(contextType);

            return context;
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Variable used to detect redundant invocations
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Main method for the IDisposable interface
        /// </summary>
        /// <param name="disposing">Signals if this objects is already being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                //Free all the resources if this is the first call to this method and if this object is being disposed
                if (_container != null)
                    _container.Dispose();
            }

            isDisposed = true;
        }

        /// <summary>
        /// Added code to implement the Disposable pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
