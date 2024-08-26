namespace Microservicio.CuentaMovimiento.Dominio.Dto
{
    /// <summary>
    /// DTO que representa un movimiento en una cuenta.
    /// </summary>
    public class MovimientoDto
    {
        /// <summary>
        /// Identificador unico del movimiento.
        /// </summary>
        public int IdMovimiento { get; set; }

        /// <summary>
        /// Fecha en la que se realizo el movimiento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Tipo de movimiento (por ejemplo, "Deposito", "Retiro").
        /// </summary>
        public string TipoMovimiento { get; set; }

        /// <summary>
        /// Valor del movimiento realizado.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Saldo restante en la cuenta despues del movimiento.
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Identificador de la cuenta asociada al movimiento.
        /// </summary>
        public int IdCuenta { get; set; }
    }
}
