using System;
using WarApi.Models;

namespace WarApi.Models
{
    public class Lista
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;

        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
