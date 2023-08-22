using System;
using Mirror;
using UnityEngine;

namespace Network
{
    public class GameSettings : MonoBehaviour
    {
        public bool isLocalGame;

        private static GameSettings _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static GameSettings Get()
        {
            return _instance;
        }
    }
}