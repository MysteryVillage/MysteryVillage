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

    void Start()
    {
        rotation = new float[_tubes.Count];
    }

    void Update()
    {
        for (int i = 0; i < _tubes.Count; i++)
        {
            rotation[i] = (_tubes[i].eulerAngles.z);
            //Debug.Log("tube" + i + ":" + rotation[i]);
        }

        //---- linke seite ----
        if (rotation[0] < 10)
        {
            CheckLeftLeftPath();
            CheckLeftMiddlePath();
            _lamps[0].material = _red;
        }
        if (rotation[0] > 80 && rotation[0] < 100)
        {
            _lamps[0].material = _red;
            _lamps[6].material = _red;
            _lamps[7].material = _red;
            _lamps[8].material = _red;
        }
        if (rotation[0] > 170 && rotation[0] < 190)
        {
            CheckLeftMiddlePath();
            _lamps[0].material = _green;
        }
        if (rotation[0] > 260 && rotation[0] < 280)
        {
            CheckLeftLeftPath();
            _lamps[0].material = _green;
        }

        //---- mitte ----
        if (rotation[4] < 10)
        {
            _lamps[1].material = _green;
            _lamps[9].material = _red;
            _lamps[2].material = _red;
            _lamps[3].material = _red;
        }

        if (rotation[4] > 80 && rotation[4] < 100)
        {
            CheckMiddleMiddlePath();
            _lamps[1].material = _red;
            _lamps[9].material = _green;
        }
        if (rotation[4] > 170 && rotation[4] < 190)
        {
            _lamps[1].material = _red;
            _lamps[9].material = _red;
            _lamps[2].material = _red;
            _lamps[3].material = _red;
        }
        if (rotation[4] > 260 && rotation[4] < 280)
        {
            CheckMiddleMiddlePath();
            _lamps[1].material = _green;
            _lamps[9].material = _green;
        }

        //---- rechte Seite ----
        if (rotation[9] < 10)
        {
            CheckRightDownPath();
        }

        if (rotation[9] > 80 && rotation[9] < 100)
        {
            _lamps[4].material = _red;
            _lamps[5].material = _red;
        }
        if (rotation[9] > 170 && rotation[9] < 190)
        {
            CheckRightUpPath();
            if (rotation[7] > 170 && rotation[7] <280)
            {
                _lamps[4].material = _green;
            }
            else
            {
                _lamps[4].material = _red;
            }
        }
        if (rotation[9] > 260 && rotation[9] < 280)
        {
            CheckRightUpPath();
            CheckRightDownPath();
        }
    }


    private void CheckLeftLeftPath()
    {
        if (rotation[1] < 10)
        {
            _lamps[8].material = _green;
        }
        else
        {
            _lamps[8].material = _red;
        }
    }
    private void CheckLeftMiddlePath()
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
    private void CheckMiddleMiddlePath()
    {
        if (rotation[5] < 10)
        {
            if (rotation[6] < 10)
            {
                _lamps[2].material = _green;
                _lamps[3].material = _red;
            }
            if (rotation[6] > 80 && rotation[6] < 100)
            {
                _lamps[2].material = _red;
                _lamps[3].material = _green;
            }
            if (rotation[6] > 170 && rotation[6] < 190)
            {
                _lamps[2].material = _red;
                _lamps[3].material = _red;
            }
            if (rotation[6] > 260 && rotation[6] < 280)
            {
                _lamps[2].material = _green;
                _lamps[3].material = _green;
            }
        }
        else
        {
            _lamps[2].material = _red;
            _lamps[3].material = _red;
        }
    }
    private void CheckRightUpPath() 
    {
        if (rotation[8] < 10)
        {
            _lamps[5].material = _green;
        }
        else
        {
            _lamps[5].material = _red;
        }
    }
    private void CheckRightDownPath()
    {
        if (rotation[7] < 10)
        {
            _lamps[4].material = _red;
            _lamps[5].material = _green;
        }
        if (rotation[7] > 80 && rotation[7] < 100)
        {
            _lamps[4].material = _green;
            _lamps[5].material = _red;
        }
        if (rotation[7] > 170 && rotation[7] < 190)
        {
            _lamps[4].material = _red;
            _lamps[5].material = _red;
        }
        if (rotation[7] > 260 && rotation[7] < 280)
        {
            _lamps[4].material = _green;
            _lamps[5].material = _green;
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
