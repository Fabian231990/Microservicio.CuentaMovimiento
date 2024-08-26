using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio que maneja la logica de negocio para la entidad Cuenta.
    /// </summary>
    public interface ICuentaServicio
    {
        /// <summary>
        /// Obtiene todas las cuentas registradas.
        /// </summary>
        /// <returns>Una respuesta con una coleccion enumerable de objetos CuentaDto.</returns>
        Task<Respuesta<IEnumerable<CuentaDto>>> ObtenerCuentasAsync();

        /// <summary>
        /// Obtiene una cuenta especifica por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a buscar.</param>
        /// <returns>Una respuesta con el objeto CuentaDto correspondiente al numero de cuenta proporcionado.</returns>
        Task<Respuesta<CuentaDto>> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta);

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaDto">El objeto CuentaDto que se va a crear.</param>
        /// <returns>Una respuesta con el objeto CuentaDto creado.</returns>
        Task<Respuesta<CuentaDto>> CrearCuentaAsync(CuentaDto cuentaDto);

        /// <summary>
        /// Actualiza los datos de una cuenta existente.
        /// </summary>
        /// <param name="numeroCuenta">Numero de cuenta de la cuenta a actualizar.</param>
        /// <param name="cuentaDto">El objeto CuentaDto con los datos actualizados.</param>
        /// <returns>Una respuesta con el objeto CuentaDto actualizado.</returns>
        Task<Respuesta<CuentaDto>> ActualizarCuentaAsync(string numeroCuenta, CuentaDto cuentaDto);

        /// <summary>
        /// Elimina una cuenta por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Una respuesta que indica el resultado de la operacion.</returns>
        Task<Respuesta<string>> EliminarCuentaAsync(string numeroCuenta);
    }
}
