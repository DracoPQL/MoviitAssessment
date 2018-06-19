namespace Moviit.Infrastructure.CrossCutting.Utils.Common
{

    /// <summary>
    /// Enumeration for the different operators for the filters to be used in repository operators
    /// </summary>
    public enum FilterOperators
    {
        /// <summary>
        /// EQUALS
        /// </summary>
        EQUALS = 1,
        /// <summary>
        /// NOT EQUALS
        /// </summary>
        NOTEQUALS = 2,
        /// <summary>
        /// GREATER THAN
        /// </summary>
        GREATERTHAN = 3,
        /// <summary>
        /// GREATER THAN OR EQUALS
        /// </summary>
        GREATERTHANOREQUALS = 4,
        /// <summary>
        /// LESS THAN
        /// </summary>
        LESSTHAN = 5,
        /// <summary>
        /// LESS THAN OR EQUALS
        /// </summary>
        LESSTHANOREQUALS = 6,
        /// <summary>
        /// LIKE
        /// </summary>
        LIKE = 7,
        /// <summary>
        /// INCLUDED
        /// </summary>
        INCLUDED = 8,
        /// <summary>
        /// NOT INCLUDED
        /// </summary>
        NOTINCLUDED = 9
    }
}
