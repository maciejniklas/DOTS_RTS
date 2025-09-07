using System;
using DOTS_RTS.Modules.Management;
using DOTS_RTS.Modules.Management.UnitySelection;
using DOTS_RTS.Modules.Movement;
using DOTS_RTS.Patterns;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DOTS_RTS.Tools
{
    public class UnitsSelectionTool : LocalSingleton<UnitsSelectionTool>
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var mouseGroundPosition = MouseGroundPositionTool.Instance.GetPosition();

                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMovementData, SelectableData>().Build(entityManager);
                var unitsMovementData = entityQuery.ToComponentDataArray<UnitMovementData>(Allocator.Temp);

                for (var index = 0; index < unitsMovementData.Length; index++)
                {
                    var unitMovementData = unitsMovementData[index];
                    
                    unitMovementData.TargetGroundPosition = mouseGroundPosition;

                    unitsMovementData[index] = unitMovementData;
                }
                
                entityQuery.CopyFromComponentDataArray(unitsMovementData);
            }
        }
    }
}