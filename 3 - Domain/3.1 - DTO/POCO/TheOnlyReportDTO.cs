using System;

namespace Moviit.Domain.DTO.POCO
{

    /// <summary>
    /// POCO for the data of the only report in the system
    /// </summary>
    public class TheOnlyReportDTO
    {

        /// <summary>
        /// Name of the sandwich in the report
        /// </summary>
        public string SandwichName { get; set; }

        /// <summary>
        /// Quantity of the sandwich in the report
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Order date of the sandwich in the report
        /// </summary>
        public DateTime OrderDate { get; set; }

    }
}
