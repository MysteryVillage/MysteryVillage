using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUi : MonoBehaviour
{
    [Header("Menue")] 
    private Transform playUi;
    private Transform book;
    private bool menueUi;

    // Start is called before the first frame update
    void Start()
    {
        playUi = transform.GetChild(0);
        book = transform.GetChild(1);
        menueUi = false;
    }

    public void SwitchUI (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            playUi.gameObject.SetActive(menueUi ? true : false);
            book.gameObject.SetActive(menueUi ? false : true);
            menueUi = menueUi ? false : true;
            menueUi = menueUi ? true : false;
        }
    }

}
