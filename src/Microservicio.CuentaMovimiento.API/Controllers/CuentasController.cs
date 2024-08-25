using Microservicio.CuentaMovimiento.Aplicacion.Servicios;
using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.CuentaMovimiento.API.Controllers
{
    /// <summary>
    /// Controlador de API para la entidad Cuenta.
    /// </summary>
    /// <remarks>
    /// Constructor del controlador de cuentas.
    /// </remarks>
    /// <param name="cuentaServicio">Servicio de cuentas.</param>
    [Route("[controller]")]
    [ApiController]
    public class CuentasController(ICuentaServicio iCuentaServicio) : ControllerBase
    {
        private readonly ICuentaServicio iCuentaServicio = iCuentaServicio;

        /// <summary>
        /// Obtiene todas las cuentas.
        /// </summary>
        /// <returns>Listado de cuentas.</returns>
        [HttpGet]
        public async Task<ActionResult<Respuesta<IEnumerable<CuentaEntidad>>>> ObtenerCuentas()
        {
            var respuesta = await iCuentaServicio.ObtenerCuentasAsync();
            return Ok(respuesta);
        }

        /// <summary>
        /// Obtiene una cuenta por su Numero.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Entidad Cuenta encontrada.</returns>
        [HttpGet("{numeroCuenta}")]
        public async Task<ActionResult<Respuesta<CuentaEntidad>>> ObtenerCuenta(string numeroCuenta)
        {
            var respuesta = await iCuentaServicio.ObtenerCuentaPorNumeroCuentaAsync(numeroCuenta);
            if (!respuesta.EsExitoso)
            {
                return NotFound(respuesta);
            }

            return Ok(respuesta);
        }

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta a crear.</param>
        [HttpPost]
        public async Task<ActionResult<Respuesta<CuentaEntidad>>> CrearCuenta(CuentaEntidad cuentaEntidad)
        {
            var respuesta = await iCuentaServicio.CrearCuentaAsync(cuentaEntidad);
            if (!respuesta.EsExitoso)
            {
                if (respuesta.Codigo == 404)
                {
                    return NotFound(respuesta);
                }
                return StatusCode(500, respuesta);
            }

            return CreatedAtAction(nameof(ObtenerCuenta), new { numeroCuenta = cuentaEntidad.NumeroCuenta }, respuesta);
        }

        /// <summary>
        /// Actualiza una cuenta existente.
        /// </summary>
        /// <param name="idCuenta">Identificador de la cuenta.</param>
        /// <param name="cuentaEntidad">Entidad Cuenta con los datos actualizados.</param>
        [HttpPut("{idCuenta}")]
        public async Task<ActionResult<Respuesta<CuentaEntidad>>> ActualizarCuenta(int idCuenta, CuentaEntidad cuentaEntidad)
        {
            var respuesta = await iCuentaServicio.ActualizarCuentaAsync(idCuenta, cuentaEntidad);
            if (!respuesta.EsExitoso)
            {
                return StatusCode(respuesta.Codigo, respuesta);
            }

            return Ok(respuesta);
        }

        /// <summary>
        /// Elimina una cuenta por su Numero.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a eliminar.</param>
        [HttpDelete("{numeroCuenta}")]
        public async Task<ActionResult<Respuesta<string>>> EliminarCuenta(string numeroCuenta)
        {
            var respuesta = await iCuentaServicio.EliminarCuentaAsync(numeroCuenta);
            if (!respuesta.EsExitoso)
            {
                return StatusCode(respuesta.Codigo, respuesta);
            }

            return Ok(respuesta);
        }
    }
}
