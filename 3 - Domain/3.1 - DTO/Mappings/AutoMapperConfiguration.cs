using AutoMapper;

namespace Moviit.Domain.DTO.Mappings
{

    /// <summary>
    /// This class configures and creates the automatic mapping provided by AutoMapper
    /// </summary>
    public class AutoMapperConfiguration
    {

        #region Public Methods

        /// <summary>
        /// Configures and creates the automatic mapping provided by AutoMapper
        /// This method should be called at the beginning of the application that uses the automatic mapping,
        /// i.e., if it is used by a web application must be invoked in the global.asax application_start method;
        /// If it is used by a winform application must be invoked in the initial form load.
        /// </summary>
        /// <remarks>This method should be called at the beginning of the application that uses the automatic mapping</remarks>
        public static MapperConfiguration Configure()
        {
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.AddProfile<SandwichProfile>();
            });

            return config;
        }

        #endregion

    }
}