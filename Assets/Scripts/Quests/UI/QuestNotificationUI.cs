using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Quests.UI
{
    public class QuestNotificationUI : MonoBehaviour
    {
        public TextMeshProUGUI info;
        public TextMeshProUGUI questTitle;

        public float timeToShow;
        
        private float timer;
        private bool active;

        private Stack<QuestNotification> queue = new();

        private void Start()
        {
            QuestManager.Current.notifier = this;
        }

        private void Update()
        {
            if (active)
            {
                timer += Time.deltaTime;

                if (timer > timeToShow)
                {
                    active = false;
                    timer = 0f;
                    ClearUI();
                }
            }
            else if (queue.Count > 0)
            {
                active = true;
                SetUI(queue.Pop());
            }
        }

        void SetUI(QuestNotification notification)
        {
            info.gameObject.SetActive(true);
            questTitle.gameObject.SetActive(true);
            info.text = notification.IsNewQuest ? "Neue Aufgabe" : "Aufgabe erledigt";
            questTitle.text = notification.Title;

            var qm = QuestManager.Current;
            qm.PlaySound(notification.IsNewQuest ? qm.questReceivedSound : qm.questFinishedSound);
        }

        void ClearUI()
        {
            info.text = "";
            questTitle.text = "";
            info.gameObject.SetActive(false);
            questTitle.gameObject.SetActive(false);
        }

        public void NewQuest(string title)
        {
            queue.Push(new QuestNotification(true, title));
        }

        public void QuestDone(string title)
        {
            queue.Push(new QuestNotification(false, title));
        }

        public class QuestNotification
        {
            public bool IsNewQuest;
            public string Title;

            public QuestNotification(bool isNewQuest, string title)
            {
                IsNewQuest = isNewQuest;
                Title = title;
            }
        }
    }
}