using System;

namespace WarApi.Models.Interfaces
{
    public interface IArmyList
    {
        Guid Id { get; set; }
        string Faction { get; set; }
        string Content { get; set; }
        Guid PlayerId { get; set; }
        Player Player { get; set; }
    }
}
