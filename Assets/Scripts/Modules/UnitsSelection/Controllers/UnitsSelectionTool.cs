using System;
using DOTS_RTS.Modules.Creatures.ECS;
using DOTS_RTS.Modules.Movement.ECS;
using DOTS_RTS.Modules.UnitsSelection.ECS;
using DOTS_RTS.Modules.UnitsSelection.Model.EventArgs;
using DOTS_RTS.Patterns;
using DOTS_RTS.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_RTS.Modules.UnitsSelection.Controllers
{
    public class UnitsSelectionTool : LocalSingleton<UnitsSelectionTool>
    {
        public event EventHandler OnSelectionStarted;
        public event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        public event EventHandler OnSelectionFinished;

        private Camera _mainCamera;
        private Vector2 _lastLeftMouseButtonDownPosition;
        private bool _isLeftMouseButtonDown;

        protected override void Awake()
        {
            base.Awake();
            
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastLeftMouseButtonDownPosition = Input.mousePosition;
                _isLeftMouseButtonDown = true;
                
                OnSelectionStarted?.Invoke(this, EventArgs.Empty);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isLeftMouseButtonDown = false;
                
                var mousePosition = Input.mousePosition;
                var selectionRectArea = GetSelectionRectArea(_lastLeftMouseButtonDownPosition, mousePosition);
                var isMultipleSelection = selectionRectArea.width * selectionRectArea.height >= Constants.Values.MinimumAreaForMultipleSelection;

                if (isMultipleSelection)
                {
                    // Iterate through all entities with CreatureTag and SelectableData components and set their SelectableData.Active property to true if they are inside the selection area.
                    
                    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                    var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, CreatureTag>().WithPresent<SelectableData>().Build(entityManager);
                    var entitiesArray = entityQuery.ToEntityArray(Allocator.Temp);
                    var localTransforms = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                    for (var index = 0; index < localTransforms.Length; index++)
                    {
                        var localTransform = localTransforms[index];
                        var creatureScreenPosition = _mainCamera.WorldToScreenPoint(localTransform.Position);
                    
                        entityManager.SetComponentEnabled<SelectableData>(entitiesArray[index], selectionRectArea.Contains(creatureScreenPosition));
                    }
                }
                else
                {
                    // Iterate through all entities with CreatureTag and SelectableData components and set their SelectableData.Active property to false.
                    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                    var selectedUnitsEntityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<CreatureTag>().WithPresent<SelectableData>().Build(entityManager);
                    var entitiesArray = selectedUnitsEntityQuery.ToEntityArray(Allocator.Temp);

                    foreach (var entity in entitiesArray)
                    {
                        entityManager.SetComponentEnabled<SelectableData>(entity, false);
                    }
                    
                    // Cast ray from mouse position to find entity with SelectableData component.
                    var physicsWorldSingletonEntityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>().Build(entityManager);
                    var physicsWorldSingleton = physicsWorldSingletonEntityQuery.GetSingleton<PhysicsWorldSingleton>();
                    var collisionsWorld = physicsWorldSingleton.CollisionWorld;
                    var ray = _mainCamera.ScreenPointToRay(mousePosition);
                    var unitsLayer = LayerMask.NameToLayer(Constants.Layers.UnitLayerName);
                    var raycastInput = new RaycastInput
                    {
                        Start = ray.GetPoint(0),
                        End = ray.GetPoint(1000),
                        Filter = new CollisionFilter
                        {
                            BelongsTo = ~0u,
                            CollidesWith = 1u << unitsLayer,
                            GroupIndex = 0,
                        },
                    };

                    if (collisionsWorld.CastRay(raycastInput, out var hit) && entityManager.HasComponent<SelectableData>(hit.Entity))
                    {
                        entityManager.SetComponentEnabled<SelectableData>(hit.Entity, true);
                    }
                }
                
                OnSelectionFinished?.Invoke(this, EventArgs.Empty);
            }
            else if (_isLeftMouseButtonDown && Input.GetMouseButton(0))
            {
                OnSelectionChanged?.Invoke(this, new SelectionChangedEventArgs { SelectionArea = GetSelectionRectArea(_lastLeftMouseButtonDownPosition, Input.mousePosition) });
            }
            
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

        private Rect GetSelectionRectArea(Vector2 point1, Vector2 point2)
        {
            var lowerLeftCorner = Vector2.Min(point1, point2);
            var upperRightCorner = Vector2.Max(point1, point2);
            
            return new Rect(lowerLeftCorner, upperRightCorner - lowerLeftCorner);
        }
    }
}