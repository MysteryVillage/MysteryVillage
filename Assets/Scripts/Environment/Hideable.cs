using Mirror;
using UnityEngine;
using Yarn.Unity;

namespace Environment
{
    public class Hideable : NetworkBehaviour
    {
        [YarnCommand("hide")]
        public void HideObject()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        
        [YarnCommand("show")]
        public void ShowObject()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}