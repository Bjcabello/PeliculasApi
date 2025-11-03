using PeliculasApi.Entidades;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs
{
    public class CineDTO: IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        
    }
}
