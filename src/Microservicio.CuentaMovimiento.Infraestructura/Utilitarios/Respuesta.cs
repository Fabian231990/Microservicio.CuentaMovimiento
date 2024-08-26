using System.Text.Json.Serialization;

namespace Microservicio.CuentaMovimiento.Infraestructura.Utilitarios
{
    public class Respuesta<T>
    {
        [JsonPropertyName("esExitoso")]
        public bool EsExitoso { get; set; }

        [JsonPropertyName("datos")]
        public T Datos { get; set; }

        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        public static Respuesta<T> CrearRespuestaExitosa(T datos)
        {
            return new Respuesta<T>
            {
                EsExitoso = true,
                Datos = datos,
                Codigo = 100000,
                Mensaje = "OK"
            };
        }

        public static Respuesta<T> CrearRespuestaFallida(int codigo, string mensaje)
        {
            return new Respuesta<T>
            {
                EsExitoso = false,
                Datos = default,
                Codigo = codigo,
                Mensaje = mensaje
            };
        }
    }

}
