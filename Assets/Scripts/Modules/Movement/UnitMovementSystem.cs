using Unity.Burst;
using Unity.Entities;

namespace DOTS_RTS.Modules.Movement
{
    [BurstCompile]
    public partial struct UnitMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var unitMovementJob = new UnitMovementJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            };
            
            unitMovementJob.ScheduleParallel();
        }
    }
}