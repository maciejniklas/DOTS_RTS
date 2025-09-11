using DOTS_RTS.Modules.Creatures.ECS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_RTS.Modules.SearchingForTarget.ECS
{
    [BurstCompile]
    public partial struct SearchingForTargetSystem : ISystem {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionsWorld = physicsWorldSingleton.CollisionWorld;
            var distanceHits = new NativeList<DistanceHit>(Allocator.Temp);
            var unitsLayer = LayerMask.NameToLayer(Constants.Layers.UnitLayerName);
            var collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << unitsLayer,
                GroupIndex = 0,
            };

            foreach (var (searchForTargetData, localTransform, targetData) in SystemAPI.Query<RefRO<SearchingForTargetData>, RefRO<LocalTransform>, RefRW<TargetData>>())
            {
                distanceHits.Clear();

                var targetFactionHitsCount = 0;

                if (collisionsWorld.OverlapSphere(localTransform.ValueRO.Position, searchForTargetData.ValueRO.Range, ref distanceHits, collisionFilter))
                {
                    foreach (var hit in distanceHits)
                    {
                        var creatureData = SystemAPI.GetComponent<CreatureData>(hit.Entity);

                        if (creatureData.Faction == searchForTargetData.ValueRO.TargetFaction)
                        {
                            targetData.ValueRW.Target = hit.Entity;
                            targetFactionHitsCount++;
                        }
                    }
                }
                
                if (targetFactionHitsCount <= 0)
                {
                    targetData.ValueRW.Target = Entity.Null;
                }
            }
        }
    }
}