using System;
using TMPro;
using UnityEngine;

namespace Quests
{
    public class QuestInfoUI : MonoBehaviour
    {
        public TextMeshProUGUI questTitle;
        public TextMeshProUGUI questDescription;

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
                questDescription.text = quest.Information.description;
            }
            else
            {
                ToggleDisplay(false);
                questTitle.text = "";
                questDescription.text = "";
            }
        }

        void ToggleDisplay(bool val)
        {
            questTitle.gameObject.SetActive(val);
            questDescription.gameObject.SetActive(val);
        }
    }
}