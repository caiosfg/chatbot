using AutoMapper;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;

namespace RangoAgil.API.Profiles;

public class RangoAgilProfile : Profile
{
    public RangoAgilProfile()
    {
        CreateMap<Rango, RangoDTO>().ReverseMap();
        CreateMap<Rango, RangoParaCriacaoDTO>().ReverseMap();
        CreateMap<Ingrediente, IngredienteDTO>()
            .ForMember(
                ingredient => ingredient.RangoId,
                o => o.MapFrom(src => src.Rangos.First().Id)
            );
    }
}
