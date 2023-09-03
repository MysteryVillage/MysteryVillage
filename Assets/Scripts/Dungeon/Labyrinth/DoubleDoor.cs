using Mirror;
using UnityEngine;

namespace Dungeon.Labyrinth
{
    public class DoubleDoor : NetworkBehaviour
    {
        public GameObject leftDoor;
        public GameObject rightDoor;
        private Animator leftAnimator;
        private Animator rightAnimator;

        private void Start()
        {
            leftAnimator = leftDoor.GetComponent<Animator>();
            rightAnimator = rightDoor.GetComponent<Animator>();
        }

        [ClientRpc]
        public void OpenDoor()
        {
            leftAnimator.Play("DoorLeft_Open", 0, 0.0f);
            rightAnimator.Play("DoorRight_Open", 0, 0.0f);
        }
    }
}