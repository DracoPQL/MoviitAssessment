using Moviit.Domain.DTO.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviit.Domain.Business.Sandwichs.Contracts
{

    /// <summary>
    /// Interface for the sandwich and other objects management and reports
    /// </summary>
    public interface ISandwichManagement
    {

        #region Sandwich Management

        /// <summary>
        /// Creates a new sandwich
        /// </summary>
        /// <param name="request">Sandwich to create</param>
        /// <returns>Id of the new created sandwich</returns>
        Task<int> SandwichCreate(Sandwich sandwich);

        /// <summary>
        /// Returns a sandwich given his id
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to return</param>
        /// <returns>The specified sandwich</returns>
        Task<Sandwich> SandwichRead(int sandwichId);

        /// <summary>
        /// Updates a sandwich
        /// </summary>
        /// <param name="request">Sandwich to update</param>
        /// <returns>Task for async operations</returns>
        Task SandwichUpdate(Sandwich sandwich);

        /// <summary>
        /// Deletes a sandwich
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to delete</param>
        /// <returns>Task for async operations</returns>
        Task SandwichDelete(int sandwichId);

        /// <summary>
        /// Lists the sandwichs
        /// </summary>
        /// <returns>All the sandwichs</returns>
        Task<IEnumerable<Sandwich>> SandwichList();

        #endregion

        #region Ingredient Management

        /// <summary>
        /// Creates a new ingredient
        /// </summary>
        /// <param name="request">Ingredient to create</param>
        /// <returns>Id of the new created ingredient</returns>
        Task<int> IngredientCreate(Ingredient ingredient);

        /// <summary>
        /// Returns an ingredient given his id
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to return</param>
        /// <returns>The specified ingredient</returns>
        Task<Ingredient> IngredientRead(int ingredientId);

        /// <summary>
        /// Updates an ingredient
        /// </summary>
        /// <param name="request">Ingredient to update</param>
        /// <returns>Task for async operations</returns>
        Task IngredientUpdate(Ingredient ingredient);

        /// <summary>
        /// Deletes an ingredient
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to delete</param>
        /// <returns>Task for async operations</returns>
        Task IngredientDelete(int ingredientId);

        /// <summary>
        /// Lists the ingredients
        /// </summary>
        /// <returns>All the ingredients</returns>
        Task<IEnumerable<Ingredient>> IngredientList();

        #endregion

        #region Reports

        /// <summary>
        /// Get the sandwichs sold on the last seven days with their quantities.
        /// </summary>
        /// <returns>The report data</returns>
        Task<IEnumerable<TheOnlyReportDTO>> TheOnlyReport();

        #endregion

    }
}
