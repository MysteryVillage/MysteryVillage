using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool sneak;
		public bool interact;
		public bool pause;
		private bool toggle = false;
		private float count = 0;
		

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMoveInput(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				MoveInput(context.ReadValue<Vector2>());
			} else if (context.phase == InputActionPhase.Canceled)
			{
				MoveInput(Vector2.zero);
			}
		}

		public void OnLookInput(InputAction.CallbackContext context)
		{
			if(cursorInputForLook)
			{
				LookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnJumpInput(InputAction.CallbackContext context)
		{
			JumpInput(context.performed);
		}

		public void OnSprintInput(InputAction.CallbackContext context)
		{
			SprintInput(context.performed);
		}

		public void OnSneakInput(InputAction.CallbackContext context)
		{
			SneakInput(context.performed);
		}

		public void OnPauseInput(InputAction.CallbackContext context)
		{
			if (Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			PauseInput(context.started);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void SneakInput(bool newSneakState)
		{
			if (newSneakState && !toggle)
			{
                sneak = true;
				toggle = true;
            }
			if(newSneakState && toggle)
			{
				count++;
			}
			if(count == 2)
			{
				sneak = false;
				count = 0;
                toggle = false;
            }
        }

		public void PauseInput(bool newPauseState)
		{
			pause = newPauseState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}