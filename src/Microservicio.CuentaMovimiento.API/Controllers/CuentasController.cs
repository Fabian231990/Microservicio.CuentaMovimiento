using Microservicio.CuentaMovimiento.Aplicacion.Servicios;
using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.CuentaMovimiento.API.Controllers
{
    /// <summary>
    /// Controlador de API para la entidad Cuenta.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaServicio _cuentaServicio;

        /// <summary>
        /// Constructor del controlador de cuentas.
        /// </summary>
        /// <param name="cuentaServicio">Servicio de cuentas.</param>
        public CuentasController(ICuentaServicio cuentaServicio)
        {
            _cuentaServicio = cuentaServicio;
        }

        /// <summary>
        /// Obtiene todas las cuentas.
        /// </summary>
        /// <returns>Listado de cuentas.</returns>
        [HttpGet]
        public async Task<ActionResult<Respuesta<IEnumerable<CuentaDto>>>> ObtenerCuentas()
        {
            var respuesta = await _cuentaServicio.ObtenerCuentasAsync();
            return Ok(respuesta);
        }

        /// <summary>
        /// Obtiene una cuenta por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Cuenta encontrada.</returns>
        [HttpGet("{numeroCuenta}")]
        public async Task<ActionResult<Respuesta<CuentaDto>>> ObtenerCuenta(string numeroCuenta)
        {
            var respuesta = await _cuentaServicio.ObtenerCuentaPorNumeroCuentaAsync(numeroCuenta);
            if (!respuesta.EsExitoso)
            {
                return NotFound(respuesta);
            }

            return Ok(respuesta);
        }

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaDto">DTO de la cuenta a crear.</param>
        [HttpPost]
        public async Task<ActionResult<Respuesta<CuentaDto>>> CrearCuenta(CuentaDto cuentaDto)
        {
            var respuesta = await _cuentaServicio.CrearCuentaAsync(cuentaDto);
            if (!respuesta.EsExitoso)
            {
                if (respuesta.Codigo == 404)
                {
                    return NotFound(respuesta);
                }
                return StatusCode(500, respuesta);
            }

            return CreatedAtAction(nameof(ObtenerCuenta), new { numeroCuenta = cuentaDto.NumeroCuenta }, respuesta);
        }

        /// <summary>
        /// Actualiza una cuenta existente.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <param name="cuentaDto">DTO de la cuenta con los datos actualizados.</param>
        [HttpPut("{numeroCuenta}")]
        public async Task<ActionResult<Respuesta<CuentaDto>>> ActualizarCuenta(string numeroCuenta, CuentaDto cuentaDto)
        {
            var respuesta = await _cuentaServicio.ActualizarCuentaAsync(numeroCuenta, cuentaDto);
            if (!respuesta.EsExitoso)
            {
                return StatusCode(respuesta.Codigo, respuesta);
            }

            return Ok(respuesta);
        }

        /// <summary>
        /// Elimina una cuenta por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a eliminar.</param>
        [HttpDelete("{numeroCuenta}")]
        public async Task<ActionResult<Respuesta<string>>> EliminarCuenta(string numeroCuenta)
        {
            var respuesta = await _cuentaServicio.EliminarCuentaAsync(numeroCuenta);
            if (!respuesta.EsExitoso)
            {
                return StatusCode(respuesta.Codigo, respuesta);
            }

            return Ok(respuesta);
        }
    }
}
