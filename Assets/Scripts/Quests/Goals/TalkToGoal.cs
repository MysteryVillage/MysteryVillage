using Mirror;
using NPC;
using UnityEngine;

namespace Quests.Goals
{
    public class TalkToGoal : QuestGoal
    {
        public GameObject npc;
        public NPCObject target;
        public NpcName npcName;

        public string dialogueScript = "";
        
        public new void Init()
        {
            Debug.Log(npcName, this);
            base.Init();
            
            var NPCs = FindObjectsOfType<NPCObject>();
            foreach (var npcObject in NPCs)
            {
                Debug.Log(npcObject.npcName, npcObject);
                if (npcName != npcObject.npcName) continue;
                target = npcObject;
                if (dialogueScript != "")
                {
                    target.nextDialogueOverwrite = dialogueScript;
                }
                target.OnTalk.AddListener(Talk);
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

            NetworkIdentity networkIdentity = talkToGoal.target == null ? null : talkToGoal.target.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);
            writer.WriteInt((int) talkToGoal.npcName);
            writer.WriteString(talkToGoal.dialogueScript);
        }

        public static TalkToGoal ReadTalkToGoal(NetworkReader reader)
        {
            var goal = CreateInstance<TalkToGoal>();

            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            NPCObject target = networkIdentity != null
                ? networkIdentity.GetComponent<NPCObject>()
                : null;

            goal.target = target;
            goal.npcName = (NpcName) reader.ReadInt();
            goal.dialogueScript = reader.ReadString();

            return goal;
        }
    }
}