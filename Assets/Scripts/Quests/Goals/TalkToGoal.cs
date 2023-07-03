using Items;
using Mirror;
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
                if (npc.name == this.npc.name)
                {
                    target = npc;
                    target.OnTalk.AddListener(Talk);
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

        public static void WriteTalkToGoal(NetworkWriter writer, QuestGoal goal)
        {
            TalkToGoal talkToGoal = goal as TalkToGoal;
            if (talkToGoal == null) return;
            
            NetworkIdentity networkIdentity = talkToGoal.target.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);
        }

        public static TalkToGoal ReadTalkToGoal(NetworkReader reader)
        {
            var goal = CreateInstance<TalkToGoal>();

            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            NPCObject target = networkIdentity != null
                ? networkIdentity.GetComponent<NPCObject>()
                : null;

            goal.target = target;

            return goal;
        }
    }
}