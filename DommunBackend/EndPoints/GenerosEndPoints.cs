using AutoMapper;
using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;
using DommunBackend.Filtros;
using DommunBackend.RepositoryLayer.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace DommunBackend.EndPoints
{
    public static class GenerosEndPoints
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerGeneros)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));
            group.MapGet("/{id:int}", ObtenerGeneroPorId);
            group.MapPost("/", CrearGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDto>>()
                .RequireAuthorization("esAdmin");
            group.MapPut("/{id:int}", ActualizarGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDto>>()
                .RequireAuthorization("esAdmin");
            group.MapDelete("/{id:int}", BorrarGenero).RequireAuthorization("esAdmin");

            return group;
        }

        static async Task<Ok<List<GeneroDto>>> ObtenerGeneros(IRepositorioGeneros repositorio, IMapper mapper, ILoggerFactory loggerFactory)
        {
            var tipo = typeof(GenerosEndPoints);
            var logger = loggerFactory.CreateLogger(tipo.FullName!);            
            var generos = await repositorio.ObtenerTodos();
            var generosDto = mapper.Map<List<GeneroDto>>(generos);

            return TypedResults.Ok(generosDto);
        }

        static async Task<Results<Ok<GeneroDto>, NotFound>> ObtenerGeneroPorId(IRepositorioGeneros repositorio, int id, IMapper mapper)
        {
            var genero = await repositorio.ObtenerPorId(id);

            if (genero is null)
            {
                return TypedResults.NotFound();
            }

            var generoDto = mapper.Map<GeneroDto>(genero);

            return TypedResults.Ok(generoDto);
        }

        static async Task<Results<Created<GeneroDto>, ValidationProblem>> CrearGenero(CrearGeneroDto crearGeneroDto, IRepositorioGeneros repositorio,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var genero = mapper.Map<Genero>(crearGeneroDto);
            genero.IsActive = true;
            genero.CreatedDate = DateTime.Now;

            var id = await repositorio.Crear(genero);

            await outputCacheStore.EvictByTagAsync("generos-get", default);

            var generoDto = mapper.Map<GeneroDto>(genero);

            return TypedResults.Created($"/{id}", generoDto);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> ActualizarGenero(int id, CrearGeneroDto crearGeneroDto,
            IRepositorioGeneros repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var genero = mapper.Map<Genero>(crearGeneroDto);
            genero.Id = id;

            await repositorio.Actualizar(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("generos-get", default);

            return TypedResults.NoContent();
        }
    }
}
