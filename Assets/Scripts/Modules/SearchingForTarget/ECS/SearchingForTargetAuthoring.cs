using DOTS_RTS.Modules.Creatures.Models.Enums;
using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.SearchingForTarget.ECS
{
    public class SearchingForTargetAuthoring : MonoBehaviour
    {
        [SerializeField] private Faction targetFaction;
        [SerializeField] private float range;
        
        private class SearchingForTargetAuthoringBaker : Baker<SearchingForTargetAuthoring>
        {
            public override void Bake(SearchingForTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SearchingForTargetData
                {
                    TargetFaction = authoring.targetFaction,
                    Range = authoring.range,
                });
            }
        }
    }
}