using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Repositorios;
using Microservicio.CuentaMovimiento.Infraestructura.Servicios;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio que maneja la logica de negocio para la entidad Cuenta.
    /// </summary>
    /// <remarks>
    /// Constructor del servicio de cuentas.
    /// </remarks>
    /// <param name="cuentaRepositorio">Interfaz del Repositorio de Cuenta.</param>
    /// <param name="clienteServicio">Interfaz del Servicio cliente.</param>
    /// <param name="movimientoRepositorio">Interfaz del Repositorio de Movimiento.</param>
    public class CuentaServicio(ICuentaRepositorio cuentaRepositorio, IClienteServicio clienteServicio, IMovimientoRepositorio movimientoRepositorio) : ICuentaServicio
    {
        /// <summary>
        /// Interfaz del Repositorio de Cuenta.
        /// </summary>
        private readonly ICuentaRepositorio _cuentaRepositorio = cuentaRepositorio;

        /// <summary>
        /// Interfaz del Servicio cliente.
        /// </summary>
        private readonly IClienteServicio _clienteServicio = clienteServicio;

        /// <summary>
        /// Interfaz del Repositorio de Movimiento.
        /// </summary>
        private readonly IMovimientoRepositorio _movimientoRepositorio = movimientoRepositorio;

        /// <summary>
        /// Obtiene todas las cuentas registradas.
        /// </summary>
        /// <returns>Una respuesta con una coleccion enumerable de objetos CuentaDto.</returns>
        public async Task<Respuesta<IEnumerable<CuentaDto>>> ObtenerCuentasAsync()
        {
            var cuentas = await _cuentaRepositorio.ObtenerTodasAsync();
            return Respuesta<IEnumerable<CuentaDto>>.CrearRespuestaExitosa(cuentas);
        }

        /// <summary>
        /// Obtiene una cuenta especifica por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a buscar.</param>
        /// <returns>Una respuesta con el objeto CuentaDto correspondiente al numero de cuenta proporcionado.</returns>
        public async Task<Respuesta<CuentaDto>> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta)
        {
            var cuenta = await _cuentaRepositorio.ObtenerPorNumeroCuentaAsync(numeroCuenta);
            if (cuenta == null)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
            }

            return Respuesta<CuentaDto>.CrearRespuestaExitosa(cuenta);
        }

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaDto">El objeto CuentaDto que se va a crear.</param>
        /// <returns>Una respuesta con el objeto CuentaDto creado.</returns>
        public async Task<Respuesta<CuentaDto>> CrearCuentaAsync(CuentaDto cuentaDto)
        {
            // Validacion de datos basicos
            if (cuentaDto == null || string.IsNullOrEmpty(cuentaDto.NumeroCuenta))
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "Datos de la cuenta invalidos.");
            }

            // Verificar si el cliente existe llamando al microservicio de Clientes
            var clienteRespuesta = await _clienteServicio.ObtenerClientePorIdentificacionAsync(cuentaDto.Identificacion);
            if (!clienteRespuesta.EsExitoso)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(404, "El cliente asociado no fue encontrado.");
            }

            if (!clienteRespuesta.Datos.Estado)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El cliente asociado esta inactivo y no puede crear una cuenta.");
            }

            // Validacion del tipo de cuenta
            string tipoCuenta = cuentaDto.TipoCuenta.ToLower();
            if (tipoCuenta != "ahorro" && tipoCuenta != "corriente")
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El tipo de cuenta no es valido. Debe ser 'Ahorro' o 'Corriente'.");
            }

            // Validacion del saldo inicial
            if (cuentaDto.SaldoInicial < 0)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El saldo inicial no puede ser negativo.");
            }

            // Verificar si el numero de cuenta ya existe
            var cuentaExistente = await _cuentaRepositorio.ObtenerPorNumeroCuentaAsync(cuentaDto.NumeroCuenta);
            if (cuentaExistente != null)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(409, "Ya existe una cuenta con el mismo numero.");
            }

            if (cuentaDto.NumeroCuenta.Length != 10)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El numero de cuenta debe tener exactamente 10 digitos.");
            }

            var numeroDeCuentasDelCliente = await _cuentaRepositorio.ContarCuentasPorIdentificacionAsync(cuentaDto.Identificacion);
            if (numeroDeCuentasDelCliente >= 3)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El cliente ya tiene el numero maximo permitido de cuentas.");
            }

            if (cuentaDto.TipoCuenta.ToLower().Equals("ahorro", StringComparison.CurrentCultureIgnoreCase) && cuentaDto.SaldoInicial < 50)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El saldo inicial para una cuenta de ahorros debe ser al menos de $50.");
            }

            if (cuentaDto.TipoCuenta.ToLower().Equals("corriente", StringComparison.CurrentCultureIgnoreCase) && cuentaDto.SaldoInicial < 100)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El saldo inicial para una cuenta corriente debe ser al menos de $100.");
            }

            try
            {
                CuentaDto cuentaRespuestaDto = await _cuentaRepositorio.NuevoAsync(cuentaDto);

                // Crear el movimiento de tipo "Deposito" por el saldo inicial
                var movimientoDto = new MovimientoDto
                {
                    Fecha = DateTime.Now,
                    TipoMovimiento = "Deposito",
                    Valor = cuentaRespuestaDto.SaldoInicial,
                    Saldo = cuentaRespuestaDto.SaldoInicial,
                    IdCuenta = cuentaRespuestaDto.IdCuenta
                };

                // Guardar el movimiento
                await _movimientoRepositorio.GuardarMovimientoAsync(movimientoDto);

                return Respuesta<CuentaDto>.CrearRespuestaExitosa(cuentaRespuestaDto);
            }
            catch (Exception ex)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(500, $"Error al crear la cuenta: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza los datos de una cuenta existente.
        /// </summary>
        /// <param name="numeroCuenta">Numero de cuenta de la cuenta a actualizar.</param>
        /// <param name="cuentaDto">El objeto CuentaDto con los datos actualizados.</param>
        /// <returns>Una respuesta con el objeto CuentaDto actualizado.</returns>
        public async Task<Respuesta<CuentaDto>> ActualizarCuentaAsync(string numeroCuenta, CuentaDto cuentaDto)
        {
            // Obtener la cuenta existente de la base de datos
            var cuentaExistente = await _cuentaRepositorio.ObtenerPorNumeroCuentaAsync(numeroCuenta);
            if (cuentaExistente == null)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
            }

            // Validacion: Asegurarse de que la cuenta pertenece al mismo cliente
            var clienteRespuesta = await _clienteServicio.ObtenerClientePorIdentificacionAsync(cuentaDto.Identificacion);
            if (clienteRespuesta.Datos.IdCliente != cuentaExistente.IdCliente)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "La cuenta no pertenece al cliente especificado.");
            }

            // Validacion: Solo se pueden modificar los campos SaldoInicial y Estado
            if (cuentaDto.NumeroCuenta != cuentaExistente.NumeroCuenta || cuentaDto.TipoCuenta != cuentaExistente.TipoCuenta)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "Solo se permite modificar los campos SaldoInicial y Estado.");
            }

            // Validacion del Saldo Inicial
            if (cuentaDto.SaldoInicial < 0)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(400, "El saldo inicial no puede ser negativo.");
            }

            // Actualizar solo los campos permitidos
            cuentaExistente.SaldoInicial = cuentaDto.SaldoInicial;
            cuentaExistente.Estado = cuentaDto.Estado;

            try
            {
                await _cuentaRepositorio.ModificarAsync(cuentaExistente);
                cuentaDto.IdCuenta = cuentaExistente.IdCuenta;
                return Respuesta<CuentaDto>.CrearRespuestaExitosa(cuentaDto);
            }
            catch (Exception ex)
            {
                return Respuesta<CuentaDto>.CrearRespuestaFallida(500, $"Error al actualizar la cuenta: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una cuenta por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Una respuesta que indica el resultado de la operacion.</returns>
        public async Task<Respuesta<string>> EliminarCuentaAsync(string numeroCuenta)
        {
            // Obtener la cuenta por su numero
            var cuentaExistente = await _cuentaRepositorio.ObtenerPorNumeroCuentaAsync(numeroCuenta);

            // Verificar si la cuenta existe
            if (cuentaExistente == null)
            {
                return Respuesta<string>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
            }

            // Validacion: La cuenta debe estar inactiva (estado en false)
            if (cuentaExistente.Estado)
            {
                return Respuesta<string>.CrearRespuestaFallida(400, "No se puede eliminar una cuenta activa. La cuenta debe estar inactiva.");
            }

            // Validacion: El saldo de la cuenta debe ser 0
            if (cuentaExistente.SaldoInicial != 0)
            {
                return Respuesta<string>.CrearRespuestaFallida(400, "No se puede eliminar una cuenta con saldo. El saldo debe ser 0.");
            }

            try
            {
                //Eliminar primero los movimientos de esa cuenta
                await _movimientoRepositorio.EliminarMovimientosPorCuentaAsync(numeroCuenta);

                // Eliminar la cuenta
                await _cuentaRepositorio.EliminarAsync(numeroCuenta);
                return Respuesta<string>.CrearRespuestaExitosa("Cuenta eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                return Respuesta<string>.CrearRespuestaFallida(500, $"Error al eliminar la cuenta: {ex.Message}");
            }
        }
    }
}
