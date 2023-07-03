using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Quests.UI
{
    public class QuestListItem : MonoBehaviour
    {
        public Quest quest;
        public TextMeshProUGUI title;

        public void Init(Quest q)
        {
            quest = q;
            title.text = quest.Information.name;
        }

        public void Refresh(Quest overwrite)
        {
            quest = overwrite;
        }

        public void OnSelect()
        {
            Debug.Log("Select button: " + quest.Information.name);
            transform.GetComponentInParent<QuestList>().questInfo.SetInfo(quest);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click button: " + quest.Information.name);
            transform.GetComponentInParent<QuestList>().questInfo.SetInfo(quest);
        }
    }
}