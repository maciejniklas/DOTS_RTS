using DOTS_RTS.Modules.General.ECS;
using DOTS_RTS.Modules.Health.ECS;
using DOTS_RTS.Modules.SearchingForTarget.ECS;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_RTS.Modules.Attack.ECS
{
    [BurstCompile]
    public partial struct ShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferencesData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferencesData>();
            
            foreach (var (shootData, targetData, localTransform) in SystemAPI.Query<RefRW<ShootData>, RefRO<TargetData>, RefRO<LocalTransform>>())
            {
                if (targetData.ValueRO.Target == Entity.Null)
                {
                    if (shootData.ValueRO.Timer >= 0) shootData.ValueRW.Timer = 0f;
                    
                    continue;
                }
                
                shootData.ValueRW.Timer += SystemAPI.Time.DeltaTime;

                if (shootData.ValueRO.Timer < shootData.ValueRO.Cooldown) continue;
                
                shootData.ValueRW.Timer = 0f;
                
                var bulletEntity = state.EntityManager.Instantiate(entitiesReferences.BulletPrefabEntity);
                SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                
                var bulletData = SystemAPI.GetComponentRW<BulletData>(bulletEntity);
                bulletData.ValueRW.Damage = shootData.ValueRO.Damage;

                var bulletTarget = SystemAPI.GetComponentRW<TargetData>(bulletEntity);
                bulletTarget.ValueRW.Target = targetData.ValueRO.Target;
            }
        }
    }
}