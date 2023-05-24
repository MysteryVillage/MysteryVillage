using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
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
    private bool sprint = false;

    public float sneakSpeed = 2f;
    private bool sneak = false;

    public float smoothTurn = 0.1f;

    private float jumpHeight = 1.0f;
    private bool jump = false;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    float smoothTurnVelocity;
    
    Vector3 moveDirection;

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
            jump = true;
        }
        else
        {
            jump = false;
        }
       
    }

    public void OnPlayerPause(InputAction.CallbackContext context)
    {
        // Men� Einblenden 
    }

    public void OnPlayerSprint(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started) { sprint = true;}
        else if(context.phase == InputActionPhase.Canceled) { sprint = false;}
    }

    public void OnPlayerSneak(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed){ sneak = true; }
        else if(context.phase == InputActionPhase.Canceled) { sneak = false; }
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

        if (sprint && !sneak) 
        {moveSpeed = sprintSpeed;}
        else if(!sprint && !sneak)
        { moveSpeed = defaultMoveSpeed;}

        /* ---------------------- Sneak -------------------------- */
        /* Spieler Collider wird noch nicht kleiner skaliert
         * Der Spieler Schleicht solange die Taste gedr�ckt wird
         *      *  Soll die Taste nur einmal gedr�ckt werden oder die ganze Zeit?
         *      *  Umsehen ist nicht m�glich wenn die Sneaktaste gedr�ckt gehalten werden muss
         *       
         */

        if (sneak && !sprint) 
        { moveSpeed = sneakSpeed; }
        else if(!sneak && !sprint)
        { moveSpeed = defaultMoveSpeed;}

        /* ---------------------- Jump -------------------------- */
        /* Jump Funktioniert noch nicht so richtig
         * Bug: Wenn die Jump-Taste gedr�ckt gehalten wird und sich dann bewegt wird dauert es bis die taste wieder Funktioniert
         *      Auch wenn die Jump-Taste schnell hintereinander gedr�ckt wird 
         */

        groundedPlayer = player.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (jump && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        player.Move(playerVelocity * Time.deltaTime);

        if (!player.isGrounded) { jump = false; }


        // if player is not the local player, don't move it
        if (!isLocalPlayer)
        {
            return;
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
        
    }
}
