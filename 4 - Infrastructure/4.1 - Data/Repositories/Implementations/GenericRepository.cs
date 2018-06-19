using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Moviit.Infrastructure.CrossCutting.Utils.Common;
using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using Moviit.Infrastructure.Data.Repositories.Contracts;

namespace Moviit.Infrastructure.Data.Repositories.Implementations
{

    /// <summary>
    /// Implementation of the Generic Repository pattern
    /// </summary>
    /// <typeparam name="TEntity">Entity type over wich the repository is instantiated</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        #region Variables

        /// <summary>
        /// Context to use in the operations
        /// </summary>
        protected DbContext _context;

        /// <summary>
        /// DbSet that will be instantiated with the entity type
        /// </summary>
        protected DbSet<TEntity> _dbSet;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Contexto a usar en el repositorio</param>
        public GenericRepository(DbContext context)
        {
            if (context == null)
            {
                throw new SystemConfigurationException("An unhandled configuration error has occurred. Please check the service logs for more details.");
            }

            //Assign the context and create the entity set
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an IQueryable of the entity type
        /// </summary>
        /// <returns>The IQueryable of the entity type</returns>
        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        /// Adds an item to the repository
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Create(TEntity item)
        {
            if ((item != null))
            {
                _dbSet.Add(item);
            }
            else
            {
                throw new DatabaseOperationException("An attempt to add a null entity was found. Please check the service logs for more details.");
            }
        }

        /// <summary>
        /// Gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        public TEntity Read(decimal id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        public TEntity Read(string id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Gets an item from the repository given his composed id
        /// </summary>
        /// <param name="keyValues">Composed id of the item to get</param>
        public TEntity Read(object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        /// <summary>
        /// Asynchronously gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        public async Task<TEntity> ReadAsync(decimal id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously gets an item from the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to get</param>
        public async Task<TEntity> ReadAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously gets an item from the repository given his composed id
        /// </summary>
        /// <param name="keyValues">Composed id of the item to get</param>
        public Task<TEntity> ReadAsync(object[] keyValues)
        {
            return _dbSet.FindAsync(keyValues);
        }

        /// <summary>
        /// Updates an item in the repository
        /// </summary>
        /// <param name="item">Item to update</param>
        public void Update(TEntity item)
        {
            if (item != null)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _dbSet.Attach(item);
                }

                _context.Entry(item).State = EntityState.Modified;
            }
            else
            {
                throw new DatabaseOperationException("An attempt to add a null entity was found. Please check the service logs for more details.");
            }
        }

        /// <summary>
        /// Deletes an item of the repository given his id
        /// </summary>
        /// <param name="id">Id of the item to delete</param>
        public void DeleteById(decimal id)
        {
            TEntity item = _dbSet.Find(id);
            Delete(item);
        }

        /// <summary>
        /// Deletes an item of the repository
        /// </summary>
        /// <param name="item">Item to delete</param>
        public void Delete(TEntity item)
        {
            if (item != null)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _dbSet.Attach(item);
                }

