using DOTS_RTS.Modules.SearchingForTarget.ECS;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Attack.ECS
{
    [BurstCompile]
    public partial struct ShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (shootData, targetData) in SystemAPI.Query<RefRW<ShootData>, RefRO<TargetData>>())
            {
                if (targetData.ValueRO.Target == Entity.Null)
                {
                    if (shootData.ValueRO.Timer >= 0) shootData.ValueRW.Timer = 0f;
                    
                    continue;
                }
                
                shootData.ValueRW.Timer += SystemAPI.Time.DeltaTime;

                if (shootData.ValueRO.Timer < shootData.ValueRO.Cooldown) continue;
                
                shootData.ValueRW.Timer = 0f;

                Debug.Log("Shoot");
            }
        }
    }
}