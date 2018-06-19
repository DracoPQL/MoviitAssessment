using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moviit.Support.Simulations.SandwichMachine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviit.Support.Testing.UnitTesting
{

    /// <summary>
    /// Test for the Machine Sandwich simulation.
    /// </summary>
    [TestClass]
    public class SandwichMachineTest
    {

        #region Variables

        private TestContext _testContextInstance;
        private readonly IMachine machine;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SandwichMachineTest()
        {
            machine = MachineFactory.CreateAndReturn(MachineImplementations.Patient);
        }

        #endregion

        /// <summary>
        /// Test just one sandwich
        /// </summary>
        [TestMethod]
        public void Cook1Sandwich()
        {
            MachineCommand[] commands = CreateCommands();

            Sandwich sandwich = machine.Cook(commands);

            _testContextInstance.WriteLine("sandwich ready");
        }

        /// <summary>
        /// Test two sandwichs at the same time
        /// </summary>
        [TestMethod]
        public void Cook2SandwichConcurrent()
        {
            Parallel.For(0, 2, i =>
            {
                MachineCommand[] commands = CreateCommands();

                Sandwich sandwich = machine.Cook(commands);

                _testContextInstance.WriteLine("sandwich ready");
            });
        }

        /// <summary>
        /// Test three sandwichs at the same time
        /// </summary>
        [TestMethod]
        public void Cook3SandwichConcurrent()
        {
            Parallel.For(0, 3, i =>
            {
                MachineCommand[] commands = CreateCommands();
                machine.Cook(commands);

                _testContextInstance.WriteLine("sandwich ready");
            });
        }

        /// <summary>
        /// Test four sandwichs at the same time
        /// </summary>
        [TestMethod]
        public void Cook4SandwichConcurrent()
        {
            Parallel.For(0, 4, i =>
            {
                MachineCommand[] commands = CreateCommands();
                machine.Cook(commands);

                _testContextInstance.WriteLine("sandwich ready");
            });
        }

        /// <summary>
        /// Test five sandwichs at the same time
        /// </summary>
        [TestMethod]
        public void Cook5SandwichConcurrent()
        {
            Parallel.For(0, 5, i =>
            {
                MachineCommand[] commands = CreateCommands();
                machine.Cook(commands);

                _testContextInstance.WriteLine("sandwich ready");
            });
        }

        /// <summary>
        /// Test six sandwichs at the same time
        /// </summary>
        [TestMethod]
        public void Cook6SandwichConcurrent()
        {
            Parallel.For(0, 6, i =>
            {
                MachineCommand[] commands = CreateCommands();
                machine.Cook(commands);

                _testContextInstance.WriteLine("sandwich ready");
            });
        }

        /// <summary>
        /// Test seven sandwichs at the same time
        /// </summary>
        [TestMethod]
        public void Cook7SandwichConcurrent()
        {
            Parallel.For(0, 7, i =>
            {
                MachineCommand[] commands = CreateCommands();
                machine.Cook(commands);

                _testContextInstance.WriteLine("sandwich ready");
            });
        }

        /// <summary>
        /// Single command array for making a single sandwich
        /// </summary>
        /// <returns>Command array to use in the tests</returns>
        private MachineCommand[] CreateCommands()
        {
            IList<MachineCommand> commands = new List<MachineCommand>();
            commands.Add(new AddBread());
            commands.Add(new AddIngredient()
            {
                MachineStackIndex = 1,
                Name = "Lechuga"
            });
            commands.Add(new AddIngredient()
            {
                MachineStackIndex = 2,
                Name = "Tomate"
            });
            commands.Add(new AddIngredient()
            {
                MachineStackIndex = 3,
                Name = "Palta"
            });
            commands.Add(new AddSalsa());
            commands.Add(new AddBread());
            commands.Add(new CutSandwich()
            {
                Cut = CutType.Square
            });
            commands.Add(new CompressSandwich());

            return commands.ToArray();
        }

    }
}
