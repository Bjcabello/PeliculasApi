using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs
{
    public class PeliculaCreacionDTO
    {
        
        [Required]
        [StringLength(300)]
        public required string Titulo { get; set; }
        public string? Trailer { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        public IFormFile? Poster { get; set; }
    }
}
