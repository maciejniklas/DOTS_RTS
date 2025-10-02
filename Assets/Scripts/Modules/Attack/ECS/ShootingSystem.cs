using DOTS_RTS.Modules.General.ECS;
using DOTS_RTS.Modules.Health.ECS;
using DOTS_RTS.Modules.Movement.ECS;
using DOTS_RTS.Modules.SearchingForTarget.ECS;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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

            foreach (var (shootData, targetData, localTransform, unitMovementData) in SystemAPI.Query<RefRW<ShootData>, RefRO<TargetData>, RefRW<LocalTransform>, RefRW<UnitMovementData>>())
            {
                if (targetData.ValueRO.Target == Entity.Null)
                {
                    if (shootData.ValueRO.Timer >= 0) shootData.ValueRW.Timer = 0f;

                    continue;
                }

                var targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(targetData.ValueRO.Target);

                if (math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) > shootData.ValueRO.AttackDistance * shootData.ValueRO.AttackDistance)
                {
                    unitMovementData.ValueRW.TargetGroundPosition = targetLocalTransform.Position;
                    continue;
                }
                else
                {
                    unitMovementData.ValueRW.TargetGroundPosition = localTransform.ValueRO.Position;
                }
                
                var aimDirection = math.normalize(targetLocalTransform.Position - localTransform.ValueRO.Position);
                var aimRotation = quaternion.LookRotation(aimDirection, math.up());

                localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, aimRotation, SystemAPI.Time.DeltaTime * unitMovementData.ValueRO.RotationSpeed);

                shootData.ValueRW.Timer += SystemAPI.Time.DeltaTime;

                if (shootData.ValueRO.Timer < shootData.ValueRO.Cooldown) continue;

                shootData.ValueRW.Timer = 0f;

                var bulletEntity = state.EntityManager.Instantiate(entitiesReferences.BulletPrefabEntity);
                var bulletWorldSpawnPoint = localTransform.ValueRO.TransformPoint(shootData.ValueRO.BulletLocalSpawnPoint);
                SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(bulletWorldSpawnPoint));

                var bulletData = SystemAPI.GetComponentRW<BulletData>(bulletEntity);
                bulletData.ValueRW.Damage = shootData.ValueRO.Damage;

                var bulletTarget = SystemAPI.GetComponentRW<TargetData>(bulletEntity);
                bulletTarget.ValueRW.Target = targetData.ValueRO.Target;
            }
        }
    }
}