using Microservicio.CuentaMovimiento.Aplicacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.CuentaMovimiento.API.Controllers
{
    /// <summary>
    /// Constructor de la Clase
    /// </summary>
    /// <param name="movimientoServicio">Servicio de movimientos.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController(IMovimientoServicio movimientoServicio) : ControllerBase
    {
        /// <summary>
        /// Servicio de movimientos.
        /// </summary>
        private readonly IMovimientoServicio _movimientoServicio = movimientoServicio;

        /// <summary>
        /// Genera un reporte detallado de estado de cuenta para un cliente en un rango de fechas.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente.</param>
        /// <param name="fechaInicio">Fecha de inicio del rango.</param>
        /// <param name="fechaFin">Fecha de fin del rango.</param>
        /// <returns>Reporte detallado de estado de cuenta en formato JSON.</returns>
        [HttpGet("reportes")]
        public async Task<IActionResult> GenerarReporteEstadoCuenta([FromQuery] string identificacion, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var respuesta = await _movimientoServicio.GenerarReporteEstadoCuentaAsync(identificacion, fechaInicio, fechaFin);
            if (!respuesta.EsExitoso)
            {
                return StatusCode(respuesta.Codigo, respuesta);
            }

            return Ok(respuesta);
        }
    }
}
