using System;
using System.Collections.Generic;

namespace Moviit.Domain.DTO.POCO
{

    /// <summary>
    /// POCO to represent an order.
    /// </summary>
    public class Order
    {

        /// <summary>
        /// Order Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Order State
        /// </summary>
        public OrderState State { get; set; }

        /// <summary>
        /// Table where the order was made
        /// </summary>
        public int TableNumber { get; set; }

        /// <summary>
        /// Sandwichs of the order
        /// </summary>
        public IEnumerable<Sandwich> Sandwichs { get; set; }

        /// <summary>
        /// Quantities for each sandwich in the order
        /// </summary>
        public IEnumerable<int> Quantities { get; set; }

        /// <summary>
        /// Creation date and time of the order
        /// </summary>
        public DateTime CreationDate { get; set; }

    }
}
