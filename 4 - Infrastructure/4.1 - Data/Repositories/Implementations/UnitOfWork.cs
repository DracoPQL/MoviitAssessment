using Moviit.Infrastructure.Data.Repositories.Contracts;
using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using Moviit.Infrastructure.Data.Entities;

namespace Moviit.Infrastructure.Data.Repositories.Implementations
{

    /// <summary>
    /// Implementation of the Unit of Work pattern
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        #region Variables

        private GenericRepository<INGREDIENT> _ingredientRepository;
        private GenericRepository<ORDER> _orderRepository;
        private GenericRepository<ORDER_SANDWICH> _orderSandwichRepository;
        private GenericRepository<ORDER_STATE> _orderStateRepository;
        private GenericRepository<SANDWICH> _sandwichRepository;
        private GenericRepository<SANDWICH_CUT_TYPE> _sandwichCutTypeRepository;
        private GenericRepository<SANDWICH_INGREDIENT> _sandwichIngredientRepository;
        private GenericRepository<SANDWICH_TYPE> _sandwichTypeRepository;

        private DbContext _context;

        #endregion

        #region Properties

        /// <summary>
        /// Ingredient repository
        /// </summary>
        public IGenericRepository<INGREDIENT> IngredientRepository
        {
            get
            {
                if (_ingredientRepository == null)
                {
                    _ingredientRepository = new GenericRepository<INGREDIENT>(_context);
                }
                return _ingredientRepository;
            }
        }

