using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.UnitsSelection.ECS
{
    public class SelectableAuthoring : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float activeIndicatorScale;
        [SerializeField] private GameObject selectedIndicator;
        
        private class SelectableAuthoringBaker : Baker<SelectableAuthoring>
        {
            public override void Bake(SelectableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SelectableData
                {
                    ActiveIndicatorScale = authoring.activeIndicatorScale,
                    SelectedIndicator = GetEntity(authoring.selectedIndicator, TransformUsageFlags.Dynamic),
                });
                SetComponentEnabled<SelectableData>(entity, false);
            }
        }
    }
}