using Authorings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, movementData, physicsVelocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementAuthoring.MovementData>, RefRW<PhysicsVelocity>>())
            {
                var movementDirection = math.normalize(math.right());
                physicsVelocity.ValueRW.Linear = movementDirection * movementData.ValueRO.Speed;
                physicsVelocity.ValueRW.Angular = float3.zero;
                
                localTransform.ValueRW.Rotation = quaternion.LookRotation(movementDirection, math.up());
            }
        }
    }
}