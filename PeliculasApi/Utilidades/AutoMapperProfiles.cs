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
        }

        private void ConfigurarMapeoGeneros()
        {
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }
    }
}
