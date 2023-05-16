using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractionManager : NetworkBehaviour
    {
        public float checkRate = .05f;
        private float _lastCheckTime;
        public float maxCheckDistance;
        public LayerMask layerMask;

        private GameObject _curInteractGameObject;
        private IIinteractable _curInteractable;
        public TextMeshProUGUI promptText;
        public Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Time.time - _lastCheckTime > checkRate)
            {
                _lastCheckTime = Time.time;

                Ray ray = cam.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
                {
                    if (hit.collider.gameObject != _curInteractGameObject)
                    {
                        _curInteractGameObject = hit.collider.gameObject;
                        _curInteractable = hit.collider.GetComponent<IIinteractable>();
                        SetPromptText();
                    }
                }
                else
                {
                    _curInteractGameObject = null;
                    _curInteractable = null;
                    promptText.gameObject.SetActive(false);
                }
            }
        }

        void SetPromptText()
        {
            promptText.gameObject.SetActive(true);
            promptText.text = string.Format("<b>[E]</b> {0}", _curInteractable.GetInteractPrompt());
        }

        public void OnInteractInput(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && _curInteractable != null)
            {
                if (_curInteractGameObject) {
                    OnInteractCmd(_curInteractGameObject.GetComponent<NetworkIdentity>().netId);
                    _curInteractGameObject = null;
                    _curInteractable = null;
                    promptText.gameObject.SetActive(false);
                }
            }
        }

        [Command]
        public void OnInteractCmd(uint id)
        {
            GameObject interGo;
            if (_curInteractGameObject)
            {
                interGo = _curInteractGameObject;
            }
            else
            {
                interGo = NetworkServer.spawned[id].gameObject;
            }
            var inter = interGo.GetComponent<IIinteractable>();
            inter.OnInteract(GetComponent<NetworkIdentity>().netId);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Gizmos.DrawRay(ray.origin, ray.direction * maxCheckDistance);
        }
    
    }

    public interface IIinteractable
    {
        string GetInteractPrompt();
        void OnInteract(uint networkIdentifier);
    }
}