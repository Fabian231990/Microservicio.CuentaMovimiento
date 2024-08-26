using Microservicio.CuentaMovimiento.Dominio.Dto;
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
        /// <summary>
        /// Instancia de HttpClient inyectada.
        /// </summary>
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Llama al microservicio de Clientes para obtener informacion de un cliente por identificacion.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente a consultar.</param>
        /// <returns>Respuesta que contiene la informacion del cliente como DTO.</returns>
        public async Task<Respuesta<ClienteDto>> ObtenerClientePorIdentificacionAsync(string identificacion)
        {
            string url = $"http://localhost:9100/Clientes/{identificacion}";

            HttpResponseMessage respuesta = await _httpClient.GetAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                string contenidoJson = await respuesta.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Respuesta<ClienteDto>>(contenidoJson);
            }
            else
            {
                throw new HttpRequestException($"Error al llamar al microservicio PersonaCliente: {respuesta.StatusCode}");
            }
        }
    }
}
