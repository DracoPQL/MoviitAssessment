using System.Collections.Generic;

namespace Moviit.Domain.DTO.POCO
{

    /// <summary>
    /// POCO to represent a sandwich
    /// </summary>
    public class Sandwich
    {

        /// <summary>
        /// Sandwich Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sandwich Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sandwich Type
        /// </summary>
        public SandwichType Type { get; set; }

        /// <summary>
        /// Ingredients of the Sandwich
        /// </summary>
        public IEnumerable<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Sandwich Price
        /// </summary>
        public decimal Price { get; set; }

    }
}
