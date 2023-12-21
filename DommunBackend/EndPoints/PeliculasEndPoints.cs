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

namespace DommunBackend.EndPoints
{
    public static class PeliculasEndPoints
    {
        private static readonly string contenedor = "peliculas";

        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerPeliculas)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("peliculas-get"))
                .AgregarParametrosPaginacionOpenAPI();

            group.MapGet("/{id:int}", ObtenerPeliculaPorId);

            group.MapPost("/", CrearPelicula).DisableAntiforgery()
                .AddEndpointFilter<FiltroValidaciones<CrearPeliculaDto>>().RequireAuthorization("esAdmin");

            group.MapPut("/{id:int}", ActualizarPelicula).DisableAntiforgery()
                .AddEndpointFilter<FiltroValidaciones<CrearPeliculaDto>>().RequireAuthorization("esAdmin");

            group.MapDelete("/{id:int}", BorrarPelicula).RequireAuthorization("esAdmin");

            group.MapGet("obtenerPorNombre/{nombre}", ObtenerPorNombre);

            group.MapPost("/{id:int}/asignargeneros", AsignarGeneros);

            group.MapPost("/{id:int}/asignaractores", AsignarActores);

            return group;
        }

        static async Task<Ok<List<PeliculaDto>>> ObtenerPeliculas(IRepositorioPeliculas repositorio,
            IMapper mapper, PaginacionDto paginacion)
        {
            var peliculas = await repositorio.ObtenerTodos(paginacion);
            var peliculasDto = mapper.Map<List<PeliculaDto>>(peliculas);

            return TypedResults.Ok(peliculasDto);
        }

        static async Task<Results<Ok<PeliculaDto>, NotFound>> ObtenerPeliculaPorId(IRepositorioPeliculas repositorio, int id, IMapper mapper)
        {
            var actor = await repositorio.ObtenerPorId(id);

            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDto = mapper.Map<PeliculaDto>(actor);

            return TypedResults.Ok(actorDto);
        }

        static async Task<Created<PeliculaDto>> CrearPelicula([FromForm] CrearPeliculaDto crearPeliculaDto, IRepositorioPeliculas repositorio,
            IOutputCacheStore outputCacheStore, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actor = mapper.Map<Pelicula>(crearPeliculaDto);

            if (crearPeliculaDto.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculaDto.Poster);
                actor.Poster = url;
            }

            var id = await repositorio.Crear(actor);

            await outputCacheStore.EvictByTagAsync("peliculas-get", default);

            var actorDto = mapper.Map<PeliculaDto>(actor);

            return TypedResults.Created($"/pelicula/{id}", actorDto);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarPelicula(int id, [FromForm] CrearPeliculaDto crearPeliculaDto,
            IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actor = mapper.Map<Pelicula>(crearPeliculaDto);
            actor.Id = id;
            actor.Poster = actorDB.Poster;

            if (crearPeliculaDto.Poster is not null)
            {
                var url = await almacenadorArchivos.Editar(actor.Poster, contenedor, crearPeliculaDto.Poster);

                actor.Poster = url;
            }

            await repositorio.Actualizar(actor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarPelicula(int id, IRepositorioPeliculas repositorio,
            IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(actorDB.Poster, contenedor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Ok<List<PeliculaDto>>> ObtenerPorNombre(string nombre, IRepositorioPeliculas repositorio, IMapper mapper)
        {
            var peliculas = await repositorio.ObtenerPorNombre(nombre);
            var peliculasDto = mapper.Map<List<PeliculaDto>>(peliculas);

            return TypedResults.Ok(peliculasDto);
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarGeneros(int id, List<int> generosIds,
            IRepositorioPeliculas repositorioPeliculas, IRepositorioGeneros repositorioGeneros)
        {
            if (!await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var generosExistentes = new List<int>();

            if (generosIds.Count != 0)
            {
                generosExistentes = await repositorioGeneros.Existen(generosIds);
            }

            if (generosExistentes.Count != generosIds.Count)
            {
                var generosNoExistentes = generosIds.Except(generosExistentes);

                return TypedResults.BadRequest($"Los géneros de id {string.Join(",", generosNoExistentes)} no existen.");
            }

            await repositorioPeliculas.AsignarGeneros(id, generosIds);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarActores(int id, List<AsignarActorPeliculaDto> actoresDto,
            IRepositorioPeliculas repositorioPeliculas, IRepositorioActores repositorioActores, IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var actoresExistentes = new List<int>();
            var actoresIds = actoresDto.Select(a => a.ActorId).ToList();

            if (actoresDto.Count != 0)
            {
                actoresExistentes = await repositorioActores.Existen(actoresIds);
            }

            if (actoresExistentes.Count != actoresDto.Count)
            {
                var actoresNoExistentes = actoresIds.Except(actoresExistentes);

                return TypedResults.BadRequest($"Los actores de id {string.Join(",", actoresNoExistentes)} no existen.");
            }

            var actores = mapper.Map<List<ActorPelicula>>(actoresDto);

            await repositorioPeliculas.AsignarActores(id, actores);

            return TypedResults.NoContent();
        }
    }
}
