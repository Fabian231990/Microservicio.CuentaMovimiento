using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Repositorios;
using Microservicio.CuentaMovimiento.Infraestructura.Servicios;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio que maneja la lógica de negocio para la entidad Cuenta.
    /// </summary>
    /// <remarks>
    /// Constructor del servicio de cuentas.
    /// </remarks>
    /// <param name="iCuentaRepositorio">Repositorio de cuentas.</param>
    /// <param name="iClienteServicio">Servicio para interactuar con el microservicio de Clientes.</param>
    /// <param name="iCuentaRepositorio">Repositorio de movimientos.</param>
    public class CuentaServicio(ICuentaRepositorio iCuentaRepositorio, IClienteServicio iClienteServicio, IMovimientoRepositorio iMovimientoRepositorio) : ICuentaServicio
    {
        private readonly ICuentaRepositorio iCuentaRepositorio = iCuentaRepositorio;
        private readonly IClienteServicio iClienteServicio = iClienteServicio;
        private readonly IMovimientoRepositorio iMovimientoRepositorio = iMovimientoRepositorio;

        /// <summary>
        /// Obtiene todas las cuentas registradas.
        /// </summary>
        /// <returns>Una respuesta con una colección enumerable de objetos CuentaEntidad.</returns>
        public async Task<Respuesta<IEnumerable<CuentaEntidad>>> ObtenerCuentasAsync()
        {
            IEnumerable<CuentaEntidad> cuentas = await iCuentaRepositorio.ObtenerTodasAsync();
            return Respuesta<IEnumerable<CuentaEntidad>>.CrearRespuestaExitosa(cuentas);
        }

        /// <summary>
        /// Obtiene una cuenta específica por su ID.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Una respuesta con el objeto CuentaEntidad correspondiente al ID proporcionado.</returns>
        public async Task<Respuesta<CuentaEntidad>> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta)
        {
            CuentaEntidad cuenta = await iCuentaRepositorio.ObtenerPorNumeroCuentaAsync(numeroCuenta);
            if (cuenta == null)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
            }

            return Respuesta<CuentaEntidad>.CrearRespuestaExitosa(cuenta);
        }

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaEntidad">El objeto CuentaEntidad que se va a crear.</param>
        /// <returns>Una respuesta con el objeto CuentaEntidad creado.</returns>
        public async Task<Respuesta<CuentaEntidad>> CrearCuentaAsync(CuentaEntidad cuentaEntidad)
        {
            // Validación de datos básicos
            if (cuentaEntidad == null || cuentaEntidad.IdCliente <= 0 || string.IsNullOrEmpty(cuentaEntidad.NumeroCuenta))
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "Datos de la cuenta inválidos.");
            }

            // Verificar si el cliente existe llamando al microservicio de Clientes
            Respuesta<ClienteEntidad> clienteRespuesta = await iClienteServicio.ObtenerClientePorIdAsync(cuentaEntidad.IdCliente);
            if (!clienteRespuesta.EsExitoso)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(404, "El cliente asociado no fue encontrado.");
            }

            if (!clienteRespuesta.Datos.Estado)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El cliente asociado está inactivo y no puede crear una cuenta.");
            }

            // Validación del tipo de cuenta
            string tipoCuenta = cuentaEntidad.TipoCuenta.ToLower();
            if (tipoCuenta != "ahorro" && tipoCuenta != "corriente")
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El tipo de cuenta no es válido. Debe ser 'Ahorro' o 'Corriente'.");
            }

            // Validación del saldo inicial
            if (cuentaEntidad.SaldoInicial < 0)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El saldo inicial no puede ser negativo.");
            }

            // Verificar si el número de cuenta ya existe
            CuentaEntidad cuentaExistente = await iCuentaRepositorio.ObtenerPorNumeroCuentaAsync(cuentaEntidad.NumeroCuenta);
            if (cuentaExistente != null)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(409, "Ya existe una cuenta con el mismo número.");
            }

            if (cuentaEntidad.NumeroCuenta.Length != 10)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El número de cuenta debe tener exactamente 10 dígitos.");
            }

            int numeroDeCuentasDelCliente = await iCuentaRepositorio.ContarCuentasPorClienteAsync(cuentaEntidad.IdCliente);
            if (numeroDeCuentasDelCliente >= 3)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El cliente ya tiene el número máximo permitido de cuentas.");
            }

            CuentaEntidad cuentaExistenteDelMismoTipo = await iCuentaRepositorio.ObtenerPorClienteYTipoCuentaAsync(cuentaEntidad.IdCliente, cuentaEntidad.TipoCuenta);
            if (cuentaExistenteDelMismoTipo != null)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, $"El cliente ya tiene una cuenta de tipo {cuentaEntidad.TipoCuenta}.");
            }

            if (cuentaEntidad.TipoCuenta.Equals("ahorro", StringComparison.CurrentCultureIgnoreCase) && cuentaEntidad.SaldoInicial < 50)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El saldo inicial para una cuenta de ahorros debe ser al menos de $50.");
            }

            if (cuentaEntidad.TipoCuenta.Equals("corriente", StringComparison.CurrentCultureIgnoreCase) && cuentaEntidad.SaldoInicial < 100)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El saldo inicial para una cuenta de ahorros debe ser al menos de $100.");
            }

            try
            {
                // Crear la cuenta
                await iCuentaRepositorio.NuevoAsync(cuentaEntidad);

                // Crear el movimiento de tipo "Depósito" por el saldo inicial
                MovimientoEntidad nuevoMovimiento = new()
                {
                    Fecha = DateTime.Now,
                    TipoMovimiento = "Depósito",
                    Valor = cuentaEntidad.SaldoInicial,
                    Saldo = cuentaEntidad.SaldoInicial,
                    IdCuenta = cuentaEntidad.IdCuenta
                };

                // Guardar el movimiento
                await iMovimientoRepositorio.GuardarMovimientoAsync(nuevoMovimiento);

                return Respuesta<CuentaEntidad>.CrearRespuestaExitosa(cuentaEntidad);
            }
            catch (Exception ex)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(500, $"Error al crear la cuenta: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza los datos de una cuenta existente.
        /// </summary>
        /// <param name="idCuenta">El ID de la cuenta a actualizar.</param>
        /// <param name="cuentaEntidad">El objeto CuentaEntidad con los datos actualizados.</param>
        /// <returns>Una respuesta con el objeto CuentaEntidad actualizado.</returns>
        public async Task<Respuesta<CuentaEntidad>> ActualizarCuentaAsync(int idCuenta, CuentaEntidad cuentaEntidad)
        {
            // Validación inicial: Verificar que el ID de la cuenta proporcionado coincida
            if (idCuenta != cuentaEntidad.IdCuenta)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El ID de la cuenta proporcionado no coincide.");
            }

            // Obtener la cuenta existente de la base de datos
            CuentaEntidad cuentaExistente = await iCuentaRepositorio.ObtenerPorNumeroCuentaAsync(cuentaEntidad.NumeroCuenta);
            if (cuentaExistente == null)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
            }

            // Validación: Asegurarse de que la cuenta pertenece al mismo cliente
            if (cuentaExistente.IdCliente != cuentaEntidad.IdCliente)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "La cuenta no pertenece al cliente especificado.");
            }

            // Validación: Solo se pueden modificar los campos SaldoInicial y Estado
            if (cuentaEntidad.NumeroCuenta != cuentaExistente.NumeroCuenta || cuentaEntidad.TipoCuenta != cuentaExistente.TipoCuenta)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "Solo se permite modificar los campos SaldoInicial y Estado.");
            }

            // Validación del Saldo Inicial
            if (cuentaEntidad.SaldoInicial < 0)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(400, "El saldo inicial no puede ser negativo.");
            }

            // Actualizar solo los campos permitidos
            cuentaExistente.SaldoInicial = cuentaEntidad.SaldoInicial;
            cuentaExistente.Estado = cuentaEntidad.Estado;

            try
            {
                await iCuentaRepositorio.ModificarAsync(cuentaExistente);
                return Respuesta<CuentaEntidad>.CrearRespuestaExitosa(cuentaExistente);
            }
            catch (Exception ex)
            {
                return Respuesta<CuentaEntidad>.CrearRespuestaFallida(500, $"Error al actualizar la cuenta: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una cuenta por su número de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <returns>Una respuesta que indica el resultado de la operación.</returns>
        public async Task<Respuesta<string>> EliminarCuentaAsync(string numeroCuenta)
        {
            // Obtener la cuenta por su número
            CuentaEntidad cuentaExistente = await iCuentaRepositorio.ObtenerPorNumeroCuentaAsync(numeroCuenta);

            // Verificar si la cuenta existe
            if (cuentaExistente == null)
            {
                return Respuesta<string>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
            }

            // Validación: La cuenta debe estar inactiva (estado en false)
            if (cuentaExistente.Estado)
            {
                return Respuesta<string>.CrearRespuestaFallida(400, "No se puede eliminar una cuenta activa. La cuenta debe estar inactiva.");
            }

            // Validación: El saldo de la cuenta debe ser 0
            if (cuentaExistente.SaldoInicial != 0)
            {
                return Respuesta<string>.CrearRespuestaFallida(400, "No se puede eliminar una cuenta con saldo. El saldo debe ser 0.");
            }

            try
            {
                //Eliminar primero los movimientos de esa cuenta
                await iMovimientoRepositorio.EliminarMovimientosPorCuentaAsync(cuentaExistente.IdCuenta);

                // Eliminar la cuenta
                await iCuentaRepositorio.EliminarAsync(cuentaExistente.IdCuenta);
                return Respuesta<string>.CrearRespuestaExitosa("Cuenta eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                return Respuesta<string>.CrearRespuestaFallida(500, $"Error al eliminar la cuenta: {ex.Message}");
            }
        }
    }
}
