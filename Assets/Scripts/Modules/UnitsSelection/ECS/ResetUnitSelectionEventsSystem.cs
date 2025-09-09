using Unity.Burst;
using Unity.Entities;

namespace DOTS_RTS.Modules.UnitsSelection.ECS
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ResetUnitSelectionEventsSystem : ISystem
    { 
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var selectableData in SystemAPI.Query<RefRW<SelectableData>>().WithDisabled<SelectableData>())
            {
                selectableData.ValueRW.OnSelected = false;
                selectableData.ValueRW.OnDeselected = false;
            }
        }
    }
}