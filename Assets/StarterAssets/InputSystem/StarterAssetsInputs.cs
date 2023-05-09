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
		public void OnMove(InputValue value)
		{
			Debug.Log("Move input");
			Debug.Log(value.ToString());
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}


		public void OnSneak(InputValue value)
		{
			SneakInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

		public void OnPause(InputValue value)
		{
			if (Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			PauseInput(value.isPressed);
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

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
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