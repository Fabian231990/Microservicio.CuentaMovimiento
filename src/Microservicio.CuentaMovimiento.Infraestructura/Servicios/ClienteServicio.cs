using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Microservicio.CuentaMovimiento.Infraestructura.Servicios
{
    /// <summary>
    /// Servicio para interactuar con el microservicio de Clientes.
    /// </summary>
    /// <remarks>
    /// Constructor que inyecta el HttpClient.
    /// </remarks>
    /// <param name="httpClient">Instancia de HttpClient inyectada.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    public class ClienteServicio(HttpClient httpClient, IConfiguration configuration) : IClienteServicio
    {
        /// <summary>
        /// Instancia de HttpClient inyectada.
        /// </summary>
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Configuración de la aplicación.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Llama al microservicio de Clientes para obtener informacion de un cliente por identificacion.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente a consultar.</param>
        /// <returns>Respuesta que contiene la informacion del cliente como DTO.</returns>
        public async Task<Respuesta<ClienteDto>> ObtenerClientePorIdentificacionAsync(string identificacion)
        {
            try
            {
                // Construir la URL del servicio
                string url = string.Format("{0}{1}", _configuration["ConfiguracionesCliente:UrlServicioClientes"], identificacion);

                // Hacer la llamada HTTP
                HttpResponseMessage respuesta = await _httpClient.GetAsync(url);

                if (respuesta.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta
                    string contenidoJson = await respuesta.Content.ReadAsStringAsync();

                    // Deserializar la respuesta en el DTO de cliente
                    var resultado = JsonSerializer.Deserialize<Respuesta<ClienteDto>>(contenidoJson);

                    // Devolver la respuesta exitosa
                    return resultado;
                }
                else
                {
                    // Manejar el caso en el que la llamada no fue exitosa
                    return Respuesta<ClienteDto>.CrearRespuestaFallida((int)respuesta.StatusCode, $"Error al llamar al microservicio PersonaCliente: {respuesta.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el proceso
                return Respuesta<ClienteDto>.CrearRespuestaFallida(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
