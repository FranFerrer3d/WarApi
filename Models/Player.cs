
using WarApi.Models.Interfaces;
using System.Text.Json.Serialization;
namespace WarApi.Models
{
    public class Player:IPlayer
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string? Foto { get; set; }
        public string Equipo { get; set; } = string.Empty;

        // Relaciones futuras
        //public List<Lista>? Listas { get; set; }
        //public List<Reporte>? Reportes { get; set; }

    }
}
