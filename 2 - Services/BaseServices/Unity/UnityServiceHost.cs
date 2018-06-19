using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Unity;

namespace Moviit.Services.BaseServices.Unity
{

    /// <summary>
    /// A custom ServiceHost class that can pass a Unity container instance into the WCF infrastructure
    /// </summary>
    public class UnityServiceHost : ServiceHost
    {

        #region Constructor

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="container">Unity container to pass to the WCF infrastructure</param>
        /// <param name="serviceType">The type of hosted service</param>
        /// <param name="baseAddresses">An array of type System.Uri that contains the base addresses for the hosted service</param>
        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            //Check container parameter
            if (container == null)
            {
                throw new SystemConfigurationException("1050");
            }

            //Add the container in the behavior of each contract implemented in the service host
            foreach (ContractDescription cd in this.ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new UnityInstanceProvider(container));
            }
        }

        #endregion

    }
}
