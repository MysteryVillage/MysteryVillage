using System;
using System.Collections.Generic;
using Inventory;
using Items;
using Mirror;
using Network;
using Player;
using Quests.Goals;
using Quests.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Yarn.Unity;
using NetworkManager = Network.NetworkManager;

namespace Quests
{
    public class QuestManager : NetworkBehaviour
    {
        public readonly SyncList<Quest> Quests = new();
        [SerializeField, SyncVar]
        public Quest currentQuest;
        public List<Quest> availableQuests;

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
            if (!isServer) return;
            if (quest == null) return;
            
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

        [YarnCommand("move_npc")]
        public static void MoveNpc(string npcName, string targetName)
        {
            var npc = GameObject.Find(npcName);
            var target = GameObject.Find(targetName);

            if (npc == null || target == null) return;
            npc.transform.position = target.transform.position;
            npc.transform.rotation = target.transform.rotation;
        }

        [YarnCommand("show_go")]
        public static void ShowGameObject(string goName)
        {
            var go = GameObject.Find(goName);
            if (go == null) return;
            go.SetActive(true);
        }

        [YarnCommand("hide_go")]
        public static void HideGameObject(string goName)
        {
            var go = GameObject.Find(goName);
            if (go == null) return;
            go.SetActive(false);
        }

        [YarnCommand("give_quest_by_id")]
        public static void GiveQuest(int questId)
        {
            var quest = Quest.FindById(questId);
            if (quest == null) return;
            Current.AddQuest(quest);
        }

        [YarnCommand("spawn_item")]
        public static void SpawnItem(int itemId, string position)
        {
            if (!NetworkServer.active) return;
            
            var go = GameObject.Find(position);
            var pos = go.transform.position;
            var rot = go.transform.rotation;

            var item = ItemData.FindById(itemId);
            var itemObject = Instantiate(item.dropPrefab, pos, rot);
            
            NetworkServer.Spawn(itemObject);
        }

        [YarnCommand("give_item")]
        public static void GiveItem(int itemId)
        {
            Debug.Log("isServer: " + Current.isServer);
            if (!Current.isServer) return;
            PlayerController colin = null;
            var players = PlayerController.GetPlayers();
            foreach (var player in players)
            {
                if (player.isBoy) colin = player;
            }

            Debug.Log("inventory: " + colin);
            if (colin == null) return;

            var inv = colin.GetComponent<PlayerInventory>();
            
            inv.CollectItem(itemId);
        }
    }
}