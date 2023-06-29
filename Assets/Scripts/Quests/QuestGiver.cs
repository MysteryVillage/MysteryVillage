using System;
using UnityEngine;
using Yarn.Unity;

namespace Quests
{
    public class QuestGiver : MonoBehaviour
    {
        public Quest quest;
        private QuestManager _questManager;

        private void Start()
        {
            _questManager = QuestManager.Current;
        }

        [YarnCommand("give_quest")]
        public void Give()
        {
            _questManager.AddQuest(quest);
        }
    }
}