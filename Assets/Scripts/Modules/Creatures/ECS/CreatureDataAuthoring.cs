using System;
using DOTS_RTS.Modules.Creatures.Models.Enums;
using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Modules.Creatures.ECS
{
    public class CreatureDataAuthoring : MonoBehaviour
    {
        [SerializeField] private Faction faction;
        
        private class CreatureDataAuthoringBaker : Baker<CreatureDataAuthoring>
        {
            public override void Bake(CreatureDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new CreatureData
                {
                    Faction = authoring.faction,
                });

                switch (authoring.faction)
                {
                    case Faction.Soldiers:
                        AddComponent(entity, new FactionTagSoldiers());
                        break;
                    case Faction.Zombies:
                        AddComponent(entity, new FactionTagZombies());
                        break;
                }
            }
        }
    }
}