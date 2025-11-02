using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;
using PeliculasApi.Entidades;

namespace PeliculasApi.DTOs
{
    public class GeneroDTO: IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

    }
}
