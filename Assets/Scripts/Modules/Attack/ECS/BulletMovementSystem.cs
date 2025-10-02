using DOTS_RTS.Modules.Health.ECS;
using DOTS_RTS.Modules.SearchingForTarget.ECS;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS_RTS.Modules.Attack.ECS
{
    [BurstCompile]
    public partial struct BulletMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (localTransform, bulletData, targetData, bulletEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<BulletData>, RefRO<TargetData>>().WithEntityAccess())
            {
                if (targetData.ValueRO.Target == Entity.Null)
                {
                    entityCommandBuffer.DestroyEntity(bulletEntity);
                    continue;
                }
                
                var targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(targetData.ValueRO.Target);
                var targetHitData = SystemAPI.GetComponent<HitData>(targetData.ValueRO.Target);
                var targetPosition = targetLocalTransform.TransformPoint(targetHitData.HitLocalPoint);
                var movementDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);
                var distanceBeforeMovement = math.distancesq(localTransform.ValueRO.Position, targetPosition);
                
                localTransform.ValueRW.Position += movementDirection * bulletData.ValueRO.Speed * SystemAPI.Time.DeltaTime;

                var distanceAfterMovement = math.distancesq(localTransform.ValueRO.Position, targetPosition);

                if (distanceAfterMovement <= 0.1f || distanceBeforeMovement < distanceAfterMovement)
                {
                    var targetHealthData = SystemAPI.GetComponentRW<HealthData>(targetData.ValueRO.Target);

                    targetHealthData.ValueRW.Health -= bulletData.ValueRO.Damage;
                    
                    entityCommandBuffer.DestroyEntity(bulletEntity);
                }
            }
        }
    }
}