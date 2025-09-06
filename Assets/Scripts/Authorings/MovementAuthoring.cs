using Unity.Entities;
using UnityEngine;

namespace Authorings
{
    public class MovementAuthoring : MonoBehaviour
    {
        [SerializeField] private float speed;
    
        private class MoveSpeedAuthoringBaker : Baker<MovementAuthoring>
        {
            public override void Bake(MovementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                AddComponent(entity, new MovementData
                {
                    Speed = authoring.speed
                });
            }
        }
    
        public struct MovementData : IComponentData
        {
            public float Speed;
        }
    }
}