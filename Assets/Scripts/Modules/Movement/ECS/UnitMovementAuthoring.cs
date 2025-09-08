using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Movement.ECS
{
    public class UnitMovementAuthoring : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float stoppingDistance;
    
        private class MoveSpeedAuthoringBaker : Baker<UnitMovementAuthoring>
        {
            public override void Bake(UnitMovementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                AddComponent(entity, new UnitMovementData
                {
                    Speed = authoring.speed,
                    RotationSpeed = authoring.rotationSpeed,
                    StoppingDistanceSquared = authoring.stoppingDistance * authoring.stoppingDistance,
                });
            }
        }
    }
}