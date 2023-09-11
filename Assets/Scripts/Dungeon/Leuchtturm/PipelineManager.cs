using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineManager : MonoBehaviour
{
    [SerializeField] List<Transform> _tubes;
    [SerializeField] List<Renderer> _lamps;
    [SerializeField] Material _red;
    [SerializeField] Material _green;

    private float[] rotation;

    private bool D;
    private bool U;
    private bool Moon;
    private bool Drop;
    private bool Dot;
    private bool S;
    private bool horizontal ;
    private bool vertical;
    private bool Square;
    private bool circle;


    // Start is called before the first frame update
    void Start()
    {
        rotation = new float[_tubes.Count];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<_tubes.Count; i++)
        {
            rotation[i] = (_tubes[i].eulerAngles.z);
            //Debug.Log("tube" + i + ":" + rotation[i]);
        }

        if ((rotation[1] > 269 || rotation[1] < 10) || (rotation[0] > 89 && rotation[0] < 181))
        {
            _lamps[0].material = _red;
        }
        else
        {
            _lamps[0].material = _green;
        }

        /*if (rotation[0] > 89 && rotation[0] < 181)
        {
            _lamps[0].material.color = red;
        }
        else
        {
            _lamps[0].material.color = white;
        }*/

    }
}
