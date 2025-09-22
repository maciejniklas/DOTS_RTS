using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.General.ECS
{
    public class EntitiesReferencesAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab; 
        
        private class EntitiesReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring>
        {
            public override void Bake(EntitiesReferencesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new EntitiesReferencesData()
                {
                    BulletPrefabEntity = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}