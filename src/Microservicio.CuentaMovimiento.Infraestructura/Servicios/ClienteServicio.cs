using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Utilitarios;
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
    public class ClienteServicio(HttpClient httpClient) : IClienteServicio
    {
        private readonly HttpClient httpClient = httpClient;

        /// <summary>
        /// Llama al microservicio de Clientes para obtener información de un cliente por ID.
        /// </summary>
        /// <param name="clienteId">ID del cliente a consultar.</param>
        /// <returns>Contenido de la respuesta como string.</returns>
        public async Task<Respuesta<ClienteEntidad>> ObtenerClientePorIdAsync(int clienteId)
        {
            string url = $"http://localhost:9100/Clientes/{clienteId}";

            HttpResponseMessage respuesta = await httpClient.GetAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                string contenidoJson = await respuesta.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Respuesta<ClienteEntidad>>(contenidoJson);
            }
            else
            {
                throw new HttpRequestException($"Error al llamar al microservicio PersonaCliente: {respuesta.StatusCode}");
            }
        }
    }
}
