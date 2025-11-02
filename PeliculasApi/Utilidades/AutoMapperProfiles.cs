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

