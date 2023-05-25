using UnityEngine;

namespace UI
{
    public class Minimap : MonoBehaviour
    {
        public Transform player;
        public Transform cam;

        private void LateUpdate()
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(90f, cam.eulerAngles.y, 0f);
        }
    }
}
