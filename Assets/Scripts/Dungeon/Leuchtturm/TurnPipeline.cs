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

    public string GetInteractPrompt()
    {
        return "Turned Tube";
    }

    public void OnInteract(uint networkIdentifier)
    {
        if(isMoving)
        {
            return;
        }
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
            transform.Rotate(0, 0, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;
    }
}
