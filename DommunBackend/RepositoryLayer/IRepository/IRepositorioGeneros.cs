using DommunBackend.DomainLayer.Models;

namespace DommunBackend.RepositoryLayer.IRepository
{
    public interface IRepositorioGeneros
    {
        Task Actualizar(Genero genero);
        Task Borrar(int id);
        Task<int> Crear(Genero genero);
        Task<bool> Existe(int id);
        Task<bool> Existe(int id, string nombre);
        Task<List<int>> Existen(List<int> ids);
        Task<Genero?> ObtenerPorId(int id);
        Task<List<Genero>> ObtenerTodos();
    }
}
