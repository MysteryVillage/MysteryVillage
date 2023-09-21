using UnityEngine;
using Cinemachine;
using Mirror;
using Player;
using Unity.VisualScripting;

public class SwitchCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cam;


    private Vector3 defaultCam = new Vector3(1f, 0f, 0f);
    private Vector3 LTcam = new Vector3(0.4f, 0.2f, -1f);
    private CinemachineComposer composer;
    private Cinemachine3rdPersonFollow follow;
    private bool isUsingCamera1 = true;
    
   
    private void Start()
    {
        follow = cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        
        CamSwitch();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LT_CamTrigger"))
        {
            isUsingCamera1 = false;
            CamSwitch();
            GetComponent<CameraController>().ZoomOut();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LT_CamTrigger"))
        {
            isUsingCamera1 = true;
            CamSwitch();
        }
    }

    private void CamSwitch()
    {
        if (isUsingCamera1)
        {
            if (follow == null) return;
            follow.CameraDistance = 4;
            follow.ShoulderOffset = defaultCam;
        }
        else
        {
            if (follow == null) return;
            follow.CameraDistance = 1;
            follow.ShoulderOffset = LTcam;
        }
        
        isUsingCamera1 = !isUsingCamera1;
    }
}
