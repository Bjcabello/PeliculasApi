using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Data;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingsController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;

        public RatingsController(ApplicationDbContext context, IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] RatingCreacionDTO ratingCreacionDTO)
        {
            var usuarioId = await servicioUsuarios.ObtenerUsuarioId();

            var ratingActual = await context.RatingsPelicula
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingCreacionDTO.PeliculaId && x.UsuarioId == usuarioId);

            if (ratingActual is null) 
            {
                var rating = new Rating()
                {
                    PeliculaId = ratingCreacionDTO.PeliculaId,
                    Puntuacion = ratingCreacionDTO.Puntuacion,
                    UsuarioId = usuarioId

                };
                context.Add(rating);
            }
            else
            {
                ratingActual.Puntuacion = ratingCreacionDTO.Puntuacion;
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
