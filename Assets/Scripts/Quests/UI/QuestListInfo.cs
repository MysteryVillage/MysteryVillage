using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Quests.UI
{
    public class QuestListInfo : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public GameObject questGoals;
        public GameObject questGoalRow;
        public List<GameObject> questGoalRows;

        private void Start()
        {
            Clear();
        }

        public void SetInfo(Quest quest)
        {
            title.text = quest.Information.name;
            description.text = quest.Information.description;

            ClearGoals();
            
            foreach (var questGoal in quest.goals)
            {
                var goalRow = Instantiate(questGoalRow, questGoals.transform).GetComponent<QuestGoalUI>();
                goalRow.Init(questGoal);
                
                questGoalRows.Add(goalRow.gameObject);
            }
        }
        
        public void Clear()
        {
            title.text = "";
            description.text = "";
            
            ClearGoals();
        }

        void ClearGoals()
        {
            foreach (var row in questGoalRows.ToList())
            {
                questGoalRows.Remove(row);
                Destroy(row);
            }
        }
    }
}