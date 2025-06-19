using System.Collections.Generic;
using MatchReportNamespace;
using WarApi.Models;
namespace WarApi.Models.Interfaces
{
    public interface IPlayer
    {
        Guid ID { get; set; }
        string Nombre { get; set; }
        string Apellidos { get; set; }
        string Alias { get; set; }
        string Email { get; set; }
        string Contraseña { get; set; }
        string? Foto { get; set; }
        string Equipo { get; set; }
        ICollection<Lista>? Listas { get; set; }
        ICollection<MatchReport>? Reportes { get; set; }
    }
}
