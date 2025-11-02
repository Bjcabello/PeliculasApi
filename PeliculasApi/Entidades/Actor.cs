using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Actor: IId
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        public required string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [Unicode(false)]
        public string? Foto { get; set; }
    }
}
