using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUi : MonoBehaviour
{
    [Header("Menu")] 
    public Transform playUi;
    public Transform book;
    public Transform map;
    private bool menueUi;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        menueUi = false;
        _playerController = transform.parent.GetComponent<PlayerController>();
    }

    public void SwitchToBook (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            menueUi = !menueUi;
            playUi.gameObject.SetActive(menueUi);
            book.gameObject.SetActive(!menueUi);
            map.gameObject.SetActive(false);
            _playerController.ToggleCursor(!menueUi);
        }
    }

    public void SwitchToMap (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            menueUi = !menueUi;
            playUi.gameObject.SetActive(menueUi);
            map.gameObject.SetActive(!menueUi);
            book.gameObject.SetActive(false);
            _playerController.ToggleCursor(!menueUi);
        }
    }

    public void CloseUI(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            menueUi = false;
            playUi.gameObject.SetActive(!menueUi);
            map.gameObject.SetActive(menueUi);
            book.gameObject.SetActive(menueUi);
            _playerController.ToggleCursor(menueUi);
            transform.parent.GetComponent<PlayerInventory>().Toggle();
        }
    }

}
