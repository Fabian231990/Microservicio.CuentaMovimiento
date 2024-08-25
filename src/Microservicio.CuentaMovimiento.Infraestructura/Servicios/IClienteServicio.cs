using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Infraestructura.Servicios
{
    public interface IClienteServicio
    {
        /// <summary>
        /// Llama al microservicio de Clientes para obtener información de un cliente por ID.
        /// </summary>
        /// <param name="clienteId">ID del cliente a consultar.</param>
        /// <returns>Contenido de la respuesta.</returns>
        Task<Respuesta<ClienteEntidad>> ObtenerClientePorIdAsync(int clienteId);
    }
}
