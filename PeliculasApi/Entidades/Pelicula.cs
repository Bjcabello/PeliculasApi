using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using System.ComponentModel.DataAnnotations;
namespace PeliculasApi.Entidades
{
    public class Pelicula: IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public required string Titulo { get; set; }
        public string? Trailer {  get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        [Unicode(false)]
        public string? Poster {  get; set; }
        public List<PeliculaGenero> PeliculaGeneros { get; set; } = new List<PeliculaGenero>();
        public List<PeliculaCine> PeliculaCines { get; set; } = new List<PeliculaCine>();
        public List<PeliculaActor> PeliculaActores { get; set; } = new List<PeliculaActor>();
    }
}
