using DommunBackend.ServiceLayer.IService;

namespace DommunBackend.ServiceLayer.Service
{
    public class AlmacenadorArchivos : IAlmacenadorArchivos
    {
        public Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            throw new NotImplementedException();
        }

        public Task Borrar(string? ruta, string contenedor)
        {
            throw new NotImplementedException();
        }
    }
}
