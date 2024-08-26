namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Representa la entidad Cuenta en el dominio, asociada a un cliente y con una coleccion de movimientos.
    /// </summary>
    public partial class CuentaEntidad
    {
        /// <summary>
        /// Identificador unico de la cuenta.
        /// </summary>
        public int IdCuenta { get; set; }

        /// <summary>
        /// Numero de cuenta unico asociado a la cuenta bancaria.
        /// </summary>
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Tipo de cuenta, como "Ahorro" o "Corriente".
        /// </summary>
        public string TipoCuenta { get; set; }

        /// <summary>
        /// Saldo inicial de la cuenta al momento de su creacion.
        /// </summary>
        public decimal SaldoInicial { get; set; }

        /// <summary>
        /// Estado de la cuenta, indicando si esta activa o inactiva.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Identificador unico del cliente asociado a esta cuenta.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Entidad Cliente asociada a la cuenta.
        /// </summary>
        public virtual ClienteEntidad Cliente { get; set; }

        /// <summary>
        /// Coleccion de movimientos asociados a esta cuenta.
        /// </summary>
        public virtual ICollection<MovimientoEntidad> Movimientos { get; set; } = [];
    }
}
