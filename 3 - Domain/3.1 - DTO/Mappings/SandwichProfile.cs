using AutoMapper;
using Moviit.Domain.DTO.POCO;
using Moviit.Infrastructure.Data.Entities;
using System.Linq;

namespace Moviit.Domain.DTO.Mappings
{

    /// <summary>
    /// Profile for the automatic mapping between the DTO and the Entity Framework entities from the database
    /// </summary>
    public class SandwichProfile : Profile
    {

        #region Constructor

        /// <summary>
        /// Default constructor. Contains all the mapping definitions
        /// </summary>
        public SandwichProfile()
        {

            /*******************
             * Entities To DTO *
             *******************/

            CreateMap<INGREDIENT, Ingredient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.IngredientName))
                .ForMember(dest => dest.MachineStackIndex, opt => opt.MapFrom(src => src.MachineStackIndex));

            CreateMap<SANDWICH_CUT_TYPE, SandwichCutType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SandwichCutName));

            CreateMap<SANDWICH_TYPE, SandwichType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SandwichTypeName))
                .ForMember(dest => dest.CutType, opt => opt.MapFrom(src => src.SANDWICH_CUT_TYPE))
                .ForMember(dest => dest.HasSalsa, opt => opt.MapFrom(src => src.HasSalsa))
                .ForMember(dest => dest.SalsaInside, opt => opt.MapFrom(src => src.SalsaInside))
                .ForMember(dest => dest.HasCompresssion, opt => opt.MapFrom(src => src.HasCompression));

            CreateMap<SANDWICH, Sandwich>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SandwichName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.SANDWICH_TYPE))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.SANDWICH_INGREDIENT.Select(x => x.INGREDIENT).ToList()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<ORDER_STATE, OrderState>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OrderStateName));

            /*******************
             * DTO to Entities *
             *******************/

            CreateMap<Sandwich, SANDWICH>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SandwichName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SandwichTypeId, opt => opt.MapFrom(src => src.Type.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.SANDWICH_TYPE, opt => opt.Ignore());

            CreateMap<Ingredient, INGREDIENT>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.MachineStackIndex, opt => opt.MapFrom(src => src.MachineStackIndex));

            CreateMap<Order, ORDER>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ORDER_STATE, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.TableNumber))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));

        }

        #endregion

    }
}