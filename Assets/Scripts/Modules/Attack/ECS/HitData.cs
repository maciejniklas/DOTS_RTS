using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public struct HitData : IComponentData
    {
        public float3 HitLocalPoint;
    }
}