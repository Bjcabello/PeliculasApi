using System.ComponentModel.DataAnnotations;
using PeliculasApi.Validaciones;

namespace PeliculasApi.Entidades
{
    public class Genero: IId
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; }
       
    }
}
