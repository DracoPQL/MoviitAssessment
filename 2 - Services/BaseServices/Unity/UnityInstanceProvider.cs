using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Unity;

namespace Moviit.Services.BaseServices.Unity
{

    /// <summary>
    /// Class that resolves the types from the Unity container
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider, IContractBehavior
    {

        #region Variables

        /// <summary>
        /// Container used to register the types
        /// </summary>
        private readonly IUnityContainer _container;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="container">Container to use for type resolving</param>
        public UnityInstanceProvider(IUnityContainer container)
        {
            //Check container parameter
            if (container == null)
            {
                throw new SystemConfigurationException("1050");
            }

            _container = container;
        }

        #endregion

        #region IInstanceProvider Members

        /// <summary>
        /// Returns a service object given the specified InstanceContext object
        /// </summary>
        /// <param name="instanceContext">The current InstanceContext object</param>
        /// <param name="message">The message that triggered the creation of a service object</param>
        /// <returns>The service object</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        /// <summary>
        /// Returns a service object given the specified InstanceContext object
        /// </summary>
        /// <param name="instanceContext">The current InstanceContext object</param>
        /// <returns>The service object</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return _container.Resolve(instanceContext.Host.Description.ServiceType);
        }

        /// <summary>
        /// Called when an InstanceContext object recycles a service object
        /// </summary>
        /// <param name="instanceContext">The service's instance context</param>
        /// <param name="instance">The service object to be recycled</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            //Do nothing
        }

        #endregion

        #region IContractBehavior Members

        /// <summary>
        /// Configures any binding elements to support the contract behavior
        /// </summary>
        /// <param name="contractDescription">The contract description to modify</param>
        /// <param name="endpoint">The endpoint to modify</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior</param>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //Do nothing
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract
        /// </summary>
        /// <param name="contractDescription">The contract description for which the extension is intended</param>
        /// <param name="endpoint">The endpoint</param>
        /// <param name="clientRuntime">The client runtime</param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //Do nothing
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract
        /// </summary>
        /// <param name="contractDescription">The contract description to be modified</param>
        /// <param name="endpoint">The endpoint that exposes the contract</param>
        /// <param name="dispatchRuntime">The dispatch runtime that controls service execution</param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = (IInstanceProvider)this;
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior
        /// </summary>
        /// <param name="contractDescription">The contract to validate</param>
        /// <param name="endpoint">The endpoint to validate</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            //Do nothing
        }

        #endregion

    }
}
