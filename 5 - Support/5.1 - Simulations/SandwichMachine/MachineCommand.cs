using System;
using System.Collections.Generic;
using System.Linq;

namespace Moviit.Support.Simulations.SandwichMachine
{

    /// <summary>
    /// Base class for the command received by the sandwic machine simulation
    /// </summary>
    public abstract class MachineCommand
    {

        /// <summary>
        /// Apply a command to the sandwich being cooked
        /// </summary>
        /// <param name="sandwich">Sandwich being cooked</param>
        /// <returns>The sandwich after applying the command</returns>
        public abstract Sandwich ApplyCommand(Sandwich sandwich);

    }

    /// <summary>
    /// Add bread command implementation
    /// </summary>
    public class AddBread : MachineCommand
    {

        /// <summary>
        /// Adds bread to the sandwich being cooked
        /// </summary>
        /// <param name="sandwich">Sandwich being cooked</param>
        /// <returns>The sandwich after adding the bread</returns>
        public override Sandwich ApplyCommand(Sandwich sandwich)
        {
            IList<string> contents;
            contents = (sandwich.Contents == null) ? (new List<string>()) : (sandwich.Contents.ToList());

            if (contents.Count(x => x == "bread") > 1)
            {
                throw new InvalidOperationException("Cannot add more than 2 breads");
            }
            if (contents.Count > 0 && contents.Last() == "bread")
            {
                throw new InvalidOperationException("Cannot add bread over bread");
            }

            contents.Add("bread");

            sandwich.Contents = contents.ToArray();

            return sandwich;
        }
    }

    /// <summary>
    /// Add ingredient command implementation
    /// </summary>
    public class AddIngredient : MachineCommand
    {

        /// <summary>
        /// Stack to where get the ingredient from
        /// </summary>
        public int MachineStackIndex { get; set; }

        /// <summary>
        /// Ingredient name to add
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Adds ingredient to the sandwich being cooked
        /// </summary>
        /// <param name="sandwich">Sandwich being cooked</param>
        /// <returns>The sandwich after adding the ingredient</returns>
        public override Sandwich ApplyCommand(Sandwich sandwich)
        {
            IList<string> contents;
            contents = (sandwich.Contents == null) ? (new List<string>()) : (sandwich.Contents.ToList());

            if (MachineStackIndex < 1 && MachineStackIndex > 3)
            {
                throw new InvalidOperationException("Invalid stack index");
            }

            contents.Add(Name);

            sandwich.Contents = contents.ToArray();

            return sandwich;
        }
    }

    /// <summary>
    /// Add salsa command implementation
    /// </summary>
    public class AddSalsa : MachineCommand
    {

        /// <summary>
        /// Adds salsa to the sandwich being cooked
        /// </summary>
        /// <param name="sandwich">Sandwich being cooked</param>
        /// <returns>The sandwich after adding the salsa</returns>
        public override Sandwich ApplyCommand(Sandwich sandwich)
        {
            IList<string> contents;
            contents = (sandwich.Contents == null) ? (new List<string>()) : (sandwich.Contents.ToList());

            if (contents.Count > 0 && contents.Last() == "salsa")
            {
                throw new InvalidOperationException("Cannot add salsa over salsa");
            }
            contents.Add("salsa");

            sandwich.Contents = contents.ToArray();

            return sandwich;
        }
    }

    /// <summary>
    /// Cut sandwich command implementation
    /// </summary>
    public class CutSandwich : MachineCommand
    {

        /// <summary>
        /// Cut to apply to the sandwich
        /// </summary>
        public CutType Cut { get; set; }

        /// <summary>
        /// Cut the sandwich being cooked
        /// </summary>
        /// <param name="sandwich">Sandwich being cooked</param>
        /// <returns>The sandwich after the cut</returns>
        public override Sandwich ApplyCommand(Sandwich sandwich)
        {
            sandwich.Cut = Cut;

            return sandwich;
        }
    }

    /// <summary>
    /// Compress sandwich command implementation
    /// </summary>
    public class CompressSandwich : MachineCommand
    {

        /// <summary>
        /// Compress the sandwich being cooked
        /// </summary>
        /// <param name="sandwich">Sandwich being cooked</param>
        /// <returns>The sandwich after compression</returns>
        public override Sandwich ApplyCommand(Sandwich sandwich)
        {
            sandwich.Compressed = true;

            return sandwich;
        }
    }

}
