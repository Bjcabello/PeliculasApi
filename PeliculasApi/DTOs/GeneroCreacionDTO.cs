using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; }
    }
}
