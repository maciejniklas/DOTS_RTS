using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public class ShootAuthoring : MonoBehaviour
    {
        [SerializeField] private float cooldown;
        [SerializeField] private int damage;
        [SerializeField] private float attackDistance;
        [SerializeField] private Transform bulletSpawnPoint;
        
        private class ShootAuthoringBaker : Baker<ShootAuthoring>
        {
            public override void Bake(ShootAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new ShootData
                {
                    Cooldown = authoring.cooldown,
                    Damage = authoring.damage,
                    AttackDistance = authoring.attackDistance,
                    BulletLocalSpawnPoint =  authoring.bulletSpawnPoint?.localPosition ?? float3.zero,
                });
            }
        }
    }
}