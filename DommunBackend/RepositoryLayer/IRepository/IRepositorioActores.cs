using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;

namespace DommunBackend.RepositoryLayer.IRepository
{
    public interface IRepositorioActores
    {
        Task Actualizar(Actor actor);
        Task Borrar(int id);
        Task<int> Crear(Actor actor);
        Task<bool> Existe(int id);
        Task<List<int>> Existen(List<int> ids);
        Task<Actor?> ObtenerPorId(int id);
        Task<List<Actor>> ObtenerPorNombre(string nombre);
        Task<List<Actor>> ObtenerTodos(PaginacionDto paginacionDto);
    }
}
