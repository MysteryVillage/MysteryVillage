using System;
using UnityEngine;

namespace UI.Navigation.WorldMap
{
    public class MapMarker : MonoBehaviour
    {
        public Transform target;
        public WorldMapController mapController;
        public RectTransform rectTransform;

        private void Start()
        {
            mapController = GetComponentInParent<WorldMapController>();
        }

        private void Update()
        {
            rectTransform.anchoredPosition = mapController.GetPositionOnUI(target.position);
        }
    }
}