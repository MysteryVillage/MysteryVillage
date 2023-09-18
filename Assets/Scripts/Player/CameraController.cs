using Cinemachine;
using Mirror;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera;
    

        public void OnZoom(InputAction.CallbackContext context)
        {
            if (virtualCamera == null) return;
        
            if (virtualCamera.m_Lens.FieldOfView == 40)
            {
                virtualCamera.m_Lens.FieldOfView = 70;
                return;
            
            } 
        
            if (virtualCamera.m_Lens.FieldOfView == 70)
            {
                virtualCamera.m_Lens.FieldOfView = 40;
            }
        }

        [YarnCommand("fade_out")]
        public static void FadeOut()
        {
            FindObjectOfType<Crossfade>().FadeOut();
        }

        [YarnCommand("fade_in")]
        public static void FadeIn()
        {
            FindObjectOfType<Crossfade>().FadeIn();
        }

        [YarnCommand("crossfade")]
        public static void Crossfade(int seconds)
        {
            FindObjectOfType<Crossfade>().StartCrossfade(seconds);
        }

        [YarnCommand("change_cam")]
        public static void ChangeCamera(string camera)
        {
            var cam = GameObject.Find(camera);
            
            if (cam == null)
            {
                Debug.Log("No camera found!");
                return;
            }

            var localPlayer = NetworkClient.localPlayer;
            var controller = localPlayer.GetComponent<PlayerController>();
            
            controller.cinemachineFollowCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            cam.GetComponent<CinemachineVirtualCamera>().Priority = 1;
            controller.lastUsedCamera = cam.GetComponent<CinemachineVirtualCamera>();
        }

        [YarnCommand("reset_cam")]
        public static void ResetCamera()
        {
            var localPlayer = NetworkClient.localPlayer;
            var controller = localPlayer.GetComponent<PlayerController>();
            
            controller.cinemachineFollowCamera.GetComponent<CinemachineVirtualCamera>().Priority = 1;
            controller.lastUsedCamera.Priority = 0;
        }

    }
}