        /// <summary>
        /// Order repository
        /// </summary>
        public IGenericRepository<ORDER> OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new GenericRepository<ORDER>(_context);
                }
                return _orderRepository;
            }
        }

        /// <summary>
        /// Order Sandwich repository
        /// </summary>
        public IGenericRepository<ORDER_SANDWICH> OrderSandwichRepository
        {
            get
            {
                if (_orderSandwichRepository == null)
                {
                    _orderSandwichRepository = new GenericRepository<ORDER_SANDWICH>(_context);
                }
                return _orderSandwichRepository;
            }
        }

        /// <summary>
        /// Order State repository
        /// </summary>
        public IGenericRepository<ORDER_STATE> OrderStateRepository
        {
            get
            {
                if (_orderStateRepository == null)
                {
                    _orderStateRepository = new GenericRepository<ORDER_STATE>(_context);
                }
                return _orderStateRepository;
            }
        }

        /// <summary>
        /// Sandwich repository
        /// </summary>
        public IGenericRepository<SANDWICH> SandwichRepository
        {
            get
            {
                if (_sandwichRepository == null)
                {
                    _sandwichRepository = new GenericRepository<SANDWICH>(_context);
                }
                return _sandwichRepository;
            }
        }

        /// <summary>
        /// Sandwich Cut Type repository
        /// </summary>
        public IGenericRepository<SANDWICH_CUT_TYPE> SandwichCutTypeRepository
        {
            get
            {
                if (_sandwichCutTypeRepository == null)
                {
                    _sandwichCutTypeRepository = new GenericRepository<SANDWICH_CUT_TYPE>(_context);
                }
                return _sandwichCutTypeRepository;
            }
        }

        /// <summary>
        /// Sandwich Ingredient repository
        /// </summary>
        public IGenericRepository<SANDWICH_INGREDIENT> SandwichIngredientRepository
        {
            get
            {
                if (_sandwichIngredientRepository == null)
                {
                    _sandwichIngredientRepository = new GenericRepository<SANDWICH_INGREDIENT>(_context);
                }
                return _sandwichIngredientRepository;
            }
        }

        /// <summary>
        /// Sandwich Type repository
        /// </summary>
        public IGenericRepository<SANDWICH_TYPE> SandwichTypeRepository
        {
            get
            {
                if (_sandwichTypeRepository == null)
                {
                    _sandwichTypeRepository = new GenericRepository<SANDWICH_TYPE>(_context);
                }
                return _sandwichTypeRepository;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Context to be used in the pattern implementation</param>
        public UnitOfWork(DbContext context)
        {
            _context = context;

            //Disable the Lazy Loading to make possible and reasonable the entity management
            _context.Configuration.LazyLoadingEnabled = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the database context used for this UoW
        /// </summary>
        /// <returns>The database context used for this UoW</returns>
        public DbContext GetContext()
        {
            return _context;
        }

        /// <summary>
        /// Returns the next value from a sequence.
        /// </summary>
        /// <param name="sequenceName">Name of the sequence to get the next value from</param>
        /// <returns>The next value from the sequence</returns>
        public T GetNextValInSequence<T>(string sequenceName)
        {
            try
            {
                return _context.Database.SqlQuery<T>(string.Format("SELECT NEXT VALUE FOR {0}", sequenceName)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //A general exception occurred so it will be encapsulated with the corresponding code so the upper layers can handle it properly
                throw new DatabaseValidationException(ex, "Error getting a new sequence value for sequence {0}.", sequenceName);
            }
        }

        /// <summary>
        /// Commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then an exception is thrown.
        /// </summary>
        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //The save was aborted because validation of entity property values failed.
                RollbackChanges();

                //Process the entity validation errors
                string validationErrors = string.Empty;
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    validationErrors += string.Format("Entity of type {0} in state {1} has the following validation errors:{2}",
                                                     validationResult.Entry.Entity.GetType().Name,
                                                     validationResult.Entry.State,
                                                     Environment.NewLine);

                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        validationErrors += string.Format("Property: {0}, Error: {1}{2}",
                                                          error.PropertyName,
                                                          error.ErrorMessage,
                                                          Environment.NewLine);
                    }
                }

                //Encapsulate the original exception inside an exception that will be encapsulated later.
                //This is made this way so the validation details can be logged but only a reduced message is made visible to the upper layers
                DatabaseValidationException outerEx = new DatabaseValidationException(ex, validationErrors);

                throw new DatabaseValidationException(outerEx, "A validation error occurred trying to save the changes to the database.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A database command did not affect the expected number of rows. This usually indicates an optimistic concurrency violation; that is, a row has been changed in the database since it was queried.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseOperationException(ex, "An error occurred saving changes to the database.");
            }
            catch (DbUpdateException ex)
            {
                //An error occurred sending updates to the database.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseOperationException(ex, "An error occurred saving changes to the database.");
            }
            catch (ObjectDisposedException ex)
            {
                //The context or connection have been disposed.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseConnectionException(ex, "An error occurred trying to connect to the database.");
            }
            catch (InvalidOperationException ex)
            {
                //Some error occurred attempting to process entities in the context either before or after sending commands to the database.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseOperationException(ex, "An error occurred saving changes to the database because an invalid operation was attempted.");
            }
            catch (NotSupportedException ex)
            {
                //An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently on the same context instance.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseGenericException(ex, "An error occurred saving changes to the database because an unsupported behavior was attempted.");
            }
            catch (EntityException ex)
            {
                //Provider exception
                RollbackChanges();

                if (ex.Message == "The underlying provider failed on Open.")
                {
                    //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                    throw new DatabaseConnectionException(ex, "An error occurred trying to connect to the database.");
                }
                else
                {
                    //A general exception occurred so it will be encapsulated with the corresponding code so the upper layers can handle it properly
                    throw new DatabaseGenericException(ex, "An unexpected error occurred saving changes to the database.");
                }
            }
            catch (Exception ex)
            {
                //Generic exception probably not from Entity Framework
                RollbackChanges();

                //A general exception occurred so it will be encapsulated with the corresponding code so the upper layers can handle it properly
                throw new DatabaseGenericException(ex, "An unexpected error occurred saving changes to the database.");
            }
        }

        /// <summary>
        /// Asynchronously commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then an exception is thrown.
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                await Task.Run(() => _context.SaveChangesAsync()).ConfigureAwait(false);
            }
            catch (DbEntityValidationException ex)
            {
                //The save was aborted because validation of entity property values failed.
                RollbackChanges();

                //Process the entity validation errors
                string validationErrors = string.Empty;
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    validationErrors += string.Format("Entity of type {0} in state {1} has the following validation errors:{2}",
                                                     validationResult.Entry.Entity.GetType().Name,
                                                     validationResult.Entry.State,
                                                     Environment.NewLine);

                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        validationErrors += string.Format("Property: {0}, Error: {1}{2}",
                                                          error.PropertyName,
                                                          error.ErrorMessage,
                                                          Environment.NewLine);
                    }
                }

                //Encapsulate the original exception inside an exception that will be encapsulated later.
                //This is made this way so the validation details can be logged but only a reduced message is made visible to the upper layers
                DatabaseValidationException outerEx = new DatabaseValidationException(ex, validationErrors);

                throw new DatabaseValidationException(outerEx, "A validation error occurred trying to save the changes to the database.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A database command did not affect the expected number of rows. This usually indicates an optimistic concurrency violation; that is, a row has been changed in the database since it was queried.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseOperationException(ex, "An error occurred saving changes to the database.");
            }
            catch (DbUpdateException ex)
            {
                //An error occurred sending updates to the database.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseOperationException(ex, "An error occurred saving changes to the database.");
            }
            catch (ObjectDisposedException ex)
            {
                //The context or connection have been disposed.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseConnectionException(ex, "An error occurred trying to connect to the database.");
            }
            catch (InvalidOperationException ex)
            {
                //Some error occurred attempting to process entities in the context either before or after sending commands to the database.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseOperationException(ex, "An error occurred saving changes to the database because an invalid operation was attempted.");
            }
            catch (NotSupportedException ex)
            {
                //An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently on the same context instance.
                RollbackChanges();

                //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                throw new DatabaseGenericException(ex, "An error occurred saving changes to the database because an unsupported behavior was attempted.");
            }
            catch (EntityException ex)
            {
                //Provider exception
                RollbackChanges();

                if (ex.Message == "The underlying provider failed on Open.")
                {
                    //Throw an appropiate exception so the upper layers can handle it properly and the log will store more semantic data
                    throw new DatabaseConnectionException(ex, "An error occurred trying to connect to the database.");
                }
                else
                {
                    //A general exception occurred so it will be encapsulated with the corresponding code so the upper layers can handle it properly
                    throw new DatabaseGenericException(ex, "An unexpected error occurred saving changes to the database.");
                }
            }
            catch (Exception ex)
            {
                //Generic exception probably not from Entity Framework
                RollbackChanges();

                //A general exception occurred so it will be encapsulated with the corresponding code so the upper layers can handle it properly
                throw new DatabaseGenericException(ex, "An unexpected error occurred saving changes to the database.");
            }
        }

        /// <summary>
        /// Commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then the changes in the client are refreshed.
        /// </summary>
        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    _context.SaveChanges();

                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException)
                {
                    saveFailed = true;
                }
            } while (!saveFailed);
        }

        /// <summary>
        /// Asynchronously commits the changes made on the context.
        /// If the entity has fixed properties and there are optimistic conncurrency problems then the changes in the client are refreshed.
        /// </summary>
        public async Task CommitAndRefreshChangesAsync()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    await Task.Run(() => _context.SaveChangesAsync()).ConfigureAwait(true);

                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException)
                {
                    saveFailed = true;
                }
            } while (saveFailed);
        }

        /// <summary>
        /// Rollback the changes made on the context that were not commited to the persistency layer.
        /// </summary>
        public void RollbackChanges()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        #endregion

        #region IDisposable implementation

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
                    if (_context != null)
                    {
                        //Free al the resources is this is the first call to this method and if this object is being disposed
                        _context.Dispose();
                    }
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
