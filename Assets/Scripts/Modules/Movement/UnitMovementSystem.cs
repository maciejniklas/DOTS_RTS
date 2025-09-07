using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DOTS_RTS.Modules.Movement
{
    [BurstCompile]
    public partial struct UnitMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, unitMovementData, physicsVelocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMovementAuthoring.UnitMovementData>, RefRW<PhysicsVelocity>>())
            {
                var movementDirection = math.normalize(unitMovementData.ValueRO.TargetGroundPosition - localTransform.ValueRO.Position);

                physicsVelocity.ValueRW.Linear = movementDirection * unitMovementData.ValueRO.Speed;
                physicsVelocity.ValueRW.Angular = float3.zero;

                localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(movementDirection, math.up()),
                    SystemAPI.Time.DeltaTime * unitMovementData.ValueRO.RotationSpeed);
            }
        }
    }
}