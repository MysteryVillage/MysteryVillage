using Items;
using Mirror;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TurnPipeline : NetworkBehaviour, IIinteractable
{


    public int speed = 300;
    bool isMoving = false;

    void Update()
    {
        /*if (isMoving)
        {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            StopAllCoroutines();
            StartCoroutine(Turn());
        }*/
    }

    public string GetInteractPrompt()
    {
        return string.Format("Turned Tube {0}", "here");
    }

    public void OnInteract(uint networkIdentifier)
    {
        if(isMoving)
        {
            return;
        }
        Debug.Log("turn tube");
        StopAllCoroutines();
        StartCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        isMoving = true;

        float remainingAngle = 90;
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.Rotate(0, rotationAngle, 0);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;
    }
}
