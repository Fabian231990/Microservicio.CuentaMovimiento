namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Representa la entidad Movimiento en el dominio, asociada a una cuenta especifica.
    /// </summary>
    public partial class MovimientoEntidad
    {
        /// <summary>
        /// Identificador unico del movimiento.
        /// </summary>
        public int IdMovimiento { get; set; }

        /// <summary>
        /// Fecha y hora en que se realizo el movimiento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Tipo de movimiento, como "Deposito" o "Retiro".
        /// </summary>
        public string TipoMovimiento { get; set; }

        /// <summary>
        /// Valor monetario del movimiento realizado.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Saldo restante en la cuenta despues de realizar el movimiento.
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Identificador unico de la cuenta asociada a este movimiento.
        /// </summary>
        public int IdCuenta { get; set; }

        /// <summary>
        /// Entidad Cuenta asociada al movimiento.
        /// </summary>
        public virtual CuentaEntidad Cuenta { get; set; }
    }
}
