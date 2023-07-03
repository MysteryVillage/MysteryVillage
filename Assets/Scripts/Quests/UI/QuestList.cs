using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quests.UI
{
    public class QuestList : MonoBehaviour
    {
        private QuestManager _qm;
        public GameObject questListItem;
        public List<GameObject> questList;
        public QuestListInfo questInfo;
        
        private void Start()
        {
            _qm = QuestManager.Current;
            _qm.OnQuestListChanged.AddListener(UpdateList);
        }

        public void UpdateList()
        {
            var quests = _qm.Quests;
            
            ClearList();

            foreach (var quest in quests)
            {
                var item = Instantiate(questListItem, transform);
                item.GetComponent<QuestListItem>().Init(quest);
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