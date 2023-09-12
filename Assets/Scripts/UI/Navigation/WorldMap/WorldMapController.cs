using System;
using System.Collections.Generic;
using kcp2k;
using Player;
using Quests;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Navigation.WorldMap
{
    public class WorldMapController : MonoBehaviour
    {
        public Camera worldCamera; //world camera with RenderTexture
        public RectTransform worldImage; //RawImage which shows RenderTexture
        public GameObject markerPrefab;
        public Sprite girlSprite;
        public Sprite boySprite;

        private RectTransform helper;

        private PlayerController[] players;

        private void Start()
        {
            helper = (new GameObject ("helper", typeof(RectTransform))).GetComponent<RectTransform>();
            helper.SetParent (worldImage, false);
            helper.anchorMin = Vector2.zero;
            helper.anchorMax = Vector2.zero;
            
            players = PlayerController.GetPlayers();
            worldCamera = GameObject.Find("CameraMap").GetComponent<Camera>();
            foreach (var player in players)
            {
                var marker = Instantiate(markerPrefab, worldImage);
                marker.GetComponent<Image>().sprite = player.isBoy ? boySprite : girlSprite;
                marker.GetComponent<RectTransform>().position = GetPositionOnUI(player.transform.position);
                marker.GetComponent<MapMarker>().target = player.transform;
            }
        }

        public Vector2 GetPositionOnUI(Vector3 worldPosition)
        {
            //first we get screnPoint in camera viewport space
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (worldCamera, worldPosition);
            //then transform it to position in worldImage using its rect
            Vector2 positionInImage = new Vector2 (screenPoint.x * worldImage.rect.width / worldCamera.pixelWidth, 
                screenPoint.y * worldImage.rect.height / worldCamera.pixelHeight);
            
            Debug.Log("worldImage: " + worldImage.rect.width + "x" + worldImage.rect.height);
            Debug.Log("worldCamera: " + worldCamera.pixelWidth + "x" + worldCamera.pixelHeight);
            Debug.Log("screen: " + Screen.width + "x" + Screen.height);
            Debug.Log("before: " + screenPoint.x + "x" + screenPoint.y);
            Debug.Log("after: " + positionInImage.x + "x" + positionInImage.y);

            //after positioning helper to that spot
            helper.anchoredPosition = positionInImage;
            //... return 3D position of that helper for any other RectTransform.position to use
            return positionInImage;
        }
    }
}