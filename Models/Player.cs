
namespace WarApi.Models
{
    public class Player
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string? Foto { get; set; }
        public string Equipo { get; set; } = string.Empty;

        public Team? Team { get; set; }
        public Enums.UserRole Rol { get; set; } = Enums.UserRole.User;

        // Relaciones futuras
        //public List<Lista>? Listas { get; set; }
        //public List<Reporte>? Reportes { get; set; }

    }
}
