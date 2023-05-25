using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Inventory;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUi : MonoBehaviour
{
    [Header("Menu")] 
    public Transform playUi;
    private PlayerController _playerController;
    public List<Transform> menuList;
    private int _menuIndex = 0;
    private bool _menuOpen = false;

    [Header("Debug")] 
    public TextMeshProUGUI inputDevice;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
    }

    private void Update()
    {
        inputDevice.text = transform.parent.GetComponent<PlayerInput>().currentControlScheme;
    }

    public void SwitchTo(int index)
    {
        HideAll();
        menuList[index].gameObject.SetActive(true);
    }

    public void HideAll()
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            menuList[i].gameObject.SetActive(false);
        }
    }

    public void OpenBookButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _menuIndex = 0;
            Open(_menuIndex);
        }
    }

    public void OpenMapButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _menuIndex = 1;
            Open(_menuIndex);
        }
    }

    public void OpenInventoryButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _menuIndex = 2;
            Open(_menuIndex);
            transform.parent.GetComponent<PlayerInventory>().ClearSelectedItemWindow();
        }
    }

    public void Open(int index)
    {
        _menuOpen = true;
        menuList[index].gameObject.SetActive(true);
        playUi.gameObject.SetActive(false);
        _playerController.ToggleCursor(true);
    }

    public void MenuSwitchRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_menuIndex < menuList.Count - 1)
            {
                _menuIndex++;
                SwitchTo(_menuIndex);
            }
        }
    }

    public void MenuSwitchLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_menuIndex > 0)
            {
                _menuIndex--;
                SwitchTo(_menuIndex);
            }
        }
    }

    public void CloseUI(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HideUIElements();
        }
    }

    void HideUIElements()
    {
        _menuOpen = false;
        playUi.gameObject.SetActive(true);
        HideAll();
        _playerController.ToggleCursor(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && _menuIndex == 2)
        {
            transform.parent.GetComponent<PlayerInventory>().OnDropItemButton();
        }
    }
}
