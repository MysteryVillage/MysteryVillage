using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineManager : MonoBehaviour
{
    [SerializeField] List<Transform> _tubes;
    [SerializeField] List<Renderer> _lamps;

    private float[] rotation;
    private Color red = new Color32(197, 73, 73, 255);
    private Color white = new Color32(227, 227, 227, 255);

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
            rotation[i] = (_tubes[i].eulerAngles.y);
            //Debug.Log("tube" + i + ":" + rotation[i]);
        }

        if (rotation[1] > 269 || rotation[1] < 10)
        {
            _lamps[0].material.color = red;
            //Debug.Log("tube" + 1 + ":" + rotation[1]);

            if (rotation[0] > 89 && rotation[0] < 181)
            {
                _lamps[0].material.color = red;
            }
            else
            {
                _lamps[0].material.color = white;
            }
        }
        else
        {
            _lamps[0].material.color = white;
        }

    }
}
