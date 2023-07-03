using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class QuestManager : NetworkBehaviour
    {
        public readonly SyncList<Quest> Quests = new();
        [SerializeField, SyncVar]
        public Quest currentQuest;

        public static QuestManager Current;

        public UnityEvent<Quest> OnQuestChanged;
        public UnityEvent OnQuestListChanged;

        [Header("Testing")]
        public Quest startQuest;

        [Header("AudioFeedback")] 
        public AudioClip questFinishedSound;
        public AudioClip questReceivedSound;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            if (isServer) {
                AddQuest(startQuest);
                SelectQuest(startQuest);
            }
        }
        
        public void SelectQuest(Quest quest)
        {
            currentQuest = quest;
            OnQuestChanged.Invoke(quest);
        }

        public void AddQuestCmd(Quest quest)
        {
            AddQuest(quest);
        }
        
        public void AddQuest(Quest quest)
        {
            if (!Quests.Contains(quest))
            {
                Quests.Add(quest);
                quest.Init();
                quest.QuestCompleted.AddListener(QuestFinished);
            }
            RefreshUI();

            if (currentQuest == null)
            {
                SelectQuest(quest);
            }
            
            // @TODO
            // audio and visual feedback
            PlaySound(questReceivedSound);
        }

        public void QuestFinished(Quest quest)
        {
            Debug.Log("Quest finished: " + quest.Information.name);
            currentQuest = null;
            RefreshUI();
            
            // @TODO
            // visual and audio feedback
            PlaySound(questFinishedSound);
        }

        [ClientRpc]
        void RefreshUI()
        {
            OnQuestListChanged.Invoke();
            OnQuestChanged.Invoke(currentQuest);
        }
        
        void PlaySound(AudioClip clip)
        {
            NetworkClient.localPlayer.GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}