using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs
{
    public class RatingCreacionDTO
    {
        public int PeliculaId { get; set; }
        [Range(1, 5)]
        public int Puntuacion { get; set; }

    }
}
