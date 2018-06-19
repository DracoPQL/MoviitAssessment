using System;

namespace Moviit.Support.Simulations.SandwichMachine
{

    /// <summary>
    /// Factory that determines the machine implementation to create.
    /// </summary>
    public class MachineFactory
    {

        /// <summary>
        /// Decides which class to instantiate and return.
        /// </summary>
        public static IMachine CreateAndReturn(MachineImplementations implementation)
        {
            switch (implementation)
            {
                case MachineImplementations.Patient:
                    return new MachinePatient();
                case MachineImplementations.Intolerant:
                    return new MachinePatient();
                default:
                    throw new ArgumentException("Invalid implementation", "implementation");
            }
        }

    }
}
