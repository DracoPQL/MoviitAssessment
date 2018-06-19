using AutoMapper;
using Moviit.Domain.Business.Sandwichs.Contracts;
using Moviit.Domain.DTO.POCO;
using Moviit.Infrastructure.Data.Entities;
using Moviit.Infrastructure.Data.Repositories.Contracts;
using Moviit.Infrastructure.Data.Repositories.Implementations;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace Moviit.Domain.Business.Sandwichs.Implementations
{

    /// <summary>
    /// Implementation of the sandwich management
    /// </summary>
    public class SandwichManagement : ISandwichManagement
    {

        #region Variables

        /// <summary>
        /// Context Type to be used to access the database with the UoW
        /// </summary>
        private Type _contextType;

        /// <summary>
        /// Mapper defined to use in this implementation
        /// </summary>
        private IMapper _mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">DbContext instance created in upper layers and injected through dependency injections</param>
        /// <param name="mapper">Mapper instance created in upper layers and injected through dependency injections</param>
        public SandwichManagement(DbContext context, IMapper mapper)
        {
            _contextType = context.GetType();
            _mapper = mapper;
        }

        #endregion

        #region Sandwich Management

        /// <summary>
        /// Creates a new sandwich
        /// </summary>
        /// <param name="sandwich">Sandwich to create</param>
        /// <returns>Id of the new created sandwich</returns>
        public async virtual Task<int> SandwichCreate(Sandwich sandwich)
        {
            //Map the DTO to entities
            SANDWICH sandwichBase = _mapper.Map<SANDWICH>(sandwich);

            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Get a new sequence's value
                sandwichBase.Id = uow.GetNextValInSequence<int>("SANDWICH_SEQUENCE");

                //Save the data
                uow.SandwichRepository.Create(sandwichBase);

                int sandwichId = sandwichBase.Id;

                foreach (Ingredient ingredient in sandwich.Ingredients)
                {
                    SANDWICH_INGREDIENT data = new SANDWICH_INGREDIENT()
                    {
                        Id = uow.GetNextValInSequence<int>("SANDWICH_INGREDIENT_SEQUENCE"),
                        IngredientId = ingredient.Id,
                        SandwichId = sandwichId
                    };
                    
                    uow.SandwichIngredientRepository.Create(data);
                }

                //Commit the changes
                await uow.CommitAsync();
            }

            return sandwichBase.Id;
        }

        /// <summary>
        /// Returns a sandwich given his id
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to return</param>
        /// <returns>The specified sandwich</returns>
        public async virtual Task<Sandwich> SandwichRead(int sandwichId)
        {
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Query the data and the necessary inclusions
                var query = (from o in uow.SandwichRepository.AsQueryable()
                             where o.Id == sandwichId
                             select o)
                                .Include(x => x.SANDWICH_INGREDIENT.Select(y => y.INGREDIENT))
                                .Include(x => x.SANDWICH_TYPE)
                                .Include(x => x.SANDWICH_TYPE.SANDWICH_CUT_TYPE);
                var data = await query.FirstOrDefaultAsync();

                //Return the mapped DTO
                return _mapper.Map<Sandwich>(data);
            }
        }

        /// <summary>
        /// Updates a sandwich
        /// </summary>
        /// <param name="sandwich">Sandwich to update</param>
        /// <returns>Task for async operations</returns>
        public async virtual Task SandwichUpdate(Sandwich sandwich)
        {
            //Map the DTO to entities
            SANDWICH receivedData = _mapper.Map<SANDWICH>(sandwich);

            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Update the data
                uow.SandwichRepository.Update(receivedData);

                //Commit the changes
                await uow.CommitAsync();
            }
        }

        /// <summary>
        /// Deletes a sandwich
        /// </summary>
        /// <param name="sandwichId">Id of the sandwich to delete</param>
        /// <returns>Task for async operations</returns>
        public async virtual Task SandwichDelete(int sandwichId)
        {
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Delete the dependant items
                var query = from o in uow.SandwichIngredientRepository.AsQueryable()
                            where o.SandwichId == sandwichId
                            select o.Id;

                IList<int> ids = await query.ToListAsync();
                foreach (int id in ids)
                {
                    uow.SandwichIngredientRepository.DeleteById(id);
                }

                //Delete the item
                uow.SandwichRepository.DeleteById(sandwichId);

                //Commit the changes
                await uow.CommitAsync();
            }
        }

        /// <summary>
        /// Lists the sandwichs
        /// </summary>
        /// <returns>All the sandwichs</returns>
        public async virtual Task<IEnumerable<Sandwich>> SandwichList()
        {
            //Query the data and the necessary inclusions
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                var query = (from o in uow.SandwichRepository.AsQueryable()
                             select o)
                                .Include(x => x.SANDWICH_INGREDIENT.Select(y => y.INGREDIENT))
                                .Include(x => x.SANDWICH_TYPE)
                                .Include(x => x.SANDWICH_TYPE.SANDWICH_CUT_TYPE);
                var data = await query.ToListAsync();

                //Return the mapped DTOs
                return _mapper.Map<IEnumerable<Sandwich>>(data);
            }
        }

        #endregion

        #region Ingredient Management

        /// <summary>
        /// Creates a new ingredient
        /// </summary>
        /// <param name="ingredient">Ingredient to create</param>
        /// <returns>Id of the new created ingredient</returns>
        public async virtual Task<int> IngredientCreate(Ingredient ingredient)
        {
            //Map the DTO to entities
            INGREDIENT ingredientBase = _mapper.Map<INGREDIENT>(ingredient);

            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Get a new sequence's value
                ingredientBase.Id = uow.GetNextValInSequence<int>("INGREDIENT_SEQUENCE");

                //Save the data
                uow.IngredientRepository.Create(ingredientBase);

                //Commit the changes
                await uow.CommitAsync();
            }

            return ingredientBase.Id;
        }

        /// <summary>
        /// Returns an ingredient given his id
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to return</param>
        /// <returns>The specified ingredient</returns>
        public async virtual Task<Ingredient> IngredientRead(int ingredientId)
        {
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Query the data
                var query = from o in uow.IngredientRepository.AsQueryable()
                            where o.Id == ingredientId
                            select o;
                var data = await query.FirstOrDefaultAsync();

                //Return the mapped DTO
                return _mapper.Map<Ingredient>(data);
            }
        }

        /// <summary>
        /// Updates an ingredient
        /// </summary>
        /// <param name="ingredient">Ingredient to update</param>
        /// <returns>Task for async operations</returns>
        public async virtual Task IngredientUpdate(Ingredient ingredient)
        {
            //Map the DTO to entities
            INGREDIENT receivedData = _mapper.Map<INGREDIENT>(ingredient);

            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Update the data
                uow.IngredientRepository.Update(receivedData);

                //Commit the changes
                await uow.CommitAsync();
            }
        }

        /// <summary>
        /// Deletes an ingredient
        /// </summary>
        /// <param name="ingredientId">Id of the ingredient to delete</param>
        /// <returns>Task for async operations</returns>
        public async virtual Task IngredientDelete(int ingredientId)
        {
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Delete the item
                uow.IngredientRepository.DeleteById(ingredientId);

                //Commit the changes
                await uow.CommitAsync();
            }
        }

        /// <summary>
        /// Lists the ingredients
        /// </summary>
        /// <returns>All the ingredients</returns>
        public async virtual Task<IEnumerable<Ingredient>> IngredientList()
        {
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                //Query the data
                var query = from o in uow.IngredientRepository.AsQueryable()
                            select o;
                var data = await query.ToListAsync();

                //Return the mapped DTOs
                return _mapper.Map<IEnumerable<Ingredient>>(data);
            }
        }

        #endregion

        #region Reports

        /// <summary>
        /// Get the sandwichs sold on the last seven days with their quantities.
        /// </summary>
        /// <returns>The report data</returns>
        public async virtual Task<IEnumerable<TheOnlyReportDTO>> TheOnlyReport()
        {
            using (IUnitOfWork uow = new UnitOfWork((DbContext)Activator.CreateInstance(_contextType)))
            {
                DateTime iniDate = DateTime.Now.AddDays(-7);

                //query the report data
                var query = from o in uow.OrderRepository.AsQueryable()
                            join os in uow.OrderSandwichRepository.AsQueryable() on o.Id equals os.OrderId
                            where o.CreationDate >= iniDate
                            group os.Quantity by new { os.SandwichId, Date = DbFunctions.TruncateTime(o.CreationDate) } into g
                            select new { SandwichId = g.Key.SandwichId, CreationDate = g.Key.Date, Quantity = g.Sum(x => x) };

                var data = await query.ToListAsync();

                //Construct and return the report DTO
                List<TheOnlyReportDTO> list = new List<TheOnlyReportDTO>();
                foreach (var d in data)
                {
                    list.Add(new TheOnlyReportDTO()
                    {
                        SandwichName = (await SandwichRead(d.SandwichId)).Name,
                        Quantity = d.Quantity,
                        OrderDate = d.CreationDate.Value
                    });
                }

                return list;
            }
        }

        #endregion

    }
}
