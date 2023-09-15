using Mirror;
using System;
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
    private bool [] doorState = new bool[6] ;
    private bool[] lightState = new bool[10];
    private bool test1, test2, test3 = false;
    void Start()
    {
        rotation = new float[_tubes.Count];
        for(int i=0; i< doorState.Length; i++)
        {
            doorState[i] = false;
        }
        for (int i = 0; i < lightState.Length; i++)
        {
            lightState[i] = false;
        }
    }

    void Update()
    {
        /* ------------------------ Tür Mangament ------------------------ */
        /* Tür 1 */

        Debug.Log("Mond: "+  test1  + "Punkt: " + test2 + "Viereck: " + test3);
        if (lightState[6] == true) test1 = true;
        if (lightState[1] == true) test2 = true;
        if (lightState[5] == true) test3 = true;

        if (lightState[6] == true &&
            lightState[1] == true &&
            lightState[5] == true &&
            doorState[0]  == false)
        {
            Debug.Log("Tür 1 Auf ..................................................");
            AnimateDoor(1, true); // Tür 1 öffnen 
            doorState[0] = true;
        }
        /* Tür zu wenn sie offen ist und eine der Lampen ausgeht */
        if (lightState[6] == false ||
            lightState[1] == false ||
            lightState[5] == false)
        {
            if (doorState[0] == true)
            {
               
                AnimateDoor(1, false); // Tür 1 öffnen 
                doorState[0] = false;
            }
        }


        /* Tür 2 */
        if (lightState[9] == true && 
            lightState[3] == true &&
            lightState[0] == true &&
            doorState[1]  == false)
        {
            AnimateDoor(2, true); // Tür 2 öffnen 
            doorState[1] = true;
        }
        /* Tür zu wenn sie offen ist und eine der Lampen ausgeht */
        if (lightState[9] == false || 
            lightState[3] == false ||
            lightState[0] == false)
        {
            if (doorState[1] == true)
            {
                AnimateDoor(2, false); // Tür 1 öffnen 
                doorState[1] = false;
            }
        }


        /* Tür 3 */
        if (lightState[9] == true && 
            lightState[8] == true &&
            lightState[2] == true &&
             doorState[2] == false)
        {
            AnimateDoor(3, true); // Tür 3 öffnen 
            doorState[2] = true;
        }  
        /* Tür zu wenn sie offen ist und eine der Lampen ausgeht */
        if (lightState[9] == false || 
            lightState[8] == false ||
            lightState[2] == false)
        {
            if (doorState[2] == true)
            {
                AnimateDoor(3, false); // Tür 1 öffnen 
                doorState[2] = false;
            }
        }


        /* Tür 4 */
        if (lightState[8] == true && 
            lightState[0] == true &&
            lightState[1] == true &&
             doorState[3] == false)
        {
            AnimateDoor(4, true); // Tür 4 öffnen 
            doorState[3] = true;
        }
        /* Tür zu wenn sie offen ist und eine der Lampen ausgeht */
        if (lightState[8] == false ||
            lightState[0] == false ||
            lightState[1] == false)
        {
            if (doorState[3] == true)
            {
                AnimateDoor(4, false); // Tür 1 öffnen 
                doorState[3] = false;
            }
        }


        /* Tür 5 */
        if (lightState[1] == true && 
            lightState[4] == true &&
            lightState[7] == true &&
             doorState[4] == false)
        {
            AnimateDoor(5, true); // Tür 5 öffnen 
            doorState[4] = true;
        }
        /* Tür zu wenn sie offen ist und eine der Lampen ausgeht */
        if (lightState[1] == false || 
            lightState[4] == false ||
            lightState[7] == false)
        {
            if (doorState[4] == true)
            {
                AnimateDoor(5, false); // Tür 1 öffnen 
                doorState[4] = false;
            }
        }


        /* Tür 6 */
        if (lightState[6] == true && 
            lightState[9] == true &&
            lightState[4] == true &&
             doorState[5] == false)
        {
            AnimateDoor(6, true); // Tür 6 öffnen 
            doorState[5] = true;
        }
        /* Tür zu wenn sie offen ist und eine der Lampen ausgeht */
        if (lightState[6] == false || 
            lightState[9] == false ||
            lightState[4] == false)
        {
            if (doorState[5] == true)
            {
                AnimateDoor(6, false); // Tür 1 öffnen 
                doorState[5] = false;
            }
        }






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
            lightState[0] = false;
        }
        if (rotation[0] > 80 && rotation[0] < 100)
        {
            _lamps[0].material = _red;
            _lamps[6].material = _red;
            _lamps[7].material = _red;
            _lamps[8].material = _red;
            lightState[0] = false;
            lightState[6] = false;
            lightState[7] = false;
            lightState[8] = false;
        }
        if (rotation[0] > 170 && rotation[0] < 190)
        {
            CheckLeftMiddlePath();
            _lamps[0].material = _green;
            lightState[0] = true;
        }
        if (rotation[0] > 260 && rotation[0] < 280)
        {
            CheckLeftLeftPath();
            _lamps[0].material = _green;
            lightState[0] = true;

        }

        //---- mitte ----
        if (rotation[4] < 10)
        {
            _lamps[1].material = _green;
            _lamps[9].material = _red;
            _lamps[2].material = _red;
            _lamps[3].material = _red;
            lightState[1] = true;
            lightState[9] = false;
            lightState[2] = false;
            lightState[3] = false;
        }

        if (rotation[4] > 80 && rotation[4] < 100)
        {
            CheckMiddleMiddlePath();
            _lamps[1].material = _red;
            _lamps[9].material = _green;
            lightState[1] = false;
            lightState[9] = true;
        }
        if (rotation[4] > 170 && rotation[4] < 190)
        {
            _lamps[1].material = _red;
            _lamps[9].material = _red;
            _lamps[2].material = _red;
            _lamps[3].material = _red;
            lightState[1] = false;
            lightState[9] = false;
            lightState[2] = false;
            lightState[3] = false;
        }
        if (rotation[4] > 260 && rotation[4] < 280)
        {
            CheckMiddleMiddlePath();
            _lamps[1].material = _green;
            _lamps[9].material = _green;
            lightState[1] = true;
            lightState[9] = true;
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
            lightState[4] = false;
            lightState[5] = false;
        }
        if (rotation[9] > 170 && rotation[9] < 190)
        {
            CheckRightUpPath();
            if (rotation[7] > 170 && rotation[7] <280)
            {
                _lamps[4].material = _green;
                lightState[4] = true;
            }
            else
            {
                _lamps[4].material = _red;
                lightState[4] = false;
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
            lightState[8] = true;
        }
        else
        {
            _lamps[8].material = _red;
            lightState[8] = false;
        }
    }
    private void CheckLeftMiddlePath()
    {
        if (rotation[3] < 10 || (rotation[3] > 170 && rotation[3] < 190))
        {
            if (rotation[2] > 260)
            {
                _lamps[7].material = _red;
                lightState[7] = false;
            }
            else
            {
                _lamps[7].material = _green;
                lightState[7] = true;
            }
        }
        else
        {
            _lamps[7].material = _red;
            lightState[7] = false;
        }

        if (rotation[3] > 80 && rotation[3] < 190)
        {
            _lamps[6].material = _green;
            lightState[6] = true;
        }
        else
        {
            _lamps[6].material = _red;
            lightState[6] = false;
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
                lightState[2] = true;
                lightState[3] = false;
            }
            if (rotation[6] > 80 && rotation[6] < 100)
            {
                _lamps[2].material = _red;
                _lamps[3].material = _green;
                lightState[2] = false;
                lightState[3] = true;
            }
            if (rotation[6] > 170 && rotation[6] < 190)
            {
                _lamps[2].material = _red;
                _lamps[3].material = _red;
                lightState[2] = false;
                lightState[3] = false;
            }
            if (rotation[6] > 260 && rotation[6] < 280)
            {
                _lamps[2].material = _green;
                _lamps[3].material = _green;
                lightState[2] = true;
                lightState[3] = true;
            }
        }
        else
        {
            _lamps[2].material = _red;
            _lamps[3].material = _red;
            lightState[2] = false;
            lightState[3] = false;
        }
    }
    private void CheckRightUpPath() 
    {
        if (rotation[8] < 10)
        {
            _lamps[5].material = _green;
            lightState[5] = true;
        }
        else
        {
            _lamps[5].material = _red;
            lightState[5] = false;
        }
    }
    private void CheckRightDownPath()
    {
        if (rotation[7] < 10)
        {
            _lamps[4].material = _red;
            _lamps[5].material = _green;
            lightState[4] = false;
            lightState[5] = true;
        }
        if (rotation[7] > 80 && rotation[7] < 100)
        {
            _lamps[4].material = _green;
            _lamps[5].material = _red;
            lightState[4] = true;
            lightState[5] = false;
        }
        if (rotation[7] > 170 && rotation[7] < 190)
        {
            _lamps[4].material = _red;
            _lamps[5].material = _red;
            lightState[4] = false;
            lightState[5] = false;
        }
        if (rotation[7] > 260 && rotation[7] < 280)
        {
            _lamps[4].material = _green;
            _lamps[5].material = _green;
            lightState[4] = true;
            lightState[5] = true;
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
                    if (_doors[0] != null ) _doors[0].Play("LT_Door_Animation_Open", 0, 0.0f); Debug.Log(" Tür 1 Öffnet sich");
                    break;
                case 2:
                    if (_doors[1] != null) _doors[1].Play("LT_Door_Animation_Open", 0, 0.0f); Debug.Log(" Tür 2 Öffnet sich");
                    break;
                case 3:
                    if (_doors[2] != null) _doors[2].Play("LT_Door_Animation_Open", 0, 0.0f); Debug.Log(" Tür 3 Öffnet sich");
                    break;
                case 4:
                    if (_doors[3] != null) _doors[3].Play("LT_Door_Animation_Open", 0, 0.0f); Debug.Log(" Tür 4 Öffnet sich");
                    break;
                case 5:
                    if (_doors[4] != null) _doors[4].Play("LT_Door_Animation_Open", 0, 0.0f); Debug.Log(" Tür 5 Öffnet sich");
                    break;
                case 6:
                    if (_doors[5] != null) _doors[5].Play("LT_Door_Animation_Open", 0, 0.0f); Debug.Log(" Tür 6 Öffnet sich");
                    break;
            }
        }
        else
        {
            
            switch (door)
            {
                case 1:
                    if (_doors[0] != null) _doors[0].Play("LT_Door_Animation_Close", 0, 0.0f); Debug.Log(" Tür 1 schließt sich");
                    break;
                case 2:
                    if (_doors[1] != null) _doors[1].Play("LT_Door_Animation_Close", 0, 0.0f); Debug.Log(" Tür 2 schließt sich");
                    break;
                case 3:
                    if (_doors[2] != null) _doors[2].Play("LT_Door_Animation_Close", 0, 0.0f); Debug.Log(" Tür 3 schließt sich");
                    break;
                case 4:
                    if (_doors[3] != null) _doors[3].Play("LT_Door_Animation_Close", 0, 0.0f); Debug.Log(" Tür 4 schließt sich");
                    break;
                case 5:
                    if (_doors[4] != null) _doors[4].Play("LT_Door_Animation_Close", 0, 0.0f); Debug.Log(" Tür 5 schließt sich");
                    break;
                case 6:
                    if (_doors[5] != null) _doors[5].Play("LT_Door_Animation_Close", 0, 0.0f); Debug.Log(" Tür 6 schließt sich");
                    break;
            }
        }   
    }

}
