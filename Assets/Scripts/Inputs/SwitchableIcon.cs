using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inputs
{
    public class SwitchableIcon : MonoBehaviour
    {
        public InputActionReference action;
        
        private InputIconMap _iconMap;

        public Sprite gamepadNavigation;
        public Sprite keyboardNavigation;
        private Image _navigation;
        private SpriteRenderer _spriteRenderer;


        private void Start()
        {
            _iconMap = GetComponentInParent<PlayerUi>().iconMap;
            _navigation = GameObject.Find("DPad").GetComponent<Image>();
            //_spriteRenderer = GameObject.Find("DPad").GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            for (int i = 0; i < _iconMap.map.Length; i++)
            {
                if (action.action.Equals(_iconMap.map[i].action.action))
                {
                    if (GetComponentInParent<PlayerInput>().currentControlScheme == "Gamepad")
                    {
                        GetComponent<Image>().sprite = _iconMap.map[i].gamepadIcon;
                        _navigation.sprite = gamepadNavigation;
                    }
                    else
                    {
                        GetComponent<Image>().sprite = _iconMap.map[i].keyboardIcon;
                        _navigation.sprite = keyboardNavigation;
                    }
                }
            }
        }
    }
}
