using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Moviit.Services.BaseServices.Faulting
{

    /// <summary>
    /// Utility class for Faulting.
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// Creates and returns a FaultException if the incoming request is a SOAP request or a WebFaultException if it is a REST request.
        /// </summary>
        /// <param name="message">Fault message to show</param>
        /// <param name="code">Code associated to the error</param>
        /// <param name="faultDetails">Optional details to add to the error</param>
        /// <returns>The FaultException or WebFaultException according to the incoming request and the given parameters</returns>
        public static FaultException CreateFault(string message, int code, FaultExceptionDetail faultDetails = null)
        {
            FaultException response = null;

            //Manage the SOAP or REST different returns
            if (!(WebOperationContext.Current.IncomingRequest.ContentType.StartsWith("application/json")))
            {
                //SOAP
                if (faultDetails == null || faultDetails.FieldsErrors.Count == 0)
                {
                    //The fault doesn't has details
                    response = new FaultException(new FaultReason(message), new FaultCode(code.ToString()));
                }
                else
                {
                    //Fault with details
                    response = new FaultException<FaultExceptionDetail>(faultDetails, new FaultReason(message), new FaultCode(code.ToString()));
                }
            }
            else
            {
                //REST
                if (faultDetails == null || faultDetails.FieldsErrors.Count == 0)
                {
                    //The fault doesn't has details
                    response = new WebFaultException<ErrorData>(new ErrorData(message, string.Empty), HttpStatusCode.InternalServerError);
                }
                else
                {
                    //Fault with details
                    string details = string.Empty;
                    for (int i = 0; i < faultDetails.FieldsErrors.Count; i++)
                    {
                        details += string.Format("{0}-{1}|", faultDetails.FieldsErrors[i].FieldName, faultDetails.FieldsErrors[i].ErrorMessage);
                    }

                    response = new WebFaultException<ErrorData>(new ErrorData(message, details), HttpStatusCode.InternalServerError);
                }
            }

            return response;
        }

    }
}
