using Moviit.Infrastructure.CrossCutting.Utils.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Moviit.Services.BaseServices.CORS
{

    /// <summary>
    /// Defines the methods that enable custom inspection or modification of inbound and outbound application messages in service applications.
    /// This particular dispatcher allows the CORS (Cross-origin resource sharing) requests addig the "Access-Control-*" headers to the response as configured.
    /// </summary>
    public class CORSDispatcher : IDispatchMessageInspector
    {

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// This dispatcher returns null because doesn't performs actions immediatelly after receiving the request.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>The object used to correlate state. This object is passed back in the IDispatchMessageInspector.BeforeSendReply(Message@, Object) method.</returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //Do nothing, this dispatcher doesn't performs actions after receiving the request
            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// This dispatcher adds the "Access-Control-*" headers to enable CORS based in the configuration settings.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">
        /// The correlation object returned from the IDispatchMessageInspector.AfterReceiveRequest(Message@, IClientChannel, InstanceContext) method.
        /// In this case the correlation object is null.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            //If the message is not a one way message
            if (reply != null)
            {
                //Check if the allowed origins are configured
                string allowedOrigins = ConfigUtilities.GetValue("CORS_ALLOWED_ORIGINS");

                if (!(string.IsNullOrWhiteSpace(allowedOrigins)))
                {
                    //Get a reference to the http response and add the "Access-Control-*" headers
                    if (reply.Properties.ContainsKey("httpResponse"))
                    {
                        HttpResponseMessageProperty httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;

                        httpHeader.Headers.Add("Access-Control-Allow-Origin", allowedOrigins);
                        httpHeader.Headers.Add("Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS");
                        httpHeader.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With,Content-Type");

                        //Add the "accepted" state to the response (only to OPTIONS requests)
                        if (WebOperationContext.Current.IncomingRequest.Method == "OPTIONS")
                        {
                            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Accepted;
                        }
                    }
                }
            }
        }

    }
}
