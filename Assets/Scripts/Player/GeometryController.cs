using System;
using Mirror;
using UnityEngine;

namespace Player
{
    public class GeometryController : NetworkBehaviour
    {
        [SyncVar]
        public string character;
        public GameObject boy;
        public GameObject girl;
        public Animator animator;
        public bool isInitiated = false;

        [Header("Animations")] 
        public Avatar boyAvatar;
        public Avatar girlAvatar;
        public RuntimeAnimatorController boyAnimatorController;
        public RuntimeAnimatorController girlAnimatorController;
        
        private void Update()
        {
            // Only run if geometry is not initiated yet or the character somehow magically gets changed
            if (((GetComponent<PlayerController>().isBoy && character == "Boy") ||
                 (!GetComponent<PlayerController>().isBoy && character == "Girl")) && isInitiated) return;
            SetGeometry();
        }

        public void SetGeometry()
        {
            if (GetComponent<PlayerController>().isBoy)
            {
                character = "Boy";
                girl.SetActive(false);
                animator.avatar = boyAvatar;
                animator.runtimeAnimatorController = boyAnimatorController;
            }
            else
            {
                character = "Girl";
                boy.SetActive(false);
                animator.avatar = girlAvatar;
                animator.runtimeAnimatorController = girlAnimatorController;
            }

            if (!isInitiated) isInitiated = true;
        }
    }
}