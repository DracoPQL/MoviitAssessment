namespace Moviit.Support.Simulations.SandwichMachine
{

    /// <summary>
    /// DTO to be returned by the machine simulation as a prepared sandiwch
    /// </summary>
    public class Sandwich
    {

        /// <summary>
        /// Sandwich cut type
        /// </summary>
        public CutType Cut { get; set; }

        /// <summary>
        /// Marks if the sandwich is compressed o not
        /// </summary>
        public bool Compressed { get; set; }

        /// <summary>
        /// Content of the sandwich as an array of strings
        /// </summary>
        public string[] Contents { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Sandwich()
        {
            Compressed = false;
        }

    }
}
