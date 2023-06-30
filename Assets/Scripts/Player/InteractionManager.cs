using Items;
using Mirror;
using Quests;
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
        public GameObject interactionPanel;
        public Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (isLocalPlayer) {
                if (Time.time - _lastCheckTime > checkRate)
                {
                    _lastCheckTime = Time.time;
                    
                    Ray ray = new Ray(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward));
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
                    {
                        if (hit.collider.gameObject != _curInteractGameObject)
                        {
                            if (_curInteractGameObject != null)
                            {
                                _curInteractGameObject.GetComponent<HighlightOutline>()?.ToggleOutline(false);
                            }
                            _curInteractGameObject = hit.collider.gameObject;
                            _curInteractable = hit.collider.GetComponent<IIinteractable>();
                            Debug.Log("Is Server: " + isServer);
                            _curInteractGameObject.GetComponent<HighlightOutline>()?.ToggleOutline(true);
                            SetPromptText();
                        }
                    }
                    else
                    {
                        if (_curInteractGameObject != null)
                        {
                            _curInteractGameObject.GetComponent<HighlightOutline>()?.ToggleOutline(false);
                        }
                        _curInteractGameObject = null;
                        _curInteractable = null;
                        interactionPanel.SetActive(false);
                    }
                }
            }
        }

        void SetPromptText()
        {
            interactionPanel.SetActive(true);
            promptText.text = string.Format("{0}", _curInteractable.GetInteractPrompt());
        }

        public void OnInteractInput(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && _curInteractable != null)
            {
                if (_curInteractGameObject) {
                    OnInteractCmd(_curInteractGameObject.GetComponent<NetworkIdentity>().netId);
                    _curInteractGameObject = null;
                    _curInteractable = null;
                    interactionPanel.SetActive(false);
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

        [Command]
        public void AddQuest(Quest quest)
        {
            QuestManager.Current.AddQuest(quest);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Ray ray = new Ray(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward));
            Gizmos.DrawRay(ray.origin, ray.direction * maxCheckDistance);
        }
    
    }

    public interface IIinteractable
    {
        string GetInteractPrompt();
        void OnInteract(uint networkIdentifier);
    }
}