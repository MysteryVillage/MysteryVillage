using System;
using Inventory;
using Items;
using Mirror;
using NPC;
using Player;
using UnityEngine;

namespace Quests.Goals
{
    public class BringToGoal : QuestGoal
    {
        public RequestedItem[] RequestedItems;
        public NPCObject target;
        public NpcName npcName;

        public string successDialogue = "";
        public string failureDialogue = "";
        
        public BringToGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
            
        }
        
        public new void Init()
        {
            Debug.Log("Init BringToGoal");
            base.Init();
            
            var NPCs = FindObjectsOfType<NPCObject>();
            foreach (var npcObject in NPCs)
            {
                Debug.Log(npcObject.npcName, npcObject);
                if (npcName != npcObject.npcName) continue;
                target = npcObject;
                target.OnTalk.AddListener(Talk);
            }
        }

        public void Talk()
        {
            target.OnTalk.RemoveListener(Talk);
            Debug.Log("Talked to target");
            
            // evaluate inventory
            var players = PlayerController.GetPlayers();
            var collin = players[0].isBoy ? players[0] : players[1];

            var inventory = InventoryManager.Instance().GetInventoryForPlayer(collin.gameObject);

            var hasItems = true;
            foreach (var requestedItem in RequestedItems)
            {
                if (!inventory.HasItem(ItemData.FindById(requestedItem.itemId), requestedItem.amount))
                {
                    hasItems = false;
                    break;
                }
            }

            if (hasItems)
            {
                target.OnTalk.RemoveListener(Talk);
                foreach (var requestedItem in RequestedItems)
                {
                    inventory.TryRemoveItem(ItemData.FindById(requestedItem.itemId), requestedItem.amount);
                }
                target.nextDialogueOverwrite = successDialogue;
                
                CurrentAmount++;
                Evaluate();
            }
            else
            {
                target.nextDialogueOverwrite = failureDialogue;
            }
        }

        [Serializable]
        public struct RequestedItem
        {
            public int itemId;
            public int amount;
        }
        
        public static void WriteBringToGoal(NetworkWriter writer, QuestGoal goal)
        {
            BringToGoal bringToGoal = goal as BringToGoal;
            if (bringToGoal == null) return;
            
            writer.WriteArray(bringToGoal.RequestedItems);
            
            NetworkIdentity networkIdentity = bringToGoal.target == null ? null : bringToGoal.target.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);
            writer.WriteInt((int) bringToGoal.npcName);
            writer.WriteString(bringToGoal.successDialogue);
            writer.WriteString(bringToGoal.failureDialogue);
        }

        public static BringToGoal ReadBringToGoal(NetworkReader reader)
        {
            var goal = CreateInstance<BringToGoal>();

            goal.RequestedItems = reader.ReadArray<RequestedItem>();
            
            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            NPCObject target = networkIdentity != null
                ? networkIdentity.GetComponent<NPCObject>()
                : null;
            
            goal.target = target;
            goal.npcName = (NpcName) reader.ReadInt();
            goal.successDialogue = reader.ReadString();
            goal.failureDialogue = reader.ReadString();

            return goal;
        }
    }
}