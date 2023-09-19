using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class checkLightsLT : MonoBehaviour
{
    [SerializeField] List<Renderer> _lamp;
    [SerializeField] Material _red;
    [SerializeField] Material _green;

    // Update is called once per frame
    void Update()
    {
        if (_lamp[0].material && _lamp[1].material && _lamp[2].material == _green)
        {
            EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
            if (eventSystem)
            {
                eventSystem.onQuestEvent.Invoke("LampsAreGreen");
            }
        }
    }
}
