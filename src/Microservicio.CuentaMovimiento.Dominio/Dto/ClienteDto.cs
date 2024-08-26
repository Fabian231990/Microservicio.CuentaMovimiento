using System.Text.Json.Serialization;

namespace Microservicio.CuentaMovimiento.Dominio.Dto
{
    /// <summary>
    /// DTO para la entidad Cliente.
    /// </summary>
    public class ClienteDto
    {
        /// <summary>
        /// Identificador unico del cliente.
        /// </summary>
        [JsonPropertyName("idCliente")]
        public int IdCliente { get; set; }

        /// <summary>
        /// Identificador unico de la persona.
        /// </summary>
        [JsonPropertyName("idPersona")]
        public int IdPersona { get; set; }

        /// <summary>
        /// Numero de identificacion unico de la persona.
        /// </summary>
        [JsonPropertyName("identificacion")]
        public string Identificacion { get; set; }

        /// <summary>
        /// Contraseña del cliente para acceder a sus servicios.
        /// </summary>
        [JsonPropertyName("contrasenia")]
        public string Contrasenia { get; set; }

        /// <summary>
        /// Estado del cliente. Indica si el cliente esta activo o inactivo.
        /// </summary>
        [JsonPropertyName("estado")]
        public bool Estado { get; set; }
    }
}
