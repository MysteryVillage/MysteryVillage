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

        [Header("Animations")] 
        public Avatar boyAvatar;
        public Avatar girlAvatar;
        public RuntimeAnimatorController boyAnimatorController;
        public RuntimeAnimatorController girlAnimatorController;
        
        private void Start()
        {
            if (character == "Boy")
            {
                girl.SetActive(false);
                animator.avatar = boyAvatar;
                animator.runtimeAnimatorController = boyAnimatorController;
            }
            else
            {
                boy.SetActive(false);
                animator.avatar = girlAvatar;
                animator.runtimeAnimatorController = girlAnimatorController;
            }
        }
    }
}