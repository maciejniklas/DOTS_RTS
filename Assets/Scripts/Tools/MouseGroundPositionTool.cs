using DOTS_RTS.Patterns;
using UnityEngine;

namespace DOTS_RTS.Tools
{
    public class MouseGroundPositionTool : LazySingleton<MouseGroundPositionTool>
    {
        private Camera _mainCamera;
        private LayerMask _interactionLayer;

        protected override void Awake()
        {
            base.Awake();
            
            _mainCamera = Camera.main;
            _interactionLayer = LayerMask.GetMask(Constants.Layers.GroundLayerName);
        }

        public Vector3 GetPosition()
        {
            var mouseCameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(mouseCameraRay, out var hit, 500, _interactionLayer) ? hit.point : Vector3.zero;
        }
    }
}