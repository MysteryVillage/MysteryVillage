using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController player;
    public Transform cam;
    public float moveSpeed = 5f;
    public float smoothTurn = 0.1f;

    float smoothTurnVelocity;

   
    void Start()
    {
        
    }

    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            player.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
       
    }
}
