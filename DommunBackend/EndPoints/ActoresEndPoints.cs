using AutoMapper;
using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;
using DommunBackend.Filtros;
using DommunBackend.RepositoryLayer.IRepository;
using DommunBackend.ServiceLayer.IService;
using DommunBackend.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.OpenApi.Models;

namespace DommunBackend.EndPoints
{
    public static class ActoresEndPoints
    {
        private static readonly string contenedor = "actores";

        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerActores)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"))
                .AgregarParametrosPaginacionOpenAPI();

            group.MapGet("/{id:int}", ObtenerActorPorId);
            group.MapPost("/", CrearActor).DisableAntiforgery()
                .AddEndpointFilter<FiltroValidaciones<CrearActorDto>>()
                .RequireAuthorization("esAdmin");
            group.MapPut("/{id:int}", ActualizarActor).DisableAntiforgery()
                .AddEndpointFilter<FiltroValidaciones<CrearActorDto>>()
                .RequireAuthorization("esAdmin");
            group.MapDelete("/{id:int}", BorrarActor).RequireAuthorization("esAdmin");
            group.MapGet("obtenerPorNombre/{nombre}", ObtenerPorNombre);

            return group;
        }

        static async Task<Ok<List<ActorDto>>> ObtenerActores(IRepositorioActores repositorio,
            IMapper mapper, PaginacionDto paginacion)
        {
            //var paginacion = new PaginacionDto { Pagina = pagina, RegistrosPorPagina = recorsPorpagina };
            var actores = await repositorio.ObtenerTodos(paginacion);
            var actoresDto = mapper.Map<List<ActorDto>>(actores);

            return TypedResults.Ok(actoresDto);
        }

        static async Task<Results<Ok<ActorDto>, NotFound>> ObtenerActorPorId(IRepositorioActores repositorio, int id, IMapper mapper)
        {
            var actor = await repositorio.ObtenerPorId(id);

            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDto = mapper.Map<ActorDto>(actor);

            return TypedResults.Ok(actorDto);
        }

        static async Task<Results<Created<ActorDto>, ValidationProblem>> CrearActor([FromForm] CrearActorDto crearActorDto,
            IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actor = mapper.Map<Actor>(crearActorDto);

            if (crearActorDto.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDto.Foto);
                actor.Foto = url;
            }

            var id = await repositorio.Crear(actor);

            await outputCacheStore.EvictByTagAsync("actores-get", default);

            var actorDto = mapper.Map<ActorDto>(actor);

            return TypedResults.Created($"/{id}", actorDto);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarActor(int id, [FromForm] CrearActorDto crearActorDto,
            IRepositorioActores repositorio, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actor = mapper.Map<Actor>(crearActorDto);
            actor.Id = id;
            actor.Foto = actorDB.Foto;

            if (crearActorDto.Foto is not null)
            {
                var url = await almacenadorArchivos.Editar(actor.Foto, contenedor, crearActorDto.Foto);

                actor.Foto = url;
            }

            await repositorio.Actualizar(actor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarActor(int id, IRepositorioActores repositorio,
            IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(actorDB.Foto, contenedor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Ok<List<ActorDto>>> ObtenerPorNombre(string nombre, IRepositorioActores repositorio, IMapper mapper)
        {
            var actores = await repositorio.ObtenerPorNombre(nombre);
            var actoresDto = mapper.Map<List<ActorDto>>(actores);

            return TypedResults.Ok(actoresDto);
        }
    }
}
