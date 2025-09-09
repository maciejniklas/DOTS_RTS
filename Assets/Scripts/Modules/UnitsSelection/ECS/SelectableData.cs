using Unity.Entities;

namespace DOTS_RTS.Modules.UnitsSelection.ECS
{
    public struct SelectableData : IComponentData, IEnableableComponent
    {
        public Entity SelectedIndicator;
        public float ActiveIndicatorScale;
        public bool OnSelected;
        public bool OnDeselected;
    }
}