using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player;
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
            menu.onClose.AddListener(ClearWindow);
        }

        public void UpdateList()
        {
            var quests = QuestManager.Current.Quests;
            
            ClearList();

            foreach (var quest in quests)
            {
                if (quest.Completed) continue;
                
                var item = Instantiate(questListItem, transform);
                item.GetComponent<QuestListItem>().Init(quest);
                questList.Add(item);
            }

            if (questList.Count > 0)
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

        public void ClearWindow()
        {
            questInfo.Clear();
        }
        
        public void SelectActiveQuest()
        {
            GetComponentInParent<InteractionManager>().SelectQuest(questInfo.questId);
        }
    }
}