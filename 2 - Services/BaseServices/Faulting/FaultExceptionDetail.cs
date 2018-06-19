using System.Collections.Generic;

namespace Moviit.Services.BaseServices.Faulting
{

    /// <summary>
    /// Class used to show the detailed list of a DTO validation failure
    /// </summary>
    public class FaultExceptionDetail
    {

        /// <summary>
        /// List of validation errors
        /// </summary>
        public IList<FieldValidation> FieldsErrors { get; set; }

    }
}
