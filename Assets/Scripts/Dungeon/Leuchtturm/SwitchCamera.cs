using UnityEngine;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    

    private bool isUsingCamera1 = true;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LT_CamTrigger"))
        {
            Debug.Log("Trigger Cam Switch");
            CamSwitch();
        }
    }

    private void CamSwitch()
    {
        if (isUsingCamera1)
        {
            camera1.Priority = 10;
            camera2.Priority = 20;
        }
        else
        {
            camera1.Priority = 20;
            camera2.Priority = 10;
        }
     
        isUsingCamera1 = !isUsingCamera1;
    }
}
