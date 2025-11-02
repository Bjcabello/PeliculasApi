using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Cine
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }
        public required Point Ubicacion { get; set; }

    }
}
