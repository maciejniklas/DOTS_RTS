using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DOTS_RTS.Modules.UnitsSelection.ECS
{
    [BurstCompile]
    public partial struct SelectableIndicatorsSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var selectableData in SystemAPI.Query<RefRO<SelectableData>>().WithDisabled<SelectableData>())
            {
                var selectedIndicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selectableData.ValueRO.SelectedIndicator);

                selectedIndicatorLocalTransform.ValueRW.Scale = 0f;
            }
            
            foreach (var selectableData in SystemAPI.Query<RefRO<SelectableData>>())
            {
                var selectedIndicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selectableData.ValueRO.SelectedIndicator);

                selectedIndicatorLocalTransform.ValueRW.Scale = selectableData.ValueRO.ActiveIndicatorScale;
            }
        }
    }
}