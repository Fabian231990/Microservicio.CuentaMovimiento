using Microservicio.CuentaMovimiento.Dominio.Entidades;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Define las operaciones CRUD para la entidad Cuenta.
    /// </summary>
    public interface ICuentaRepositorio
    {
        /// <summary>
        /// Obtiene todas las cuentas.
        /// </summary>
        /// <returns>Listado de cuentas.</returns>
        Task<IEnumerable<CuentaEntidad>> ObtenerTodasAsync();

        /// <summary>
        /// Obtiene una cuenta por su ID.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Entidad Cuenta encontrada.</returns>
        Task<CuentaEntidad> ObtenerPorNumeroCuentaAsync(string numeroCuenta);

        /// <summary>
        /// Cuenta el número de cuentas asociadas a un cliente específico.
        /// </summary>
        /// <param name="idCliente">ID del cliente.</param>
        /// <returns>Número de cuentas asociadas al cliente.</returns>
        Task<int> ContarCuentasPorClienteAsync(int idCliente);

        /// <summary>
        /// Obtiene una cuenta asociada a un cliente específico y un tipo de cuenta específico.
        /// </summary>
        /// <param name="idCliente">ID del cliente.</param>
        /// <param name="tipoCuenta">Tipo de cuenta (Ahorro, Corriente, etc.).</param>
        /// <returns>CuentaEntidad correspondiente o null si no se encuentra ninguna.</returns>
        Task<CuentaEntidad> ObtenerPorClienteYTipoCuentaAsync(int idCliente, string tipoCuenta);

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta a crear.</param>
        Task NuevoAsync(CuentaEntidad cuentaEntidad);

        /// <summary>
        /// Modifica una cuenta existente.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta con los datos actualizados.</param>
        Task ModificarAsync(CuentaEntidad cuentaEntidad);

        /// <summary>
        /// Elimina una cuenta por su ID.
        /// </summary>
        /// <param name="idCuenta">Identificador de la cuenta a eliminar.</param>
        Task EliminarAsync(int idCuenta);
    }
}
