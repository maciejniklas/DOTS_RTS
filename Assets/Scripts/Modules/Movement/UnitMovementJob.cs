using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DOTS_RTS.Modules.Movement
{
    [BurstCompile]
    public partial struct UnitMovementJob : IJobEntity
    {
        public float DeltaTime;
            
        public void Execute(ref LocalTransform localTransform, UnitMovementData unitMovementData, ref PhysicsVelocity physicsVelocity)
        {
            var movementDirection = unitMovementData.TargetGroundPosition - localTransform.Position;

            if (math.lengthsq(movementDirection) <= unitMovementData.StoppingDistanceSquared)
            {
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;

                return;
            }
                
            var narmalizedMovementDirection = math.normalize(movementDirection);

            physicsVelocity.Linear = narmalizedMovementDirection * unitMovementData.Speed;
            physicsVelocity.Angular = float3.zero;

            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(narmalizedMovementDirection, math.up()),
                DeltaTime * unitMovementData.RotationSpeed);
        }
    }
}