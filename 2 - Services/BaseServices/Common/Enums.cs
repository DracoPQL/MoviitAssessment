namespace Moviit.Services.BaseServices.Common
{

    /// <summary>
    /// Enumeration for the different comparison operators to be used in the operation validations
    /// </summary>
    public enum CompareOperators
    {
        /// <summary>
        /// Equals
        /// </summary>
        Equals = 1,
        /// <summary>
        /// Not equals
        /// </summary>
        NotEquals = 2,
        /// <summary>
        /// Greater than
        /// </summary>
        GreaterThan = 3,
        /// <summary>
        /// Greater than or equals
        /// </summary>
        GreaterThanOrEquals = 4,
        /// <summary>
        /// Less than
        /// </summary>
        LessThan = 5,
        /// <summary>
        /// Less than or equals
        /// </summary>
        LessThanOrEquals = 6,
        /// <summary>
        /// Contains
        /// </summary>
        Contains = 8,
        /// <summary>
        /// Not contains
        /// </summary>
        NotContains = 9
    }
}
