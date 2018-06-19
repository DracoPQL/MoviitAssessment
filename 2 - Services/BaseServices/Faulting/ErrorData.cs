using System.Runtime.Serialization;

namespace Moviit.Services.BaseServices.Faulting
{

    /// <summary>
    /// DTO to use as data contract for the error data returned to REST clients.
    /// </summary>
    [DataContract]
    [KnownType(typeof(ErrorData))]
    public class ErrorData
    {

        #region Constructor

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="reason">Error reason</param>
        /// <param name="detailedInformation">Error details</param>
        public ErrorData(string reason, string detailedInformation)
        {
            Reason = reason;
            DetailedInformation = detailedInformation;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Principal message for the error
        /// </summary>
        [DataMember]
        public string Reason { get; private set; }

        /// <summary>
        /// String with all the details for the error
        /// </summary>
        [DataMember]
        public string DetailedInformation { get; private set; }

        #endregion

    }
}
