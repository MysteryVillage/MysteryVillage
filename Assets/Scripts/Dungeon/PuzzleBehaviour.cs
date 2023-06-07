using Mirror;
using UnityEngine;

namespace Dungeon
{
    public class PuzzleBehaviour : NetworkBehaviour
    {
        public AudioClip solveSound;
        public string puzzleName = "Puzzle";

        public void Solve()
        {
            NotifySolveRpc();
        }

        [ClientRpc]
        public void NotifySolveRpc()
        {
            NetworkClient.localPlayer.GetComponent<AudioSource>().PlayOneShot(solveSound);
        }
    }
}