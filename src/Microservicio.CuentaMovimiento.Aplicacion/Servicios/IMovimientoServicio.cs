using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Aplicacion.Servicios
{
    /// <summary>
    /// Interfaz que define los metodos para manejar la logica de negocio relacionada con los movimientos.
    /// </summary>
    public interface IMovimientoServicio
    {
        /// <summary>
        /// Registra un nuevo movimiento en una cuenta.
        /// </summary>
        /// <param name="movimientoDto">DTO del movimiento a registrar.</param>
        /// <returns>Respuesta con el movimiento registrado o un mensaje de error si no es exitoso.</returns>
        Task<Respuesta<MovimientoDto>> RegistrarMovimientoAsync(MovimientoDto movimientoDto);

        /// <summary>
        /// Genera un reporte de estado de cuenta detallado para un cliente en un rango de fechas.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente.</param>
        /// <param name="fechaInicio">Fecha de inicio del rango.</param>
        /// <param name="fechaFin">Fecha de fin del rango.</param>
        /// <returns>Un reporte detallado de estado de cuenta.</returns>
        Task<Respuesta<IEnumerable<ReporteEstadoCuentaDetalladoDto>>> GenerarReporteEstadoCuentaAsync(string identificacion, DateTime fechaInicio, DateTime fechaFin);
    }
}
