using Unity.Entities;

namespace DOTS_RTS.Modules.SearchingForTarget.ECS
{
    public struct TargetData : IComponentData
    {
        public Entity Target;
    }
}