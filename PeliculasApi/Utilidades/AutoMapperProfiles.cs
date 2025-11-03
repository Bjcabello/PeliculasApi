using AutoMapper;
using PeliculasApi.Entidades;
using PeliculasApi.DTOs;
using NetTopologySuite.Geometries;
namespace PeliculasApi.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            ConfigurarMapeoGeneros();
            ConfigurarMapeoActores();
            ConfigurarMapeoCines(geometryFactory);
        }

        private void ConfigurarMapeoCines(GeometryFactory geometryFactory)
        {
            CreateMap<Cine, CineDTO>()
                .ForMember(x => x.Latitud, cine => cine.MapFrom(p => p.Ubicacion.Y))
                .ForMember(x => x.Longitud, cine => cine.MapFrom(p => p.Ubicacion.X));

            CreateMap<CineCreacionDTO, Cine>()
                .ForMember(x => x.Ubicacion, cineDTO => cineDTO.MapFrom(p =>
                    geometryFactory.CreatePoint(new Coordinate(p.Longitud, p.Latitud))));
        }

        private void ConfigurarMapeoGeneros()
        {
            CreateMap<ActorCreacionDTO, Actor>()
            .ForMember(actor => actor.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();

        }

        private void ConfigurarMapeoActores()
        {
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }
    }
}

