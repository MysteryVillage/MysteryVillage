using Items;
using Quests;
using UnityEngine.Events;

namespace Events
{
    public class EventSystem : UnityEngine.EventSystems.EventSystem
    {
        // public static new EventSystem Current { get; private set; }
        
        public UnityEvent<ItemData> OnItemPickup;

        public UnityEvent<Quest> onQuestStart;
        public UnityEvent<Quest> onQuestFinish;
        public UnityEvent<string> onQuestEvent;
    }
}