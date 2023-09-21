using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Network
{
    public class GameSettings : MonoBehaviour
    {
        public bool isLocalGame;
        public bool isTestRun;
        /**
         * 0 = Intro
         * 1 = Labyrinth
         * 2 = Leuchtturm
         */
        public int startQuest = 0;

        private static GameSettings _instance;

        private void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            if (Keyboard.current.f3Key.wasPressedThisFrame)
            {
                Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
            }
        }

        public static GameSettings Get()
        {
            return _instance;
        }
    }
}