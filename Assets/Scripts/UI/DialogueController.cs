using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

namespace UI
{
    public class DialogueController : MonoBehaviour
    {
        public LineView lineView;
        public OptionsListView optionsListView;

        public void AdvanceLine(InputAction.CallbackContext context)
        {
            Debug.Log("Notice: Line Advancement disabled.", this);
            return;
            if (context.started) lineView.UserRequestedViewAdvancement();
        }
    }
}
