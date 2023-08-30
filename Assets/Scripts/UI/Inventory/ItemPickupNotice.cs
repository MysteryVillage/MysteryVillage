using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemPickupNotice : MonoBehaviour
    { 
        private float _timer = 0f;
        public float timeToDestroy;

        public TextMeshProUGUI text;
        public Image icon;

        private void Update()
        {
            _timer += Time.deltaTime;
            
            // Destroy after X seconds
            if (_timer > timeToDestroy) Destroy(gameObject);
        }
    }
}