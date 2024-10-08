﻿using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Repositorios;
using Microservicio.CuentaMovimiento.Infraestructura.Servicios;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;

namespace Microservicio.CuentaMovimiento.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio que maneja la logica de negocio para la entidad Movimiento.
    /// </summary>
    /// <remarks>
    /// Constructor del servicio de movimientos.
    /// </remarks>
    /// <param name="movimientoRepositorio">Interfaz del Repositorio de Movimiento.</param>
    /// <param name="cuentaRepositorio">Interfaz del Repositorio de Cuenta.</param>
    /// <param name="cuentaRepositorio">Interfaz del Servicio cliente.</param>
    public class MovimientoServicio(IMovimientoRepositorio movimientoRepositorio, ICuentaRepositorio cuentaRepositorio, IClienteServicio clienteServicio) : IMovimientoServicio
    {
        /// <summary>
        /// Interfaz del Repositorio de Movimiento.
        /// </summary>
        private readonly IMovimientoRepositorio _movimientoRepositorio = movimientoRepositorio;

        /// <summary>
        /// Interfaz del Repositorio de Cuenta.
        /// </summary>
        private readonly ICuentaRepositorio _cuentaRepositorio = cuentaRepositorio;


        /// <summary>
        /// Interfaz del Servicio cliente.
        /// </summary>
        private readonly IClienteServicio _clienteServicio = clienteServicio;

        /// <summary>
        /// Registra un nuevo movimiento en una cuenta, actualizando el saldo disponible y registrando la transaccion.
        /// </summary>
        /// <param name="movimientoDto">DTO del movimiento a registrar.</param>
        /// <returns>Respuesta con el movimiento registrado o un mensaje de error si no se puede realizar el movimiento.</returns>
        public async Task<Respuesta<MovimientoDto>> RegistrarMovimientoAsync(MovimientoDto movimientoDto)
        {
            try
            {
                // Validar que los datos basicos del movimiento sean correctos
                if (movimientoDto == null || string.IsNullOrEmpty(movimientoDto.NumeroCuenta))
                {
                    return Respuesta<MovimientoDto>.CrearRespuestaFallida(400, "Los datos del movimiento no son validos.");
                }

                // Obtener la cuenta asociada al numero de cuenta
                var cuenta = await _cuentaRepositorio.ObtenerPorNumeroCuentaAsync(movimientoDto.NumeroCuenta);
                if (cuenta == null)
                {
                    return Respuesta<MovimientoDto>.CrearRespuestaFallida(404, "Cuenta no encontrada.");
                }

                // Validar el tipo de movimiento y actualizar el saldo de la cuenta
                if (movimientoDto.TipoMovimiento == "Deposito")
                {
                    cuenta.SaldoInicial += movimientoDto.Valor;
                }
                else if (movimientoDto.TipoMovimiento == "Retiro")
                {
                    // Verificar que el saldo sea suficiente si el movimiento es un retiro
                    if (cuenta.SaldoInicial < movimientoDto.Valor)
                    {
                        return Respuesta<MovimientoDto>.CrearRespuestaFallida(400, "Saldo no disponible.");
                    }
                    cuenta.SaldoInicial -= movimientoDto.Valor;
                }
                else
                {
                    return Respuesta<MovimientoDto>.CrearRespuestaFallida(400, "Tipo de movimiento no valido.");
                }

                // Registrar el movimiento en la base de datos
                var nuevoMovimiento = new MovimientoDto
                {
                    Fecha = DateTime.Now,
                    TipoMovimiento = movimientoDto.TipoMovimiento,
                    Valor = movimientoDto.Valor,
                    Saldo = cuenta.SaldoInicial,
                    NumeroCuenta = movimientoDto.NumeroCuenta,
                    IdCuenta = cuenta.IdCuenta
                };

                await _movimientoRepositorio.GuardarMovimientoAsync(nuevoMovimiento);
                await _cuentaRepositorio.ModificarAsync(cuenta);

                // Retornar el movimiento registrado
                return Respuesta<MovimientoDto>.CrearRespuestaExitosa(nuevoMovimiento);
            }
            catch (Exception ex)
            {
                return Respuesta<MovimientoDto>.CrearRespuestaFallida(500, $"Error inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Genera un reporte de estado de cuenta detallado para un cliente en un rango de fechas.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente.</param>
        /// <param name="fechaInicio">Fecha de inicio del rango.</param>
        /// <param name="fechaFin">Fecha de fin del rango.</param>
        /// <returns>Un reporte detallado de estado de cuenta.</returns>
        public async Task<Respuesta<IEnumerable<ReporteEstadoCuentaDetalladoDto>>> GenerarReporteEstadoCuentaAsync(string identificacion, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Verificar si el cliente existe
                var clienteRespuesta = await _clienteServicio.ObtenerClientePorIdentificacionAsync(identificacion);
                if (!clienteRespuesta.EsExitoso)
                {
                    return Respuesta<IEnumerable<ReporteEstadoCuentaDetalladoDto>>.CrearRespuestaFallida(404, "Cliente no encontrado.");
                }

                var cliente = clienteRespuesta.Datos;

                // Obtener las cuentas asociadas al cliente
                var cuentasRespuesta = await _cuentaRepositorio.ObtenerCuentasPorIdentificacionAsync(identificacion);
                if (!cuentasRespuesta.Any())
                {
                    return Respuesta<IEnumerable<ReporteEstadoCuentaDetalladoDto>>.CrearRespuestaFallida(404, "No se encontraron cuentas para el cliente.");
                }

                var reporte = new List<ReporteEstadoCuentaDetalladoDto>();

                foreach (var cuenta in cuentasRespuesta)
                {
                    // Obtener los movimientos en el rango de fechas especificado
                    var movimientosRespuesta = await _movimientoRepositorio.ObtenerMovimientosPorCuentaYRangoFechasAsync(cuenta.NumeroCuenta, fechaInicio, fechaFin);

                    if (movimientosRespuesta.Any())
                    {
                        foreach (var movimiento in movimientosRespuesta)
                        {
                            var reporteItem = new ReporteEstadoCuentaDetalladoDto
                            {
                                Fecha = movimiento.Fecha,
                                Cliente = cliente.Nombre,
                                NumeroCuenta = cuenta.NumeroCuenta,
                                TipoCuenta = cuenta.TipoCuenta,
                                SaldoInicial = cuenta.SaldoInicial,
                                Estado = cuenta.Estado,
                                Movimiento = movimiento.Valor,
                                SaldoDisponible = movimiento.Saldo
                            };

                            reporte.Add(reporteItem);
                        }
                    }
                }

                return Respuesta<IEnumerable<ReporteEstadoCuentaDetalladoDto>>.CrearRespuestaExitosa(reporte);
            }
            catch (Exception ex)
            {
                return Respuesta<IEnumerable<ReporteEstadoCuentaDetalladoDto>>.CrearRespuestaFallida(500, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
