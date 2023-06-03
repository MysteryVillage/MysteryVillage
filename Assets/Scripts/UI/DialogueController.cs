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
            if (context.started) lineView.UserRequestedViewAdvancement();
        }
    }
}
