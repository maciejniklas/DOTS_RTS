using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_RTS.Modules.Attack.ECS
{
    public struct ShootData : IComponentData
    {
        public float Timer;
        public float Cooldown;
        public int Damage;
        public float AttackDistance;
        public float3 BulletLocalSpawnPoint;
    }
}