using Moviit.Services.BaseServices.Faulting.Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Moviit.Services.BaseServices.Faulting
{

    /// <summary>
    /// Implements the conversion of every exception throwed by any method invoked by the service operations into a FaultException.
    /// If the original exception has a numeric code as message then the handler proceeds to create the FaultException using the definition for that code stored in the language specific resource file,
    /// otherwise creates a generic FaultException.
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {

        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether the dispatcher aborts the session and the instance context in certain cases.
        /// </summary>
        /// <param name="error">The exception thrown during processing.</param>
        /// <returns>True if Windows Communication Foundation (WCF) should not abort the session (if there is one) and instance context if the instance context is not InstanceContextMode.Single; otherwise, false.
        /// The default is false.</returns>
        public bool HandleError(Exception error)
        {
            //Do not handle the error
            return false;
        }

        /// <summary>
        /// Enables the creation of a custom FaultException (or WebFaultException) that is returned from an exception in the course of a service method
        /// </summary>
        /// <param name="error">The exception object thrown in the course of the service operation</param>
        /// <param name="version">The SOAP version of the message</param>
        /// <param name="fault">The Channels.Message object that is returned to the client, or service, in the duplex case</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //Proceed only of the exception is not a FaultException, if it's a FaultException it means it was handled before in the current thread
            if (!(error is FaultException))
            {
                //Unwrap the Unity exceptions
                if ((error.Source == "Microsoft.Practices.Unity") && (error.InnerException != null) && (error.InnerException.InnerException != null))
                {
                    error = error.InnerException.InnerException;
                }

                int faultCode;
                FaultException faultEx;

                if (int.TryParse(error.Message, out faultCode))
                {
                    //If the exception message is a number then treat it like fault message code and build the FaultException with it

                    //Get the error message
                    string errorMessage = FaultMessages.ResourceManager.GetString(string.Format("FaultMessage{0}", faultCode));
                    if (!(string.IsNullOrWhiteSpace(errorMessage)))
                    {
                        if (error.Data.Count > 0)
                        {
                            //If the exception has Data then use it to format the error message
                            List<string> data = new List<string>();

                            for (int i = 0; i < error.Data.Count; i++)
                            {
                                data.Add(error.Data[i].ToString());
                            }

                            try
                            {
                                errorMessage = string.Format(errorMessage, data.ToArray());

                                //Build the FaultException
                                faultEx = new FaultException(new FaultReason(errorMessage),
                                                             new FaultCode(faultCode.ToString()));
                            }
                            catch (FormatException)
                            {
                                //The arguments to replace in the error message were bad defined
                                faultEx = new FaultException(new FaultReason(FaultMessages.FaultMessage1000),
                                                             new FaultCode("1000"));
                            }
                        }
                        else
                        {
                            //Build the FaultException
                            faultEx = new FaultException(new FaultReason(errorMessage),
                                                         new FaultCode(faultCode.ToString()));
                        }
                    }
                    else
                    {
                        //The exception code didn't correspond to any defined resource
                        faultEx = new FaultException(new FaultReason(FaultMessages.FaultMessage1000),
                                                     new FaultCode("1000"));
                    }
                }
                else
                {
                    //If the exception message is not a number then is an unhandled or catastrophic exception
                    faultEx = new FaultException(new FaultReason(FaultMessages.FaultMessage1000),
                                                 new FaultCode("1000"));
                }

                //Construct the message to return as reference
                MessageFault msgFault = faultEx.CreateMessageFault();

                //Manage the SOAP or REST different returns
                if (!(WebOperationContext.Current.IncomingRequest.ContentType.StartsWith("application/json")))
                {
                    //SOAP
                    fault = Message.CreateMessage(version, msgFault, faultEx.Action);
                }
                else
                {
                    //REST
                    fault = CreateRESTFault(version, faultEx.Action, faultEx.Message);
                }
            }
        }

        /// <summary>
        /// Creates the message to return that will be understood by REST clients.
        /// </summary>
        /// <param name="version">The SOAP version of the message</param>
        /// <param name="action">Action of the error</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Optional detail to add to the detailed information</param>
        /// <returns>The message for the REST case</returns>
        private Message CreateRESTFault(MessageVersion version, string action, string message, string details = "")
        {
            //Create the message
            Message fault = Message.CreateMessage(version,
                                                  action,
                                                  new ErrorData(message, details),
                                                  new DataContractJsonSerializer(typeof(string),
                                                                                 new DataContractJsonSerializerSettings()
                                                                                 {
                                                                                     EmitTypeInformation = EmitTypeInformation.Never
                                                                                 }));

            //Set the message name property to Json
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));

            //Set the outgoing response parameters (500 code and same conten type as the request)
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            WebOperationContext.Current.OutgoingResponse.ContentType = WebOperationContext.Current.IncomingRequest.ContentType;

            return fault;
        }

    }
}
