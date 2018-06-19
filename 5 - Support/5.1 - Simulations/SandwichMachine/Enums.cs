namespace Moviit.Support.Simulations.SandwichMachine
{

    /// <summary>
    /// Machine implementations enumeration.
    /// </summary>
    public enum MachineImplementations
    {
        /// <summary>
        /// Patient machine. It will make orders wait when full.
        /// </summary>
        Patient = 1,
        /// <summary>
        /// Intolerant machine. It will throw exceptions when full.
        /// </summary>
        Intolerant = 2
    }

    /// <summary>
    /// Cut type enumeration.
    /// </summary>
    public enum CutType
    {
        /// <summary>
        /// Square cut
        /// </summary>
        Square = 1,
        /// <summary>
        /// Round cut
        /// </summary>
        Round = 2,
        /// <summary>
        /// Triangle cut
        /// </summary>
        Triangle = 3
    }

}