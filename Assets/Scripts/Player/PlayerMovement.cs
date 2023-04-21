using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController player;
    public Transform cam;
    public Transform tpCam;
    private Vector2 curMovementInput;
    
    private float moveSpeed;
    public float defaultMoveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float smoothTurn = 0.1f;
    public Vector3 fallVelocity;
    float smoothTurnVelocity;
    public float jumpForce = 10f;
    Vector3 moveDirection;
    private bool jump = false;
    private bool sprint = false;


    private void Start()
    {
        if (!isLocalPlayer)
        {
            GetComponent<PlayerInput>().enabled = false;
            cam.gameObject.SetActive(false);
            tpCam.gameObject.SetActive(false);

           
        }

       

    }

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

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {

            if (player.isGrounded)
            {
                jump = true;
            }
            
            
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            jump = false;
        }
       
    }

    public void OnPlayerPause(InputAction.CallbackContext context)
    {
        // Menü Einblenden 
    }

    public void OnPlayerSprint(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            sprint = true;
        }else if(context.phase == InputActionPhase.Canceled)
        {
            sprint = false;
        }
    }

    public void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            // Hier wird interagiert. 
        }
    }

    void Update()
    {
        /* ---------------------- Sprint -------------------------- */
        /* Noch ohne Zeitbegrenzung 
         * Sprinten wird beendet wenn die Taste losgelassen wird
         */
        if (sprint) // Soll der Spieler auch beim Springen weiter sprinten können ? (Sprintsprung) 
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }

        /* ---------------------- Sprint -------------------------- */
        /* Jump Funktioniert noch nicht so richtig
         * Sprung höhe hängt noch davon ab wie lange die Spacetaste gedrückt wurde
         * Irgendwas muss noch mit der Grvity passiern wenn jump gedrückt wurde
         * Jumpforce erhöhen sieht nicht mehr gut aus
         */
        if (jump)
        {
            Vector3 test = Vector3.up;
            player.Move(test * jumpForce * Time.deltaTime);

        }


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

             moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            player.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }

       
        

        // Apply gravity
        ApplyGravity();

    }

    private void ApplyGravity()
    {
        player.Move(fallVelocity * Time.deltaTime);
    }
}
