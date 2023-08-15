using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnPipeline : MonoBehaviour
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
