using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inputs
{
    public class SwitchableNavigationIcon : MonoBehaviour
    {
        public Sprite gamepadIcon;
        public Sprite keyboardIcon;

        private void Update()
        {
            UpdateIcon();
        }

        protected void UpdateIcon()
        {
            if (GetComponentInParent<PlayerInput>().currentControlScheme == "Gamepad")
            {
                GetComponent<Image>().sprite = gamepadIcon;
            }
            else
            {
                GetComponent<Image>().sprite = keyboardIcon;
            }
        }
    }
}