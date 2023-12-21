using AutoMapper;
using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;
using DommunBackend.Filtros;
using DommunBackend.RepositoryLayer.IRepository;
using DommunBackend.RepositoryLayer.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace DommunBackend.EndPoints
{
    public static class ComentariosEndPoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerComentarios)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60))
                .Tag("comentarios-get")
                .SetVaryByRouteValue(new string[] { "peliculaId" }));
            group.MapGet("/{id:int}", ObtenerComentarioPorId);
            group.MapPost("/", CrearComentario).AddEndpointFilter<FiltroValidaciones<CrearComentarioDto>>();
            group.MapPut("/{id:int}", ActualizarComentario).AddEndpointFilter<FiltroValidaciones<CrearComentarioDto>>();
            group.MapDelete("/{id:int}", BorrarComentario);

            return group;
        }

        static async Task<Results<Ok<List<ComentarioDto>>, NotFound>> ObtenerComentarios(int peliculaId, IRepositorioComentarios repositorio,
            IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentarios = await repositorio.ObtenerTodos(peliculaId);
            var comentariosDto = mapper.Map<List<ComentarioDto>>(comentarios);

            return TypedResults.Ok(comentariosDto);
        }

        static async Task<Results<Ok<ComentarioDto>, NotFound>> ObtenerComentarioPorId(int peliculaId, int id,
            IRepositorioComentarios repositorio, IMapper mapper)
        {
            var comentario = await repositorio.ObtenerPorId(id);

            if (comentario is null)
            {
                return TypedResults.NotFound();
            }

            var comentarioDto = mapper.Map<ComentarioDto>(comentario);

            return TypedResults.Ok(comentarioDto);
        }

        static async Task<Results<Created<ComentarioDto>, NotFound>> CrearComentario(int peliculaId, CrearComentarioDto crearComentarioDto,
            IRepositorioComentarios repositorio, IRepositorioPeliculas repositorioPeliculas, IOutputCacheStore outputCacheStore,
            IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDto);
            comentario.PeliculaId = peliculaId;
            var id = await repositorio.Crear(comentario);

            await outputCacheStore.EvictByTagAsync("comentarios-get", default);

            var comentarioDto = mapper.Map<ComentarioDto>(comentario);

            return TypedResults.Created($"/comentario/{id}", comentarioDto);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarComentario(int peliculaId, int id, CrearComentarioDto crearComentarioDto,
            IRepositorioComentarios repositorio, IRepositorioPeliculas repositorioPeliculas, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            if (!await repositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDto);
            comentario.Id = id;
            comentario.PeliculaId = peliculaId;

            await repositorio.Actualizar(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarComentario(int id, IRepositorioComentarios repositorio, IOutputCacheStore outputCacheStore)
        {
            if (!await repositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);

            return TypedResults.NoContent();
        }
    }
}
