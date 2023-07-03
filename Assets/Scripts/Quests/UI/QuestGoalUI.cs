using Quests.Goals;
using TMPro;
using UnityEngine;

namespace Quests.UI
{
    public class QuestGoalUI : MonoBehaviour
    {
        public TextMeshProUGUI description;
        public TextMeshProUGUI status;

        public void Init(QuestGoal questGoal)
        {
            description.text = questGoal.Description;
            status.text = $"{questGoal.CurrentAmount}/{questGoal.RequiredAmount}";
        }

        public void UpdateSlot(QuestGoal questGoal)
        {
            if (questGoal == null)
            {
                gameObject.SetActive(false); 
                return;
            }

            Debug.Log("Set stuff");
            
            gameObject.SetActive(true);
            description.text = questGoal.Description;
            status.text = $"{questGoal.CurrentAmount}/{questGoal.RequiredAmount}";
        }
    }
}