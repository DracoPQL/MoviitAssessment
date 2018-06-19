using Moviit.Domain.DTO.POCO;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Moviit.Services.MinisServices
{

    /// <summary>
    /// Interface for the sandwich and other objects management and reports.
    /// </summary>
    [ServiceContract(Namespace = "urn:Moviit.Services.MinisServices")]
    public interface ISandwichsService
    {

        #region Sandwich Management

        /// <summary>
        /// Creates a new sandwich
        /// </summary>
        /// <param name="request">Sandwich to create</param>
        /// <returns>Id of the new created sandwich</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        Task<int> SandwichCreate(Sandwich request);

        /// <summary>
        /// Returns a sandwich given his id
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to return</param>
        /// <returns>The specified sandwich</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        Task<Sandwich> SandwichRead(int sandwichId);

        /// <summary>
        /// Updates a sandwich
        /// </summary>
        /// <param name="request">Sandwich to update</param>
        /// <returns>Task for async operations</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        Task SandwichUpdate(Sandwich request);

        /// <summary>
        /// Deletes a sandwich
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to delete</param>
        /// <returns>Task for async operations</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        Task SandwichDelete(int sandwichId);

        /// <summary>
        /// Lists the sandwichs
        /// </summary>
        /// <returns>All the sandwichs</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        Task<IEnumerable<Sandwich>> SandwichList();

        #endregion

        #region Ingredient Management

        /// <summary>
        /// Creates a new ingredient
        /// </summary>
        /// <param name="request">Ingredient to create</param>
        /// <returns>Id of the new created ingredient</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        Task<int> IngredientCreate(Ingredient request);

        /// <summary>
        /// Returns an ingredient given his id
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to return</param>
        /// <returns>The specified ingredient</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        Task<Ingredient> IngredientRead(int ingredientId);

        /// <summary>
        /// Updates an ingredient
        /// </summary>
        /// <param name="request">Ingredient to update</param>
        /// <returns>Task for async operations</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        Task IngredientUpdate(Ingredient request);

        /// <summary>
        /// Deletes an ingredient
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to delete</param>
        /// <returns>Task for async operations</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        Task IngredientDelete(int ingredientId);

        /// <summary>
        /// Lists the ingredients
        /// </summary>
        /// <returns>All the ingredients</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        Task<IEnumerable<Ingredient>> IngredientList();

        #endregion

        #region Reports

        /// <summary>
        /// Get the sandwichs sold on the last seven days with their quantities.
        /// </summary>
        /// <returns>The report data</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        Task<IEnumerable<TheOnlyReportDTO>> TheOnlyReport();

        #endregion

    }
}
