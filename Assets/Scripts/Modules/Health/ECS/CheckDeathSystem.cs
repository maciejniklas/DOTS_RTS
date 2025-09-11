using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DOTS_RTS.Modules.Health.ECS
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct CheckDeathSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommendBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (healthData, entity) in SystemAPI.Query<RefRO<HealthData>>().WithEntityAccess())
            {
                if (healthData.ValueRO.Health <= 0)
                {
                    entityCommendBuffer.DestroyEntity(entity);
                }
            }
        }
    }
}