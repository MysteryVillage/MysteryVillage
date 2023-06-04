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

        private void Start()
        {
            _iconMap = GetComponentInParent<PlayerUi>().iconMap;
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
                    }
                    else
                    {
                        GetComponent<Image>().sprite = _iconMap.map[i].keyboardIcon;
                    }
                }
            }
        }
    }
}
