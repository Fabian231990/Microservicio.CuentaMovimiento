using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Infraestructura.Servicios
{
    public interface IClienteServicio
    {
        /// <summary>
        /// Llama al microservicio de Clientes para obtener informacion de un cliente por identificacion.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente a consultar.</param>
        /// <returns>Respuesta que contiene la informacion del cliente como DTO.</returns>
        Task<Respuesta<ClienteDto>> ObtenerClientePorIdentificacionAsync(string identificacion);
    }
}
