using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Creatures.ECS
{
    public class CreatureTagAuthoring : MonoBehaviour
    {
        private class CreatureTagAuthoringBaker : Baker<CreatureTagAuthoring>
        {
            public override void Bake(CreatureTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new CreatureTag());;
            }
        }
    }
}