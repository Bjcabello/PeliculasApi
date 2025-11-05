using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PeliculasApi.Data;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController: CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "peliculas";
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IMapper mapper,
            IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
            :base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("Landing")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            var proximoEstrenos = await context.Peliculas
                .Where(p => p.FechaLanzamiento > hoy)
                .OrderBy(p => p.FechaLanzamiento)
                .Take(top)
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var enCines = await context.Peliculas
                .Where(p => p.PeliculaCines.Select(pc => pc.PeliculaId).Contains(p.Id))
                .OrderBy(p => p.FechaLanzamiento)
                .Take(top)
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var resultado = new LandingPageDTO();
            resultado.EnCines = enCines;
            resultado.ProximosEstrenos = proximoEstrenos;
            return resultado;
        }

        [HttpGet("{id:int}", Name = "ObtenerPeliculaPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<PeliculaDetallesDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDetallesDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }
            return pelicula;
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
            var generos = await context.Generos.ProjectTo<GeneroDTO>(mapper.ConfigurationProvider).ToListAsync();

            return new PeliculasPostGetDTO
            {
                Cines = cines,
                Generos = generos
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster is not null) 
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, peliculaCreacionDTO.Poster);
                pelicula.Poster = url;
            }
            AsignarOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return CreatedAtRoute("ObtenerPeliculaPorId", new {id = pelicula.Id}, peliculaDTO);
        }
        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculasPutGetDTO>> PutGet(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDetallesDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula is null)
            {
                return NotFound();
            }

            var generosSeleccionadosIds = pelicula.Generos.Select(g => g.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                  .Where(g => !generosSeleccionadosIds.Contains(g.Id))
                  .ProjectTo<GeneroDTO>(mapper.ConfigurationProvider)
                  .ToListAsync();

            var cinesSeleccionadosIds = pelicula.Cines.Select(c => c.Id).ToList();
            var cinesNoSeleccionados = await context.Cines
                .Where(c => !cinesSeleccionadosIds.Contains(c.Id))
                .ProjectTo<CineDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var respuesta = new PeliculasPutGetDTO();
            respuesta.Pelicula = pelicula;
            respuesta.GenerosSeleccionados = pelicula.Generos;
            respuesta.GenerosNoSeleccionados = generosNoSeleccionados;
            respuesta.CinesSeleccionados = pelicula.Cines;
            respuesta.CinesNoSeleccionados = cinesNoSeleccionados;
            respuesta.Actores = pelicula.Actores;
            return respuesta;

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = await context.Peliculas
                .Include(p => p.PeliculaActores)
                .Include(p => p.PeliculaCines)
                .Include(p => p.PeliculaGeneros)
                .FirstOrDefaultAsync(P => P.Id == id);

            if(pelicula is null)
            {
                return NotFound();
            }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);

            if (peliculaCreacionDTO.Poster is not null)
            {
                pelicula.Poster = await almacenadorArchivos.Editar(pelicula.Poster,
                    contenedor, peliculaCreacionDTO.Poster);
            }
            AsignarOrdenActores(pelicula);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculaActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculaActores.Count; i++)
                {
                    pelicula.PeliculaActores[i].Orden = i;
                }
            }
        }
    }
}
