using Moviit.Domain.Business.Sandwichs.Contracts;
using Moviit.Domain.DTO.POCO;
using Moviit.Services.BaseServices.CORS;
using Moviit.Services.BaseServices.Faulting;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Moviit.Services.MinisServices
{

    /// <summary>
    /// Implementation of the sandwich service.
    /// </summary>
    [ServiceBehavior(Namespace = "urn:Moviit.Services.MinisServices")]
    [ErrorHandlerBehavior()]
    [CORSBehavior()]
    public class SandwichsService : ISandwichsService
    {

        #region Variables

        private ISandwichManagement _sandwichManagement;

        #endregion

        #region Constructors

        /// <summary>
        /// Shared constructor. It will be called once on the service initialization.
        /// </summary>
        static SandwichsService()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SandwichsService()
        {
        }

        /// <summary>
        /// Constructor with injected dependencies
        /// </summary>
        /// <param name="sandwichManagement">ISandwichManagement implementation to use</param>
        public SandwichsService(ISandwichManagement sandwichManagement)
        {
            _sandwichManagement = sandwichManagement;
        }

        #endregion

        #region Sandwich Management

        /// <summary>
        /// Creates a new sandwich
        /// </summary>
        /// <param name="request">Sandwich to create</param>
        /// <returns>Id of the new created sandwich</returns>
        public async Task<int> SandwichCreate(Sandwich request)
        {
            int result = await _sandwichManagement.SandwichCreate(request);

            return result;
        }

        /// <summary>
        /// Returns a sandwich given his id
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to return</param>
        /// <returns>The specified sandwich</returns>
        public async Task<Sandwich> SandwichRead(int sandwichId)
        {
            Sandwich result = await _sandwichManagement.SandwichRead(sandwichId);

            return result;
        }

        /// <summary>
        /// Updates a sandwich
        /// </summary>
        /// <param name="request">Sandwich to update</param>
        /// <returns>Task for async operations</returns>
        public async Task SandwichUpdate(Sandwich request)
        {
            await _sandwichManagement.SandwichUpdate(request);
        }

        /// <summary>
        /// Deletes a sandwich
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to delete</param>
        /// <returns>Task for async operations</returns>
        public async Task SandwichDelete(int sandwichId)
        {
            await _sandwichManagement.SandwichDelete(sandwichId);
        }

        /// <summary>
        /// Lists the sandwichs
        /// </summary>
        /// <returns>All the sandwichs</returns>
        public async Task<IEnumerable<Sandwich>> SandwichList()
        {
            IEnumerable<Sandwich> result = await _sandwichManagement.SandwichList();

            return result;
        }

        #endregion

        #region Ingredient Management

        /// <summary>
        /// Creates a new ingredient
        /// </summary>
        /// <param name="request">Ingredient to create</param>
        /// <returns>Id of the new created ingredient</returns>
        public async Task<int> IngredientCreate(Ingredient request)
        {
            int result = await _sandwichManagement.IngredientCreate(request);

            return result;
        }

        /// <summary>
        /// Returns an ingredient given his id
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to return</param>
        /// <returns>The specified ingredient</returns>
        public async Task<Ingredient> IngredientRead(int ingredientId)
        {
            Ingredient result = await _sandwichManagement.IngredientRead(ingredientId);

            return result;
        }

        /// <summary>
        /// Updates an ingredient
        /// </summary>
        /// <param name="request">Ingredient to update</param>
        /// <returns>Task for async operations</returns>
        public async Task IngredientUpdate(Ingredient request)
        {
            await _sandwichManagement.IngredientUpdate(request);
        }

        /// <summary>
        /// Deletes an ingredient
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to delete</param>
        /// <returns>Task for async operations</returns>
        public async Task IngredientDelete(int ingredientId)
        {
            await _sandwichManagement.IngredientDelete(ingredientId);
        }

        /// <summary>
        /// Lists the ingredients
        /// </summary>
        /// <returns>All the ingredients</returns>
        public async Task<IEnumerable<Ingredient>> IngredientList()
        {
            IEnumerable<Ingredient> result = await _sandwichManagement.IngredientList();

            return result;
        }

        #endregion

        #region Reports

        /// <summary>
        /// Get the sandwichs sold on the last seven days with their quantities.
        /// </summary>
        /// <returns>The report data</returns>
        public async Task<IEnumerable<TheOnlyReportDTO>> TheOnlyReport()
        {
            IEnumerable<TheOnlyReportDTO> result = await _sandwichManagement.TheOnlyReport();

            return result;
        }

        #endregion

    }
}
