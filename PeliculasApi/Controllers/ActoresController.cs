using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Data;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;
using PeliculasApi.Utilidades;

namespace PeliculasApi.Controllers
{
    [Route("api/actores")]
    public class ActoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "actores";
        private readonly string contenedor = "actores";


        public ActoresController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ActorDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Actores;
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable
                .OrderBy(a => a.Nombre)
                .Paginar(paginacion)
                .ProjectTo<ActorDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }


        [HttpGet("{id:int}", Name = "ObtenerActorPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actores
                .ProjectTo<ActorDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null)
            {
                return NotFound();
            }
            return actor;
        }


        [HttpPost()]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, actorCreacionDTO.Foto);
                actor.Foto = url;
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerActorPorId", new { id = actor.Id }, actor);

        }

        [HttpPut("{id: int}")]
        public async Task<IActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);
            if(actor is null)
            { 
                return NotFound(); 
            }
            actor = mapper.Map(actorCreacionDTO, actor);

            if (actorCreacionDTO.Foto is not null)
            {
                actor.Foto = await almacenadorArchivos.Editar(actor.Foto, contenedor, actorCreacionDTO.Foto);


            }
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
    }
}
