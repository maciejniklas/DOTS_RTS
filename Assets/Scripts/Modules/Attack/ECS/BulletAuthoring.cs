using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public class BulletAuthoring : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float speed;
        [SerializeField] [Min(0)] private int damage;
        
        private class BulletAuthoringBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new BulletData
                {
                    Speed = authoring.speed,
                    Damage = authoring.damage,
                });
            }
        }
    }
}