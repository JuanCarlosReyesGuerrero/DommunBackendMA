using AutoMapper;
using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;

namespace DommunBackend.Utilidades
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<CrearGeneroDto, Genero>();
            CreateMap<Genero, GeneroDto>();

            CreateMap<CrearActorDto, Actor>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDto>();

            CreateMap<CrearPeliculaDto, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore());
            CreateMap<Pelicula, PeliculaDto>()
                .ForMember(p => p.Generos, entidad => entidad.MapFrom(p => p.GeneroPeliculas
                .Select(gp => new GeneroDto { Id = gp.GeneroId, Nombre = gp.Genero.Nombre })))
                .ForMember(p => p.Actores, entidad => entidad.MapFrom(p => p.ActoresPeliculas
                .Select(ap => new ActorPeliculaDto { Id = ap.ActorId, Nombre = ap.Actor.Nombre, Personaje = ap.Personaje })));

            CreateMap<CrearComentarioDto, Comentario>();
            CreateMap<Comentario, ComentarioDto>();

            CreateMap<AsignarActorPeliculaDto, ActorPelicula>();
        }
    }
}
