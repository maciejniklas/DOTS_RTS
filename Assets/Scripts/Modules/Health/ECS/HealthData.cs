using Unity.Entities;

namespace DOTS_RTS.Modules.Health.ECS
{
    public struct HealthData : IComponentData
    {
        public int Health;
    }
}