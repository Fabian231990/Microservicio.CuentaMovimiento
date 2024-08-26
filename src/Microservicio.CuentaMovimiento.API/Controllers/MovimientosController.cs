using Microservicio.CuentaMovimiento.Aplicacion.Servicios;
using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservicio.CuentaMovimiento.API.Controllers
{
    /// <summary>
    /// Controlador de API para la entidad Movimiento.
    /// </summary>
    /// <remarks>
    /// Constructor del controlador de movimientos.
    /// </remarks>
    /// <param name="movimientoServicio">Servicio de movimientos.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController(IMovimientoServicio movimientoServicio) : ControllerBase
    {
        /// <summary>
        /// Servicio de movimientos
        /// </summary>
        private readonly IMovimientoServicio _movimientoServicio = movimientoServicio;

        /// <summary>
        /// Registra un nuevo movimiento en una cuenta.
        /// </summary>
        /// <param name="movimientoDto">DTO del movimiento a registrar.</param>
        /// <returns>Respuesta con el movimiento registrado o un mensaje de error.</returns>
        [HttpPost]
        public async Task<ActionResult<Respuesta<MovimientoDto>>> RegistrarMovimiento(MovimientoDto movimientoDto)
        {
            if (movimientoDto == null)
            {
                return BadRequest(Respuesta<MovimientoDto>.CrearRespuestaFallida(400, "Los datos del movimiento no son validos."));
            }

            var respuesta = await _movimientoServicio.RegistrarMovimientoAsync(movimientoDto);
            if (!respuesta.EsExitoso)
            {
                return StatusCode(respuesta.Codigo, respuesta);
            }

            return Ok(respuesta);
        }
    }
}
