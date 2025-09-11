using Unity.Entities;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public struct ShootData : IComponentData
    {
        public float Timer;
        public float Cooldown;
    }
}