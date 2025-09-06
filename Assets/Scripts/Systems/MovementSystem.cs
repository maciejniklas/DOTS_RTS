using Authorings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, movementData) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementAuthoring.MovementData>>())
            {
                localTransform.ValueRW.Position += new float3(1, 0, 0) * movementData.ValueRO.Speed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}