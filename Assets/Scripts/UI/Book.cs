using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Book : MonoBehaviour
{
    [SerializeField] float _pageSpeed = 0.5f;
    [SerializeField] List<Transform> _pages;
    int _index = -1;

    private void Start()
    {
        InitialState();
    }

    public void NextPage(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            RotateNext();
        }
    }

    public void PriorPage(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            RotateBack();
        }
    }

    public void InitialState()
    {
        for (int i=0; i<_pages.Count; i++)
        {
            _pages[i].transform.rotation = Quaternion.identity;
        }
        _pages[0].SetAsLastSibling();
    }

    public void RotateNext()
    {
        if (_index < _pages.Count-1)
        {
            _index++;
            _pages[_index].SetAsLastSibling();
            float angle = 180;
            Rotate(angle, -1);
        }
    }

    public void RotateBack()
    {
        if (_index > - 1)
        {
            _pages[_index].SetAsLastSibling();
            float angle = 0;
            Rotate(angle, 0);
        }
    }

    private void Rotate(float angle, int direction)
    {
        StopAllCoroutines();
        StartCoroutine(RotateOverTime(angle, direction));
    }

    IEnumerator RotateOverTime(float angle, int direction)
    {
        float timeElapsed = 0;
        float angle1 = 1;
        while (angle1 > 0.1f )
        {
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            timeElapsed += Time.deltaTime;
            _pages[_index].rotation = Quaternion.Lerp(_pages[_index].rotation, targetRotation, timeElapsed * _pageSpeed);
            angle1 = Quaternion.Angle(_pages[_index].rotation, targetRotation);

            yield return null;
        }
        if (direction == 0)
        {
            _index--;
        }
    }
}
