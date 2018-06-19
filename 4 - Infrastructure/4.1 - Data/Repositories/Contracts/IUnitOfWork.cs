using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Moviit.Infrastructure.Data.Entities;

namespace Moviit.Infrastructure.Data.Repositories.Contracts
{

    /// <summary>
    /// Interface for the Unit of Work pattern used in this solution.
    /// The Unit of Work pattern brings these benefits when used correctly:
    /// - Abstract the ORM used and their operations.
    /// - Give a common entry point for each repository.
    /// - Ensures the consistency of the database.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        #region Properties

        /// <summary>
        /// Ingredient repository
        /// </summary>
        IGenericRepository<INGREDIENT> IngredientRepository { get; }

        /// <summary>
        /// Order repository
        /// </summary>
        IGenericRepository<ORDER> OrderRepository { get; }

        /// <summary>
        /// Order Sandwich repository
        /// </summary>
        IGenericRepository<ORDER_SANDWICH> OrderSandwichRepository { get; }

        /// <summary>
        /// Order State repository
        /// </summary>
        IGenericRepository<ORDER_STATE> OrderStateRepository { get; }

        /// <summary>
        /// Sandwich repository
        /// </summary>
        IGenericRepository<SANDWICH> SandwichRepository { get; }

        /// <summary>
        /// Sandwich Cut Type repository
        /// </summary>
        IGenericRepository<SANDWICH_CUT_TYPE> SandwichCutTypeRepository { get; }

        /// <summary>
        /// Sandwich Ingredient repository
        /// </summary>
        IGenericRepository<SANDWICH_INGREDIENT> SandwichIngredientRepository { get; }

        /// <summary>
        /// Sandwich Type repository
        /// </summary>
        IGenericRepository<SANDWICH_TYPE> SandwichTypeRepository { get; }

        #endregion

        /// <summary>
        /// Returns the database context used for this UoW
        /// </summary>
        /// <returns>The database context used for this UoW</returns>
        DbContext GetContext();

        /// <summary>
        /// Returns the next value from a sequence.
        /// </summary>
        /// <param name="sequenceName">Name of the sequence to get thenext value from</param>
        /// <returns>The next value from the sequence</returns>
        T GetNextValInSequence<T>(string sequenceName);

        /// <summary>
        /// Commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then an exception is thrown.
        /// </summary>
        void Commit();

        /// <summary>
        /// Asynchronously commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then an exception is thrown.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then the changes in the client are refreshed.
        /// </summary>
        void CommitAndRefreshChanges();

        /// <summary>
        /// Asynchronously commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then the changes in the client are refreshed.
        /// </summary>
        Task CommitAndRefreshChangesAsync();

        /// <summary>
        /// Rollback the changes made on the context that were not commited to the persistency layer.
        /// </summary>
        void RollbackChanges();

    }
}
