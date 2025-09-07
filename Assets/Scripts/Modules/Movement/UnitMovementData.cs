using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_RTS.Modules.Movement
{
    public struct UnitMovementData : IComponentData
    {
        public float Speed;
        public float RotationSpeed;
        public float StoppingDistanceSquared;
        public float3 TargetGroundPosition;
    }
}