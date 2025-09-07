using Unity.Entities;

namespace DOTS_RTS.Modules.Management.UnitySelection
{
    public struct SelectableData : IComponentData, IEnableableComponent
    {
        public float ActiveIndicatorScale;
        public Entity SelectedIndicator;
    }
}