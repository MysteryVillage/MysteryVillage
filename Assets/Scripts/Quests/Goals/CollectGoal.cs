using Events;
using Items;
using Mirror;
using UnityEngine;

namespace Quests.Goals
{
    public class CollectGoal : QuestGoal
    {
        public ItemData item;

        public new void Init(int questId)
        {
            Debug.Log("Init CollectGoal");
            base.Init(questId);
            
            EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
            if (eventSystem)
            {
                eventSystem.OnItemPickup.AddListener(ItemPickup);
                Debug.Log("Add Listener");
            }
        }

        public void ItemPickup(ItemData itemData)
        {
            Debug.Log("Collect Goal: ItemPickup - " + itemData.displayName);
            if (itemData.GetId() != item.GetId()) return;
            
            CurrentAmount++;
            Evaluate();
            QuestManager.Current.OnGoalUpdated.Invoke(this);
        }

        public CollectGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }

        public static void WriteCollectGoal(NetworkWriter writer, QuestGoal goal)
        {
            CollectGoal collectGoal = goal as CollectGoal;
            if (collectGoal == null) return;
            writer.WriteInt(collectGoal.item.GetId());
        }

        public static CollectGoal ReadCollectGoal(NetworkReader reader)
        {
            var itemId = reader.ReadInt();
            var item = ItemData.FindById(itemId);
            var goal = CreateInstance<CollectGoal>();

            goal.item = item;

            return goal;
        }
    }
}