using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : NetworkBehaviour
{
    public CharacterController player;
    public Transform cam;
    private Vector2 curMovementInput;
    public float moveSpeed = 5f;
    public float smoothTurn = 0.1f;
    public Vector3 fallVelocity;

    float smoothTurnVelocity;

    /**
     * Read and process movement inputs
     */
    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        } else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    void Update()
    {
        // if player is not the local player, don't move it
        if (!isLocalPlayer)
        {
            return;
        }

        // check if player is grounded
        // if he is not, apply gravity force
        if (player.isGrounded)
        {
            fallVelocity = new Vector3(0, -1f, 0);
        }
        else
        {
            fallVelocity -= Physics.gravity * -2 * Time.deltaTime;
        }
        
        // get movement inputs
        float horizontal = curMovementInput.x;
        float vertical = curMovementInput.y;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            player.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
        
        // Apply gravity
        player.Move(fallVelocity * Time.deltaTime);
       
    }
}
