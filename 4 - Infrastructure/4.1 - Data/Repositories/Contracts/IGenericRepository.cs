using Moviit.Infrastructure.CrossCutting.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Moviit.Infrastructure.Data.Repositories.Contracts
{

    /// <summary>
    /// Generic interface for the repository pattern.
    /// The repository is defined as generic to define in just one place the basic and common use for datasets
    /// </summary>
    /// <typeparam name="TEntity">Entity type over which the repository is instantiated</typeparam>
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {

        /// <summary>
        /// Returns an IQueryable of the entity type
        /// </summary>
        /// <returns>The IQueryable of the entity type</returns>
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Adds an item to the repository
        /// </summary>
        /// <param name="item">Item to add</param>
        void Create(TEntity item);

        /// <summary>
        /// Gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        TEntity Read(decimal id);

        /// <summary>
        /// Gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        TEntity Read(string id);

        /// <summary>
        /// Gets an item from the repository given his composed id
        /// </summary>
        /// <param name="keyValues">Composed id of the item to get</param>
        TEntity Read(object[] keyValues);

        /// <summary>
        /// Asynchronously gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        Task<TEntity> ReadAsync(decimal id);

        /// <summary>
        /// Asynchronously gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        Task<TEntity> ReadAsync(string id);

        /// <summary>
        /// Asynchronously gets an item from the repository given his composed id
        /// </summary>
        /// <param name="keyValues">Composed id of the item to get</param>
        Task<TEntity> ReadAsync(object[] keyValues);

        /// <summary>
        /// Updates an item in the repository
        /// </summary>
        /// <param name="item">Item to update</param>
        void Update(TEntity item);

        /// <summary>
        /// Deletes an item of the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to delete</param>
        void DeleteById(decimal id);

        /// <summary>
        /// Deletes an item of the repository
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Delete(TEntity item);

        /// <summary>
        /// Gets all the entities from the repository
        /// </summary>
        /// <returns>All the entities from the repository</returns>
        IEnumerable<TEntity> List();

        /// <summary>
        /// Asynchronously gets all the entities from the repository
        /// </summary>
        /// <returns>All the entities from the repository</returns>
        Task<IEnumerable<TEntity>> ListAsync();

        /// <summary>
        /// Filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filter">Expression to filter the repository</param>
        /// <param name="orderBy">Function user to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Asynchronously filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filter">Expression to filter the repository</param>
        /// <param name="orderBy">Function used to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filters">Enumeration of expressions to filter the repository</param>
        /// <param name="orderBy">Function user to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        IEnumerable<TEntity> GetFiltered(IEnumerable<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Asynchronously filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filters">Enumeration of expressions to filter the repository</param>
        /// <param name="orderBy">Function used to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        Task<IEnumerable<TEntity>> GetFilteredAsync(IEnumerable<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Gets a page of entities from the repository according to the received parameters
        /// </summary>
        /// <typeparam name="KProperty">Property to make the sort</typeparam>
        /// <param name="pageIndex">Index of the page to return</param>
        /// <param name="pageCount">Quantity of items by page</param>
        /// <param name="filter">Expression to filter the repository</param>
        /// <param name="orderByExpression">Function used to order the filtered results</param>
        /// <param name="ascending">Signals if the sorting must be ascending or descending</param>
        /// <returns>A page of entities from the repository according to the received parameters</returns>
        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, KProperty>> orderByExpression = null, bool ascending = true);

        /// <summary>
        /// Gets a page of entities from the repository according to the received parameters
        /// </summary>
        /// <typeparam name="KProperty">Property to make the sort</typeparam>
        /// <param name="pageIndex">Index of the page to return</param>
        /// <param name="pageCount">Quantity of items by page</param>
        /// <param name="filters">Enumeration of expressions to filter the repository</param>
        /// <param name="orderByExpression">Function used to order the filtered results</param>
        /// <param name="ascending">Signals if the sorting must be ascending or descending</param>
        /// <returns>A page of entities from the repository according to the received parameters</returns>
        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, IEnumerable<Expression<Func<TEntity, bool>>> filters = null, Expression<Func<TEntity, KProperty>> orderByExpression = null, bool ascending = true);

        /// <summary>
        /// Creates a enumeration of lambda expressions to use on Where expressions
        /// </summary>
        /// <param name="filters">Filters to use to build the lambda expression</param>
        /// <returns>An enumeration of lambda expressions to use on Where expressions</returns>
        IEnumerable<Expression<Func<TEntity, bool>>> CreateLambdaExpressions(List<EntityFilter> filters);

        /// <summary>
        /// Creates the expression to make an OrderBy or OrderByDescending in a Linq expression
        /// </summary>
        /// <param name="sortFieldName">Name of the property by whom the sorting will be made</param>
        /// <returns>Expression to make an OrderBy or OrderByDescending in a Linq expression</returns>
        Expression<Func<TEntity, TResult>> CreateSortExpression<TResult>(string sortFieldName);

    }
}
