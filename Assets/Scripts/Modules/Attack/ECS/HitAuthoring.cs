using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public class HitAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform hitPoint;
        
        private class ShootTargetAuthoringBaker : Baker<HitAuthoring>
        {
            public override void Bake(HitAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new HitData
                {
                    HitLocalPoint = authoring.hitPoint?.localPosition ?? float3.zero,
                });
            }
        }
    }
}