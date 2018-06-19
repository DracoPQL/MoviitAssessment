using AutoMapper;
using Moviit.Domain.Business.Sandwichs.Contracts;
using System;
using System.Data.Entity;

namespace Moviit.Domain.Business.Sandwichs.Implementations
{

    /// <summary>
    /// Implementation of the order management
    /// </summary>
    public class OrderOps : IOrderOps
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
        public OrderOps(DbContext context, IMapper mapper)
        {
            _contextType = context.GetType();
            _mapper = mapper;
        }

        #endregion

    }
}