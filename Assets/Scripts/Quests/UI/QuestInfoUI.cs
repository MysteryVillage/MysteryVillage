using Quests.UI;
using TMPro;
using UnityEngine;

namespace Quests
{
    public class QuestInfoUI : MonoBehaviour
    {
        public TextMeshProUGUI questTitle;
        public GameObject questDescription;

        public GameObject questGoalRow;
        public QuestGoalUI[] questGoalSlots;

        private void Start()
        {
            QuestManager.Current.OnQuestChanged.AddListener(UpdateQuest);
            ToggleDisplay(false);
        }

        public void UpdateQuest(Quest quest)
        {
            if (quest != null) {
                ToggleDisplay(true);
                questTitle.text = quest.Information.name;
                for (var i = 0; i < questGoalSlots.Length; i++)
                {
                    var goalSlot = questGoalSlots[i];
                    if (i >= quest.goals.Count)
                    {
                        goalSlot.UpdateSlot(null);
                    }
                    else
                    {
                        var goal = quest.goals[i];
                        goalSlot.UpdateSlot(goal);
                        Debug.Log("Goal: " + goal.Description);
                    }
                }
            }
            else
            {
                ToggleDisplay(false);
                questTitle.text = "";
            }
        }

        void ToggleDisplay(bool val)
        {
            questTitle.gameObject.SetActive(val);
            questDescription.SetActive(val);
        }

        void ClearGoals()
        {
            var children = questDescription.transform.childCount;

            for (var i = children - 1; i > 0; i--)
            {
                var go = questDescription.transform.GetChild(i).gameObject;
                Destroy(go);
            }
        }
    }
}