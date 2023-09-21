using System;
using Mirror;
using UnityEngine;
using Yarn.Unity;

namespace Environment
{
    public class Hideable : NetworkBehaviour
    {
        private Collider col;

        private void Start()
        {
            col = GetComponent<Collider>();
        }

        [YarnCommand("hide")]
        public void HideObject()
        {
            if (col != null)
            {
                col.enabled = false;
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        
        [YarnCommand("show")]
        public void ShowObject()
        {
            if (col != null)
            {
                col.enabled = true;
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}