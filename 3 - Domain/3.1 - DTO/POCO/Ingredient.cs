namespace Moviit.Domain.DTO.POCO
{

    /// <summary>
    /// POCO to represent an ingredient
    /// </summary>
    public class Ingredient
    {

        /// <summary>
        /// Ingredient Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ingredient Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Index of the stack in the machine for this ingredient
        /// </summary>
        public int MachineStackIndex { get; set; }

    }
}
