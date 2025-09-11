using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Health.ECS
{
    public class HealthAuthoring : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int health;
        
        private class HealthAuthoringBaker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new HealthData
                {
                    Health = authoring.health,
                });
            }
        }
    }
}