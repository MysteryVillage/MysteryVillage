using Mirror;
using NPC;
using UnityEngine;

namespace Dungeon.Labyrinth
{
    public class Labyrinth : PuzzleBehaviour, IPuzzle
    {
        public new void Solve()
        {
            // Update NPC dialogue states
            GameObject.Find("Arnold").GetComponent<NPCObject>().SetDialogue(2);
            
            // Trigger local notifications
            base.Solve();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}