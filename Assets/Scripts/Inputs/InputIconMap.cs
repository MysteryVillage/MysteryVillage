using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    [CreateAssetMenu(fileName = "Input Icon Map", menuName = "New Input Icon Map")]
    public class InputIconMap : ScriptableObject
    {
        public InputIconReference[] map;
    }

    [Serializable]
    public class InputIconReference
    {
        public Sprite gamepadIcon;
        public Sprite keyboardIcon;
        public InputActionReference action;
    }
}