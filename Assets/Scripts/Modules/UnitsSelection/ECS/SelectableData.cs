using Unity.Entities;

namespace DOTS_RTS.Modules.UnitsSelection.ECS
{
    public struct SelectableData : IComponentData, IEnableableComponent
    {
        public float ActiveIndicatorScale;
        public Entity SelectedIndicator;
    }
}