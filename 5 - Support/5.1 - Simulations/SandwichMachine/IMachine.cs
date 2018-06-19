namespace Moviit.Support.Simulations.SandwichMachine
{

    /// <summary>
    /// Interface for the definition of operations of the sandwich machine simulation.
    /// </summary>
    public interface IMachine
    {

        /// <summary>
        /// Simulates the cooking of a sandwich returning the sandwich as an object
        /// </summary>
        /// <param name="commands">Ordered array of commands passed to the machine to cook a sandwich</param>
        /// <returns>The resulting sandwich</returns>
        Sandwich Cook(MachineCommand[] commands);

    }
}
