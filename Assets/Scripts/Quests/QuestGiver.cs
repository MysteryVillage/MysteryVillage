using System;
using Mirror;
using Player;
using UnityEngine;
using Yarn.Unity;

namespace Quests
{
    public class QuestGiver : NetworkBehaviour
    {
        public Quest quest;

        [YarnCommand("give_quest")]
        public void Give()
        {
            NetworkClient.localPlayer.GetComponent<InteractionManager>().AddQuest(quest);
        }
    }
}