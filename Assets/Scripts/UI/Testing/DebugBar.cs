using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using NetworkManager = Network.NetworkManager;

namespace UI
{
    public class DebugBar : MonoBehaviour
    {
        public GameObject player;

        [Header("FPS")] public TextMeshProUGUI fpsText;
        private int _fps;
        private float _fpsTimeCounter;
        private float _lastFps;
        public float fpsRefreshTime;

        [Header("Input device")] public TextMeshProUGUI inputDevice;

        [Header("Connection")] public TextMeshProUGUI connectionType;
        public TextMeshProUGUI connectionIp;
        public TextMeshProUGUI connectionTime;
        public DateTime connectionTimestamp;

        private void Start()
        {
            connectionTimestamp = DateTime.Now;
            if (!Debug.isDebugBuild)
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            // FPS calculation
            if (_fpsTimeCounter < fpsRefreshTime)
            {
                _fpsTimeCounter += Time.deltaTime;
                _fps++;
            }
            else
            {
                //This code will break if you set your m_refreshTime to 0, which makes no sense.
                _lastFps = _fps / _fpsTimeCounter;
                _fps = 0;
                _fpsTimeCounter = 0.0f;
            }

            fpsText.text = "FPS: " + _lastFps;

            // Input device
            inputDevice.text = "Input Device: " + player.GetComponent<PlayerInput>().currentControlScheme;

            // Connection
            string connectionTypeData;
            string connectionIpData;
            if (NetworkServer.active)
            {
                connectionTypeData = "Host";
                connectionIpData = "Host";
            }
            else
            {
                connectionTypeData = "Client";
                connectionIpData = Mirror.NetworkManager.singleton.networkAddress;
            }

            connectionType.text = "Connectiontype: " + connectionTypeData;
            connectionIp.text = "Connection IP: " + connectionIpData;
            var connectionTimeSpan = DateTime.Now - connectionTimestamp;
            connectionTime.text = "Connection Time: " + FormatTimespan(connectionTimeSpan);
        }

        string FormatTimespan(TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
