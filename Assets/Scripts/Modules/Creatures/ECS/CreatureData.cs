using DOTS_RTS.Modules.Creatures.Models.Enums;
using Unity.Entities;

namespace DOTS_RTS.Modules.Creatures.ECS
{
    public struct CreatureData : IComponentData
    {
        public Faction Faction;
    }
}