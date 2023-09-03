using System;
using Events;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class QuestListener : NetworkBehaviour
    {
        public Quest listeningTo;

        public UnityEvent onStart;
        public UnityEvent onFinish;

        private void Start()
        {
            var eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
            if (eventSystem != null)
            {
                eventSystem.onQuestStart.AddListener(OnStart);  
                eventSystem.onQuestFinish.AddListener(OnFinish);    
            }
        }

        public void OnStart(Quest quest)
        {
            if (listeningTo == null) return;
            if (listeningTo.Equals(quest))
            {
                onStart.Invoke();
            }
        }

        public void OnFinish(Quest quest)
        {
            if (listeningTo == null) return;
            if (listeningTo.Equals(quest))
            {
                onFinish.Invoke();
            }
        }
    }
}