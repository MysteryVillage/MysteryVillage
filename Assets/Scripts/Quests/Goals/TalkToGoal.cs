using NPC;
using UnityEngine;

namespace Quests.Goals
{
    public class TalkToGoal : QuestGoal
    {
        public GameObject npc;
        public NPCObject target;
        
        public new void Init()
        {
            base.Init();

            var NPCs = GameObject.FindObjectsOfType<NPCObject>();
            foreach (var npc in NPCs)
            {
                Debug.Log(npc.name);
                Debug.Log(this.npc.name);
                if (npc.name == this.npc.name)
                {
                    npc.OnTalk.AddListener(Talk);
                    target = npc;
                }
            }
        }

        public void Talk()
        {
            target.OnTalk.RemoveListener(Talk);
            Debug.Log("Talked to target");
            CurrentAmount++;
            Evaluate();
        }
        
        public TalkToGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }
    }
}