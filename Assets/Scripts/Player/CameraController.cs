using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

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

}
