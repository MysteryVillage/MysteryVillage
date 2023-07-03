using Cinemachine;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class FocusOfInterest : MonoBehaviour, IIinteractable
    {
        [FormerlySerializedAs("camera")] public CinemachineVirtualCamera foiCamera;
        protected bool Active = false;

        public string GetInteractPrompt()
        {
            return Active ? "Nicht mehr ansehen" : "Ansehen";
        }

        public void OnInteract(uint networkIdentifier)
        {
            var localPlayerIdentity = NetworkClient.localPlayer;
            if (NetworkClient.localPlayer.netId == networkIdentifier)
            { 
                var localPlayer = localPlayerIdentity.gameObject;
                var controller = localPlayer.GetComponent<PlayerController>();
                if (Active)
                {
                    controller.cinemachineFollowCamera.GetComponent<CinemachineVirtualCamera>().Priority = 1;
                    foiCamera.Priority = 0;
                    controller.ToggleMovement(true);
                }
                else
                {
                    controller.cinemachineFollowCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                    foiCamera.Priority = 1;
                    controller.ToggleMovement(false);
                }
                Active = !Active;
            }
        }
    }
}