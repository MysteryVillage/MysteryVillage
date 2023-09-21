using Quests.Goals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quests.UI
{
    public class QuestGoalUI : MonoBehaviour
    {
        public TextMeshProUGUI description;
        public TextMeshProUGUI status;
        public Image checkmark;

        public Sprite done;
        public Sprite todo;

        public void Init(QuestGoal questGoal)
        {
            SetInfo(questGoal);
        }

        public void UpdateSlot(QuestGoal questGoal)
        {
            if (questGoal == null)
            {
                gameObject.SetActive(false); 
                return;
            }
            
            gameObject.SetActive(true);
            SetInfo(questGoal);
        }

        void SetInfo(QuestGoal questGoal)
        {
            description.text = questGoal.Description;
            // if (questGoal.RequiredAmount == 1)
            // {
            //     checkmark.gameObject.SetActive(true);
            //     status.gameObject.SetActive(false);
            //
            //     checkmark.sprite = questGoal.CurrentAmount < 1 ? todo : done;
            // }
            // else
            // {
            //     checkmark.gameObject.SetActive(false);
            //     status.gameObject.SetActive(true);
            //     
            //     status.text = $"{questGoal.CurrentAmount}/{questGoal.RequiredAmount}";
            // }
            
            checkmark.gameObject.SetActive(false);
            status.gameObject.SetActive(false);
        }
    }
}