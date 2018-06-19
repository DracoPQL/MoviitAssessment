namespace Moviit.Services.BaseServices.Faulting
{

    /// <summary>
    /// Class used to show the detail of a failed DTO validation
    /// </summary>
    public class FieldValidation
    {

        /// <summary>
        /// Field name of DTO with error
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Error occurred in the referenced DTO field
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
