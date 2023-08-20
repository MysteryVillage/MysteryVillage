using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCLookAt : MonoBehaviour
{
    public Transform Target;
    public float Radius = 10f;

    List<PointOfInterest> POIs;
    // Start is called before the first frame update
    void Start()
    {
        //POIs = FindObjectsOfType<PointOfInterest>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        POIs = FindObjectsOfType<PointOfInterest>().ToList();
        Transform tracking = null;
        foreach (PointOfInterest poi in POIs)
        {
            Vector3 delta = poi.transform.position - transform.position;
            if(delta.magnitude < Radius)
            {
                tracking = poi.transform;
                break;
            }
        }
        if (tracking != null)
        {
            Target.position = tracking.position;
        }
    }
}
