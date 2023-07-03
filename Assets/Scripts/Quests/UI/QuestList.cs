using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Quests.UI
{
    public class QuestList : MonoBehaviour
    {
        public GameObject questListItem;
        public List<GameObject> questList;
        public QuestListInfo questInfo;

        public Menu menu;
        
        private void Start()
        {
            QuestManager.Current.OnQuestListChanged.AddListener(UpdateList);
        }

        public void UpdateList()
        {
            var quests = QuestManager.Current.Quests;
            
            ClearList();

            foreach (var quest in quests)
            {
                var item = Instantiate(questListItem, transform);
                item.GetComponent<QuestListItem>().Init(quest);
                questList.Add(item);
            }

            if (questList.First() != null)
            {
                menu.startButton = questList.First().GetComponentInChildren<Button>()?.gameObject;
            }
        }

        public void ClearList()
        {
            foreach (var item in questList.ToList())
            {
                questList.Remove(item);
                Destroy(item);
            }
        }
    }
}