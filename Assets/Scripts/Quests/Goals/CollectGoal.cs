using Items;

namespace Quests.Goals
{
    public class CollectGoal : QuestGoal
    {
        public ItemData item;
        
        public CollectGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }
    }
}