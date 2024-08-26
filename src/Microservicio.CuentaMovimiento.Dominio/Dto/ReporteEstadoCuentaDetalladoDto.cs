namespace Microservicio.CuentaMovimiento.Dominio.Dto
{
    /// <summary>
    /// DTO que representa el estado detallado de la cuenta, incluyendo movimientos y saldo disponible.
    /// </summary>
    public class ReporteEstadoCuentaDetalladoDto
    {
        /// <summary>
        /// Fecha en que se realizó el movimiento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Nombre del cliente asociado a la cuenta.
        /// </summary>
        public string Cliente { get; set; }

        /// <summary>
        /// Número de cuenta único asociado a la cuenta bancaria.
        /// </summary>
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Tipo de cuenta, como "Ahorro" o "Corriente".
        /// </summary>
        public string TipoCuenta { get; set; }

        /// <summary>
        /// Saldo inicial de la cuenta antes de los movimientos.
        /// </summary>
        public decimal SaldoInicial { get; set; }

        /// <summary>
        /// Estado de la cuenta, indicando si está activa o inactiva.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Valor del movimiento realizado en la cuenta (positivo o negativo).
        /// </summary>
        public decimal Movimiento { get; set; }

        /// <summary>
        /// Saldo disponible en la cuenta después del movimiento.
        /// </summary>
        public decimal SaldoDisponible { get; set; }
    }
}
