using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations.Rigging;

public class ArmTracking : MonoBehaviour
{
    public Transform Target;
    public Rig ArmRig;
    public Camera MyCamera;
    public float RotationAngle = 45.0f;

    public float RetargetSpeed = 15f;
    public float MaxAngle = 180f;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Transform tracking = null;
        Vector3 targetPos = transform.position + (transform.forward * 2f);
        float rigWeight = 0;     
       
        if (tracking != null)
        {
            targetPos = tracking.position;
            rigWeight = 1;
        }
        else
        {
            float angle = Vector3.Angle(transform.forward, MyCamera.transform.forward);
            if (angle < MaxAngle)
            {
                targetPos = transform.position + MyCamera.transform.forward;
                rigWeight = 1;
            }
        }
        Target.position = Vector3.Lerp(Target.position, targetPos, Time.deltaTime * RetargetSpeed);
        //Target.position = Quaternion.AngleAxis(RotationAngle, Vector3.up) * Target.position;
        ArmRig.weight = Mathf.Lerp(ArmRig.weight, rigWeight, Time.deltaTime * 2);
    }
}
