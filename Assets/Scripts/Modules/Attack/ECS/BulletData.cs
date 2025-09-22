using Unity.Entities;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public struct BulletData : IComponentData
    {
        public float Speed;
        public int Damage;
    }
}