                _dbSet.Remove(item);
            }
            else
            {
                throw new DatabaseOperationException("An attempt to add a null entity was found. Please check the service logs for more details.");
            }
        }

        /// <summary>
        /// Gets all the entities from the repository
        /// </summary>
        /// <returns>All the entities from the repository</returns>
        public IEnumerable<TEntity> List()
        {
            return _dbSet;
        }

        /// <summary>
        /// Asynchronously gets all the entities from the repository
        /// </summary>
        /// <returns>All the entities from the repository</returns>
        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filter">Expression to filter the repository</param>
        /// <param name="orderBy">Function user to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Asynchronously filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filter">Expression to filter the repository</param>
        /// <param name="orderBy">Function used to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        /// <summary>
        /// Filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filters">Enumeration of expressions to filter the repository</param>
        /// <param name="orderBy">Function user to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        public IEnumerable<TEntity> GetFiltered(IEnumerable<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Asynchronously filter, order and returns the entities from the repository according to the filtering and ordering expressions passed by parameter
        /// </summary>
        /// <param name="filters">Enumeration of expressions to filter the repository</param>
        /// <param name="orderBy">Function used to order the filtered results</param>
        /// <returns>The filtered entities from the repository sortered by the order by function</returns>
        public async Task<IEnumerable<TEntity>> GetFilteredAsync(IEnumerable<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

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
        public IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, KProperty>> orderByExpression = null, bool ascending = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (ascending)
            {
                return query.OrderBy(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount);
            }
            else
            {
                return query.OrderByDescending(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount);
            }
        }

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
        public IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, IEnumerable<Expression<Func<TEntity, bool>>> filters = null, Expression<Func<TEntity, KProperty>> orderByExpression = null, bool ascending = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (ascending)
            {
                return query.OrderBy(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount);
            }
            else
            {
                return query.OrderByDescending(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount);
            }
        }

        /// <summary>
        /// Creates a enumeration of lambda expressions to use on Where expressions
        /// </summary>
        /// <param name="filters">Filters to use to build the lambda expression</param>
        /// <returns>An enumeration of lambda expressions to use on Where expressions</returns>
        public IEnumerable<Expression<Func<TEntity, bool>>> CreateLambdaExpressions(List<EntityFilter> filters)
        {
            //Set the parameter to associate to the query
            dynamic param = Expression.Parameter(typeof(TEntity), "e");
            List<Expression<Func<TEntity, bool>>> finalExpressions = null;
            Expression<Func<TEntity, bool>> lambdaExpression = null;
            Expression member = default(Expression);
            ConstantExpression constant = default(ConstantExpression);
            IList objectsList = null;

            //For each tuple received construct the lambda expression to use in the filter and add it to the query as a where clause
            for (int i = 0; i <= filters.Count() - 1; i++)
            {
                //Get the property to filter
                member = Expression.Property(param, filters[i].PropertyName);

                //Get the value to compare
                //if the property to filter is of a nullable type then conversions must take place based on the value to compare
                Type typeIfNullable = Nullable.GetUnderlyingType(member.Type);
                if (typeIfNullable != null)
                {
                    //If the value to compare is not nothing then convert the member to the non nullable version
                    if ((filters[i].ValueToCompare != null))
                    {
                        //Convert the member to his non nullable version
                        member = Expression.Convert(member, typeIfNullable);
                        constant = Expression.Constant(Convert.ChangeType(filters[i].ValueToCompare, typeIfNullable));
                    }
                    else
                    {
                        //If the value to compare is nothing then set the constant to nothing
                        constant = Expression.Constant(null);
                    }
                }
                else
                {
                    constant = Expression.Constant(Convert.ChangeType(filters[i].ValueToCompare, member.Type));
                }

                //Create the correspondent lambda expression according to the current comparison operator
                switch (filters[i].ComparisonType)
                {
                    case FilterOperators.EQUALS:
                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(member, constant), param);
                        break;
                    case FilterOperators.NOTEQUALS:
                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.NotEqual(member, constant), param);
                        break;
                    case FilterOperators.LESSTHAN:
                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.LessThan(member, constant), param);
                        break;
                    case FilterOperators.LESSTHANOREQUALS:
                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.LessThanOrEqual(member, constant), param);
                        break;
                    case FilterOperators.GREATERTHAN:
                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThan(member, constant), param);
                        break;
                    case FilterOperators.GREATERTHANOREQUALS:
                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThanOrEqual(member, constant), param);
                        break;
                    case FilterOperators.LIKE:
                        MethodInfo method = typeof(string).GetMethod("Contains");
                        MethodCallExpression containsMethodExp = Expression.Call(member, method, constant);

                        lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, param);
                        break;
                    case FilterOperators.INCLUDED:
                        //The value to compare is a list, so it must be casted to a list and processed item by item
                        objectsList = (IList)filters[i].ValueToCompare;

                        //Process item by item, but first check if the list holds items to make the first item the first left expression
                        if ((((objectsList != null)) && (objectsList.Count > 0)))
                        {
                            //Create the first left expression
                            constant = Expression.Constant(objectsList[0]);
                            Expression left = Expression.Equal(member, constant);

                            //This expression will hold the cumulative "orelse" expressions
                            Expression orElseExpression = left;

                            //Iterate through the remaining items and construct with every one a right expression to make the "orelse" expression
                            //After the orelse make the "orelse" expression the new left expression
                            for (int j = 1; j <= objectsList.Count - 1; j++)
                            {
                                constant = Expression.Constant(objectsList[j]);
                                Expression right = Expression.Equal(member, constant);

                                orElseExpression = Expression.OrElse(left, right);

                                left = orElseExpression;
                            }

                            lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(orElseExpression, param);
                        }
                        break;
                    case FilterOperators.NOTINCLUDED:
                        //The value to compare is a list, so it must be casted to a list and processed item by item
                        objectsList = (IList)filters[i].ValueToCompare;

                        //Process item by item, but first check if the list holds items to make the first item the first left expression
                        if ((((objectsList != null)) && (objectsList.Count > 0)))
                        {
                            //Create the first left expression
                            constant = Expression.Constant(objectsList[0]);
                            Expression left = Expression.NotEqual(member, constant);

                            //This expression will hold the cumulative "orelse" expressions
                            Expression orElseExpression = left;

                            //Iterate through the remaining items and construct with every one a right expression to make the "notalso" expression
                            //After the notalso make the "notalso" expression the new left expression
                            for (int j = 1; j <= objectsList.Count - 1; j++)
                            {
                                constant = Expression.Constant(objectsList[j]);
                                Expression right = Expression.NotEqual(member, constant);

                                orElseExpression = Expression.AndAlso(left, right);

                                left = orElseExpression;
                            }

                            lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(orElseExpression, param);
                        }
                        break;
                    default:
                        //If the execution reachs here then the operator is invalid
                        throw new DatabaseOperationException("An attempt to execute an invalid operation was found. Please check the service logs for more details.");
                }
                if (finalExpressions == null)
                {
                    finalExpressions = new List<Expression<Func<TEntity, bool>>>()
                                       {
                                           lambdaExpression
                                       };
                }
                else
                {
                    finalExpressions.Add(lambdaExpression);
                }
            }

            return finalExpressions;
        }

        /// <summary>
        /// Creates the expression to make an OrderBy or OrderByDescending in a Linq expression
        /// </summary>
        /// <param name="sortFieldName">Name of the property by whom the sorting will be made</param>
        /// <returns>Expression to make an OrderBy or OrderByDescending in a Linq expression</returns>
        public Expression<Func<TEntity, TResult>> CreateSortExpression<TResult>(string sortFieldName)
        {
            //Create the base parameter expression
            var parameter = Expression.Parameter(typeof(TEntity));

            //Create the member expression with the field name to access the property in TEntity
            var property = Expression.Property(parameter, sortFieldName);

            //Finally, create and return the built lambda expression
            Expression<Func<TEntity, TResult>> sortExpression = Expression.Lambda<Func<TEntity, TResult>>(property, parameter);

            return sortExpression;
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// Variable used to detect redundant invocations
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Main method for the IDisposable interface
        /// </summary>
        /// <param name="disposing">Signals if this objects is already being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //Free al the resources is this is the first call to this method and if this object is being disposed
                    _context.Dispose();
                }
            }
            this.disposedValue = true;
        }

        /// <summary>
        /// Added code to implement the Disposable pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
