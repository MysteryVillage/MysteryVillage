using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineManager : NetworkBehaviour
{
    [SerializeField] List<Transform> _tubes;
    [SerializeField] List<Renderer> _lamps;
    [SerializeField] List<Animator> _doors;
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


    void Start()
    {
        rotation = new float[_tubes.Count];
    }

    void Update()
    {
        for (int i=0; i<_tubes.Count; i++)
        {
            rotation[i] = (_tubes[i].eulerAngles.z);
            //Debug.Log("tube" + i + ":" + rotation[i]);
        }

        //---- linke seite ----

        if (rotation[0] < 10)
        {
            CheckTheLeftLeftPath();
            CheckTheLeftmiddlePath();
            _lamps[0].material = _red;
        }
        if (rotation[0] > 80 && rotation[0] <100)
        {
            _lamps[0].material = _red;
            _lamps[6].material = _red;
            _lamps[7].material = _red;
            _lamps[8].material = _red;
        }
        if (rotation[0] > 100 && rotation[0] < 190)
        {
            CheckTheLeftmiddlePath();
            _lamps[0].material = _green;
        }
        if (rotation[0] > 190 && rotation[0] < 280)
        {
            CheckTheLeftLeftPath();
            _lamps[0].material = _green;
        }

        //---- mitte ----

    }

    private void CheckTheLeftLeftPath()
    {
        if (rotation[1] < 10)
        {
            _lamps[8].material = _green;
            // AnimateDoor(1,true); // Tür 1 wird geöffnet 
            // AnimateDoor(1,false); // Tür 1 wird geschlossen
        }
        else
        {
            _lamps[8].material = _red;
        }
    }

    private void CheckTheLeftmiddlePath()
    {
        if (rotation[3] < 10 || (rotation[3] > 170 && rotation[3] < 190))
        {
            if (rotation[2] > 260)
            {
                _lamps[7].material = _red;
            }
            else
            {
                _lamps[7].material = _green;
            }
        }
        else
        {
            _lamps[7].material = _red;
        }

        if (rotation[3] > 80 && rotation[3] < 190)
        {
            _lamps[6].material = _green;
        }
        else
        {
            _lamps[6].material = _red;
        }
    }

    [ClientRpc]
    private void AnimateDoor( int door , bool open)
    {
        if (open)
        {
            switch (door)
            {
                case 1:
                    if (_doors[0] != null) _doors[0].Play("LT_Tür_Animation_Open", 0, 0.0f);
                    break;
                case 2:
                    if (_doors[1] != null) _doors[1].Play("LT_Tür_Animation_Open", 0, 0.0f);
                    break;
                case 3:
                    if (_doors[2] != null) _doors[2].Play("LT_Tür_Animation_Open", 0, 0.0f);
                    break;
                case 4:
                    if (_doors[3] != null) _doors[3].Play("LT_Tür_Animation_Open", 0, 0.0f);
                    break;
                case 5:
                    if (_doors[4] != null) _doors[4].Play("LT_Tür_Animation_Open", 0, 0.0f);
                    break;
                case 6:
                    if (_doors[5] != null) _doors[5].Play("LT_Tür_Animation_Open", 0, 0.0f);
                    break;
            }
        }
        else
        {
            
            switch (door)
            {
                case 1:
                    if (_doors[0] != null) _doors[0].Play("LT_Tür_Animation_Close", 0, 0.0f);
                    break;
                case 2:
                    if (_doors[1] != null) _doors[1].Play("LT_Tür_Animation_Close", 0, 0.0f);
                    break;
                case 3:
                    if (_doors[2] != null) _doors[2].Play("LT_Tür_Animation_Close", 0, 0.0f);
                    break;
                case 4:
                    if (_doors[3] != null) _doors[3].Play("LT_Tür_Animation_Close", 0, 0.0f);
                    break;
                case 5:
                    if (_doors[4] != null) _doors[4].Play("LT_Tür_Animation_Close", 0, 0.0f);
                    break;
                case 6:
                    if (_doors[5] != null) _doors[5].Play("LT_Tür_Animation_Close", 0, 0.0f);
                    break;
            }
        }   
    }

}
