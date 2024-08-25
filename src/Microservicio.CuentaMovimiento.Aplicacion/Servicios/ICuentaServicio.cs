using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio que maneja la lógica de negocio para la entidad Cuenta.
    /// </summary>
    public interface ICuentaServicio
    {
        /// <summary>
        /// Obtiene todas las cuentas.
        /// </summary>
        /// <returns>Listado de cuentas.</returns>
        Task<Respuesta<IEnumerable<CuentaEntidad>>> ObtenerCuentasAsync();

        /// <summary>
        /// Obtiene una cuenta por su ID.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Entidad Cuenta encontrada.</returns>
        Task<Respuesta<CuentaEntidad>> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta);

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta a crear.</param>
        Task<Respuesta<CuentaEntidad>> CrearCuentaAsync(CuentaEntidad cuentaEntidad);

        /// <summary>
        /// Actualiza una cuenta existente.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta con los datos actualizados.</param>
        Task<Respuesta<CuentaEntidad>> ActualizarCuentaAsync(int idCuenta, CuentaEntidad cuentaEntidad);

        /// <summary>
        /// Elimina una cuenta por su ID.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        Task<Respuesta<string>> EliminarCuentaAsync(string numeroCuenta);
    }
}
