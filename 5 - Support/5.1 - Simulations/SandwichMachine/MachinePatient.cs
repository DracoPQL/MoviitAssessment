using System;
using System.Collections.Generic;
using System.Threading;

namespace Moviit.Support.Simulations.SandwichMachine
{

    /// <summary>
    /// Implementation of the operations of the sandwich machine simulation.
    /// This implementation is patient so once the machine is full making sandwichs will make the cooking orders to wait until one place is available.
    /// </summary>
    public class MachinePatient : IMachine
    {

        #region Constants

        /// <summary>
        /// Machine definition
        /// </summary>
        private const int _TimeToPrepareInMilliseconds = 5000;
        private const int _MaxSandiwchsAtTheSameTime = 5;

        #endregion

        #region Variables

        /// <summary>
        /// Thread synchronization semaphor
        /// </summary>
        private readonly SemaphoreSlim semaphore;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// Starts the machine setting initial values.
        /// </summary>
        internal MachinePatient()
        {
            semaphore = new SemaphoreSlim(_MaxSandiwchsAtTheSameTime);
        }

        #endregion

        /// <summary>
        /// Simulates the cooking of a sandwich returning the sandwich as an object
        /// </summary>
        /// <param name="commands">Ordered array of commands passed to the machine to cook a sandwich</param>
        /// <returns>The resulting sandwich</returns>
        public Sandwich Cook(MachineCommand[] commands)
        {
            semaphore.Wait();
            Sandwich result = ProcessCommands(commands);
            semaphore.Release();

            return result;
        }

        private Sandwich ProcessCommands(MachineCommand[] commands)
        {
            DateTime timeStart = DateTime.Now;

            Sandwich result = new Sandwich();

            foreach (MachineCommand command in commands)
            {
                result = command.ApplyCommand(result);
            }

            //Sleep for the remaining time to complete the cooking
            Thread.Sleep(_TimeToPrepareInMilliseconds - ((DateTime.Now - timeStart).Milliseconds));

            return result;
        }

    }
}
