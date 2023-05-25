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
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUi : MonoBehaviour
{
    [Header("Menu")] 
    public Transform playUi;
    public Transform book;
    public Transform map;
    private bool menuUi;
    private bool _menu;
    private PlayerController _playerController;
    private List<Transform> _menuList;
    int _index = 0;

    [Header("Debug")] 
    public TextMeshProUGUI inputDevice;

    // Start is called before the first frame update
    void Start()
    {
        menuUi = false;
        _menu = false;
        _playerController = transform.parent.GetComponent<PlayerController>();
        /*for (int i = 0; i <= 1; i++)
        {
            _menuList[i] = transform.GetChild(i+2);
        }*/
    }

    private void Update()
    {
        inputDevice.text = transform.parent.GetComponent<PlayerInput>().currentControlScheme;
    }

    public void SwitchToBook (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            menuUi = !menuUi;
            playUi.gameObject.SetActive(menuUi);
            book.gameObject.SetActive(!menuUi);
            map.gameObject.SetActive(false);
            _playerController.ToggleCursor(!menuUi);
            _menu = true;
        }
    }

    public void SwitchToMap(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            menuUi = !menuUi;
            playUi.gameObject.SetActive(menuUi);
            map.gameObject.SetActive(!menuUi);
            book.gameObject.SetActive(false);
            _playerController.ToggleCursor(!menuUi);
            _menu = true;
        }
    }

    public void MenuSwitchRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log(_menu);
            if (_index < 2 - 1 && _menu)
            {
                _index++;
                /*_menuList[_index - 1].gameObject.SetActive(false);
                _menuList[_index].gameObject.SetActive(true);*/
            }
        }
    }

    public void MenuSwitchLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log(_menu);
            if (_index > -1 && _menu)
            {
                _index--;
                /*_menuList[_index + 1].gameObject.SetActive(false);
                _menuList[_index].gameObject.SetActive(true);*/
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
        menuUi = false;
        _menu = false;
        playUi.gameObject.SetActive(!menuUi);
        map.gameObject.SetActive(menuUi);
        book.gameObject.SetActive(menuUi);
        _playerController.ToggleCursor(menuUi);
        transform.parent.GetComponent<PlayerInventory>().Close();
    }

}
