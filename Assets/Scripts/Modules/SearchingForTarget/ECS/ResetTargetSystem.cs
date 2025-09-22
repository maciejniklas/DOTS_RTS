using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DOTS_RTS.Modules.SearchingForTarget.ECS
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct ResetTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var targetData in SystemAPI.Query<RefRW<TargetData>>())
            {
                if (targetData.ValueRO.Target != Entity.Null && (!SystemAPI.Exists(targetData.ValueRO.Target) || !SystemAPI.HasComponent<LocalTransform>(targetData.ValueRO.Target)))
                {
                    targetData.ValueRW.Target = Entity.Null;
                }
            }
        }
    }
}