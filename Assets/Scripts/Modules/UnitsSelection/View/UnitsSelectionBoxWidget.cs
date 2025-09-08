using System;
using System.Collections;
using DOTS_RTS.Modules.UnitsSelection.Controllers;
using DOTS_RTS.Modules.UnitsSelection.Model.EventArgs;
using UnityEngine;

namespace DOTS_RTS.Modules.UnitsSelection.View
{
    public class UnitsSelectionBoxWidget : MonoBehaviour
    {
        [SerializeField] private RectTransform selectionBoxRectTransform;

        private float _canvasScale;
        private bool _areEventsSubscribed;

        private void Awake()
        {
            if (selectionBoxRectTransform.gameObject.activeSelf) selectionBoxRectTransform.gameObject.SetActive(false);
            
            ResetSelectionBox();
        }

        private IEnumerator Start()
        {
            if (!_areEventsSubscribed)
            {
                yield return new WaitUntil(() => UnitsSelectionTool.Instance != null);
                
                UnitsSelectionTool.Instance.OnSelectionStarted += OnSelectionStarted;
                UnitsSelectionTool.Instance.OnSelectionChanged += OnSelectionChanged;
                UnitsSelectionTool.Instance.OnSelectionFinished += OnSelectionFinished;

                _areEventsSubscribed = true;
            }
            
            var canvas = GetComponentInParent<Canvas>();
            _canvasScale = canvas != null ? canvas.transform.localScale.x : 1f;
        }

        private void OnEnable()
        {
            if (!_areEventsSubscribed && UnitsSelectionTool.Instance != null)
            {
                UnitsSelectionTool.Instance.OnSelectionStarted += OnSelectionStarted;
                UnitsSelectionTool.Instance.OnSelectionChanged += OnSelectionChanged;
                UnitsSelectionTool.Instance.OnSelectionFinished += OnSelectionFinished;

                _areEventsSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (_areEventsSubscribed && UnitsSelectionTool.Instance != null)
            {
                UnitsSelectionTool.Instance.OnSelectionStarted -= OnSelectionStarted;
                UnitsSelectionTool.Instance.OnSelectionChanged -= OnSelectionChanged;
                UnitsSelectionTool.Instance.OnSelectionFinished -= OnSelectionFinished;

                _areEventsSubscribed = false;
            }
        }

        private void ResetSelectionBox()
        {
            selectionBoxRectTransform.anchoredPosition = Vector2.zero;
            selectionBoxRectTransform.sizeDelta = Vector2.zero;
        }

        private void UpdateSelectionBox(Rect rect)
        {
            selectionBoxRectTransform.anchoredPosition = rect.position / _canvasScale;
            selectionBoxRectTransform.sizeDelta = rect.size / _canvasScale;
        }

        private void OnSelectionStarted(object sender, EventArgs args)
        {
            if (!selectionBoxRectTransform.gameObject.activeSelf) selectionBoxRectTransform.gameObject.SetActive(true);
        }
        
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            UpdateSelectionBox(args.SelectionArea);
        }
        
        private void OnSelectionFinished(object sender, EventArgs args)
        {
            if (selectionBoxRectTransform.gameObject.activeSelf) selectionBoxRectTransform.gameObject.SetActive(false);
            
            ResetSelectionBox();
        }
    }
}