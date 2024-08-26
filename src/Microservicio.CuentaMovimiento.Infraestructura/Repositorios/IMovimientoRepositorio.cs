using Microservicio.CuentaMovimiento.Dominio.Dto;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Interfaz Repositorio para la entidad Movimiento que interactua con la base de datos.
    /// </summary>
    public interface IMovimientoRepositorio
    {
        /// <summary>
        /// Guarda un nuevo movimiento en la base de datos.
        /// </summary>
        /// <param name="movimientoDto">El DTO Movimiento a guardar.</param>
        /// <returns>El objeto MovimientoDto guardado.</returns>
        Task<MovimientoDto> GuardarMovimientoAsync(MovimientoDto movimientoDto);

        /// <summary>
        /// Elimina todos los movimientos asociados a una cuenta especifica, identificada por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">El numero de cuenta cuyos movimientos seran eliminados.</param>
        Task EliminarMovimientosPorCuentaAsync(string numeroCuenta);

        /// <summary>
        /// Obtiene los movimientos de una cuenta en un rango de fechas.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <param name="fechaInicio">Fecha de inicio del rango.</param>
        /// <param name="fechaFin">Fecha de fin del rango.</param>
        /// <returns>Listado de movimientos dentro del rango de fechas.</returns>
        Task<IEnumerable<MovimientoDto>> ObtenerMovimientosPorCuentaYRangoFechasAsync(string numeroCuenta, DateTime fechaInicio, DateTime fechaFin);
    }
}
