using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.SearchingForTarget.ECS
{
    public class TargetAuthoring : MonoBehaviour
    {
        private class TargetAuthoringBaker : Baker<TargetAuthoring>
        {
            public override void Bake(TargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new TargetData());
            }
        }
    }
}