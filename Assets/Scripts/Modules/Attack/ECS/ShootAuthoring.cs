using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public class ShootAuthoring : MonoBehaviour
    {
        [SerializeField] private float cooldown;
        
        private class ShootAuthoringBaker : Baker<ShootAuthoring>
        {
            public override void Bake(ShootAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new ShootData
                {
                    Cooldown = authoring.cooldown,
                });
            }
        }
    }
}