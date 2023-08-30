using Cinemachine;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Waypoint : MonoBehaviour
    {
        public Sprite icon;

        public Image waypointImage;
        
        // The target (location, enemy, etc..)
        public Transform target;
        // UI Text to display the distance
        public Text meter;
        // To adjust the position of the icon
        public Vector3 offset;

        public Camera cam;

        private void Start()
        {
            CinemachineCore.CameraUpdatedEvent.AddListener(Calculate);
            cam = NetworkClient.localPlayer.GetComponent<PlayerController>().cinemachineMainCamera.GetComponent<Camera>();
            waypointImage.sprite = icon;
        }

        public void Calculate(CinemachineBrain brain)
        {
            var pos = GetComponentInParent<PlayerUi>().GetScreenPosition(target, waypointImage, offset); 

            // Update the marker's position
            waypointImage.transform.position = pos;
            // Change the meter text to the distance with the meter unit 'm'
            // meter.text = ((int)Vector3.Distance(target.position, transform.position)) + "m";
        }
    }
}