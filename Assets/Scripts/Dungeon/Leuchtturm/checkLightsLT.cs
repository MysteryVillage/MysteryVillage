using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class checkLightsLT : MonoBehaviour
{
    public List<SwitchesLeuchtturm> switches;
    private EventSystem eventSystem;

    // Update is called once per frame
    void Update()
    {
        foreach (var ltSwitch in switches)
        {
            if (!ltSwitch.IsActivated()) return;
        }
        
        if (eventSystem == null) GetEventSystem();

        Debug.Log("Game completed");
        if (eventSystem != null) eventSystem.onQuestEvent.Invoke("LampsAreGreen");
    }

    public void GetEventSystem()
    {
        eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
    }
}
