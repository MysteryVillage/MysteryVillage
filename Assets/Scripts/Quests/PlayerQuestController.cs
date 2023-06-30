using System;
using Mirror;
using Quests.Goals;
using UnityEngine;

namespace Quests
{
    public class PlayerQuestController : NetworkBehaviour
    {
        public void OnGoalCollision(QuestGoalTrigger trigger)
        {
            TriggerQuestGoal(trigger);
        }
        
        [Command]
        protected void TriggerQuestGoal(QuestGoalTrigger trigger)
        {
            if (trigger == null) return; 
            
            trigger.OnGoalTrigger.Invoke();
        }
    }
}