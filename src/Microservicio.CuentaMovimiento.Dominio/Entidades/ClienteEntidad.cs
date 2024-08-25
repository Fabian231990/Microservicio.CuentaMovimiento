using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Clase Entidad de la tabla Cliente
    /// </summary>
    public class ClienteEntidad
    {
        /// <summary>
        /// Identificador unico del Cliente
        /// </summary>
        [Key]
        [JsonPropertyName("idCliente")]
        public int IdCliente { get; set; }

        /// <summary>
        /// Clave foranea que referencia a Persona
        /// </summary>
        [JsonPropertyName("idPersona")]
        public int IdPersona { get; set; }

        /// <summary>
        /// Contrasenia del Cliente
        /// </summary>
        [JsonPropertyName("contrasenia")]
        public string Contrasenia { get; set; }

        /// <summary>
        /// Estado del Cliente
        /// </summary>
        [JsonPropertyName("estado")]
        public bool Estado { get; set; }
    }
}
