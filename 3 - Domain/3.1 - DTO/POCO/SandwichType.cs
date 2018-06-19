namespace Moviit.Domain.DTO.POCO
{

    /// <summary>
    /// POCO to represent a sandwich type
    /// </summary>
    public class SandwichType
    {

        /// <summary>
        /// Sandwich type id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sandwich type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cut type of the sandwich type 
        /// </summary>
        public SandwichCutType CutType { get; set; }

        /// <summary>
        /// True if the sandwich type has salsa, false otherwise
        /// </summary>
        public bool HasSalsa { get; set; }

        /// <summary>
        /// True if the sandwich type has salsa  over the ingredients, false indicates the salsa (if this sandwich type has it) is over the bread
        /// </summary>
        public bool SalsaInside { get; set; }

        /// <summary>
        /// True if the sandwich type is compressed, false otherwise
        /// </summary>
        public bool HasCompresssion { get; set; }
        
    }
}
