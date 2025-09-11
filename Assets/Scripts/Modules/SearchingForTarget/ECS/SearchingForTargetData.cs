using DOTS_RTS.Modules.Creatures.Models.Enums;
using Unity.Entities;

namespace DOTS_RTS.Modules.SearchingForTarget.ECS
{
    public struct SearchingForTargetData : IComponentData
    {
        public Faction TargetFaction;
        public float Range;
    }
}