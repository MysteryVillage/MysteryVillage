using System;
using Mirror;
using Quests.Goals;
using Quests.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Quests
{
    public class QuestManager : NetworkBehaviour
    {
        Animator doorAnimationLeft = null;
        Animator doorAnimationRight = null;
        public readonly SyncList<Quest> Quests = new();
        [SerializeField, SyncVar]
        public Quest currentQuest;

        public static QuestManager Current;

        public UnityEvent<Quest> OnQuestChanged;
        public UnityEvent OnQuestListChanged;
        public UnityEvent<QuestGoal> OnGoalUpdated;

        [Header("Testing")]
        public Quest startQuest;

        [Header("AudioFeedback")] 
        public AudioClip questFinishedSound;
        public AudioClip questReceivedSound;

        [Header("Waypoints")] 
        public Sprite newQuestWaypoint;
        public Sprite questTargetWaypoint;

        [DoNotSerialize]
        public QuestNotificationUI notifier;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            if (isServer) {
                // AddQuest(startQuest);
                // SelectQuest(startQuest);
            }

            var eventSystem = EventSystem.current as Events.EventSystem;
            
            OnGoalUpdated.AddListener(QuestGoalUpdated);
        }
        
        public void SelectQuest(Quest quest)
        {
            currentQuest = quest;
            OnQuestChanged.Invoke(quest);
        }
        
        public void AddQuest(Quest quest)
        {
            if (!Quests.Contains(quest))
            {
                Quests.Add(quest);
                quest.Init();
                quest.QuestCompleted.AddListener(QuestFinished);
              
            }

            if (currentQuest == null)
            {
                SelectQuest(quest);
            }
            
            RefreshUI();
            
            // @TODO
            // audio and visual feedback
            // PlaySound(questReceivedSound);
            
            QuestRecievedVisual(quest.Information.name);
        }
      

        public void SetNewQuest(Quest quest)
        {
            currentQuest = null;
            AddQuest(quest);
            Debug.Log("Set new quest: "+quest.name);
        }

        public void QuestFinished(Quest quest)
        {
            Debug.Log("Quest finished: " + quest.Information.name);

            var nextQuest = GetNextQuest();
            currentQuest = nextQuest;
            
            RefreshUI();
            
            // @TODO
            // visual and audio feedback
            // PlaySound(questFinishedSound);
            QuestFinishedVisual(quest.Information.name);
        }

        public void QuestGoalUpdated(QuestGoal goal)
        {
            RefreshUI();
        }

        private Quest GetNextQuest()
        {
            foreach (var quest in Quests)
            {
                if (!quest.Completed) return quest;
            }

            return null;
        }

        [ClientRpc]
        void RefreshUI()
        {
            OnQuestListChanged.Invoke();
            OnQuestChanged.Invoke(currentQuest);
        }

        [ClientRpc]
        void QuestRecievedVisual(string title)
        {
            notifier.NewQuest(title);
        }

        [ClientRpc]
        void QuestFinishedVisual(string title)
        {
            notifier.QuestDone(title);
        }
        
        public void PlaySound(AudioClip clip)
        {
            if (NetworkClient.localPlayer != null) NetworkClient.localPlayer.GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}