namespace Moviit.Infrastructure.CrossCutting.Utils.Common
{

    /// <summary>
    /// Helper class used to hold the expressions used to build search trees (linq expressions) to filter the persistence
    /// </summary>
    public class EntityFilter
    {

        /// <summary>
        /// Name of the property being filtered
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Value (constant value) used to filter
        /// </summary>
        public object ValueToCompare { get; set; }

        /// <summary>
        /// Operator used to filter
        /// </summary>
        public FilterOperators ComparisonType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityFilter()
        {
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="propName">Name of the property being filtered</param>
        /// <param name="valToCompare">Value (constant value) used to filter</param>
        /// <param name="compType">Operator used to filter</param>
        public EntityFilter(string propName, object valToCompare, FilterOperators compType)
        {
            PropertyName = propName;
            ValueToCompare = valToCompare;
            ComparisonType = compType;
        }

    }
}