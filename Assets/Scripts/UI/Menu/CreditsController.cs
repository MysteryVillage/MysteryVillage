using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CreditsController : MonoBehaviour
    {
        public int holdForSeconds = 20;
        private void Start()
        {
            StartCoroutine(GoToMainMenu());
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                EndSession();
            }
        }

        public IEnumerator GoToMainMenu()
        {
            yield return new WaitForSeconds(holdForSeconds);

            EndSession();
        }

        private void EndSession()
        {
            if (NetworkServer.active)
            {
                NetworkManager.singleton.StopHost();
            }
        }
    }
}