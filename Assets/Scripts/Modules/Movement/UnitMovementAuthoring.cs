using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_RTS.Modules.Movement
{
    public class UnitMovementAuthoring : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
    
        private class MoveSpeedAuthoringBaker : Baker<UnitMovementAuthoring>
        {
            public override void Bake(UnitMovementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                AddComponent(entity, new UnitMovementData
                {
                    Speed = authoring.speed,
                    RotationSpeed = authoring.rotationSpeed,
                });
            }
        }
    
        public struct UnitMovementData : IComponentData
        {
            public float Speed;
            public float RotationSpeed;
            public float3 TargetGroundPosition;
        }
    }
}