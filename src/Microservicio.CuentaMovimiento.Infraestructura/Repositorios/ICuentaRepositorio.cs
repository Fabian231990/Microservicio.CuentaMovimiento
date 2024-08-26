using Microservicio.CuentaMovimiento.Dominio.Dto;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Interfaz del Repositorio de Cuenta.
    /// </summary>
    public interface ICuentaRepositorio
    {
        /// <summary>
        /// Obtiene todas las cuentas registradas en la base de datos.
        /// </summary>
        /// <returns>Una lista con todas las cuentas en formato DTO.</returns>
        Task<IEnumerable<CuentaDto>> ObtenerTodasAsync();

        /// <summary>
        /// Obtiene una cuenta por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a buscar.</param>
        /// <returns>La cuenta encontrada en formato DTO o null si no existe.</returns>
        Task<CuentaDto> ObtenerPorNumeroCuentaAsync(string numeroCuenta);

        /// <summary>
        /// Crea una nueva cuenta en la base de datos.
        /// </summary>
        /// <param name="cuentaDto">DTO de la cuenta que se va a crear.</param>
        /// <returns>La cuenta creada en formato DTO.</returns>
        Task<CuentaDto> NuevoAsync(CuentaDto cuentaDto);

        /// <summary>
        /// Modifica los datos de una cuenta existente en la base de datos.
        /// </summary>
        /// <param name="cuentaDto">DTO de la cuenta con los datos actualizados.</param>
        /// <returns>La cuenta actualizada en formato DTO.</returns>
        Task<CuentaDto> ModificarAsync(CuentaDto cuentaDto);

        /// <summary>
        /// Elimina una cuenta de la base de datos por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a eliminar.</param>
        /// <returns>Booleano que indica si la eliminacion fue exitosa.</returns>
        Task<bool> EliminarAsync(string numeroCuenta);

        /// <summary>
        /// Cuenta cuantas cuentas tiene un cliente basado en la identificacion de la persona.
        /// </summary>
        /// <param name="identificacion">La identificacion de la persona.</param>
        /// <returns>El numero de cuentas asociadas a la persona.</returns>
        Task<int> ContarCuentasPorIdentificacionAsync(string identificacion);

        /// <summary>
        /// Obtiene todas las cuentas asociadas a un cliente por su identificacion.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente.</param>
        /// <returns>Listado de cuentas asociadas a la identificacion del cliente.</returns>
        Task<IEnumerable<CuentaDto>> ObtenerCuentasPorIdentificacionAsync(string identificacion);
    }
}
