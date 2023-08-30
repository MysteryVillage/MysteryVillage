using UnityEngine;

namespace UI
{
    public class Minimap : MonoBehaviour
    {
        public Transform player;
        public Transform cam;
        public RectTransform compass;

        private void LateUpdate()
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(90f, cam.eulerAngles.y, 0f);

            compass.localRotation = Quaternion.Euler(0f, 0f, cam.eulerAngles.y);
        }
    }
}
