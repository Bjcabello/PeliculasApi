using AutoMapper;
using PeliculasApi.Entidades;
using PeliculasApi.DTOs;
namespace PeliculasApi.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeoGeneros();
            ConfigurarMapeoActores();
        }

        private void ConfigurarMapeoGeneros()
        {
            CreateMap<ActorCreacionDTO, Actor>()
            .ForMember(actor => actor.Foto, opciones => opciones.Ignore());

        }

        private void ConfigurarMapeoActores()
        {
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }
    }
}

