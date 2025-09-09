using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DOTS_RTS.Modules.UnitsSelection.ECS
{
    [BurstCompile]
    [UpdateBefore(typeof(ResetUnitSelectionEventsSystem))]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct SelectableIndicatorsSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var selectableData in SystemAPI.Query<RefRO<SelectableData>>().WithPresent<SelectableData>())
            {
                if (selectableData.ValueRO.OnSelected)
                {
                    var selectedIndicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selectableData.ValueRO.SelectedIndicator);
                    selectedIndicatorLocalTransform.ValueRW.Scale = selectableData.ValueRO.ActiveIndicatorScale;
                }
                
                if (selectableData.ValueRO.OnDeselected)
                {
                    var selectedIndicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selectableData.ValueRO.SelectedIndicator);
                    selectedIndicatorLocalTransform.ValueRW.Scale = 0f;
                }
            }
        }
    }
}