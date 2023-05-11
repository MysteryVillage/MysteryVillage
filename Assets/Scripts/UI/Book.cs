using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Book : MonoBehaviour
{
    [SerializeField] float _pageSpeed = 0.5f;
    [SerializeField] List<Transform> _pages;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject forwardButton;
    int _index = -1;
    bool rotate = false;

    private void Start()
    {
        backButton.SetActive(false);
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
        if (_index < _pages.Count-1 && rotate == false)
        {
            _index++;
            float angle = 180;
            ForwardButtonAction();
            _pages[_index].SetAsLastSibling();
            Rotate(angle, -1);
        }
    }

    private void ForwardButtonAction()
    {
        if (backButton.activeInHierarchy == false)
        {
            backButton.SetActive(true);
        }
        if (_index == _pages.Count - 1)
        {
            forwardButton.SetActive(false);
        }
    }

    public void RotateBack()
    {
        if (_index > - 1 && rotate == false)
        {
            _pages[_index].SetAsLastSibling();
            float angle = 0;
            BackButtonAction();
            Rotate(angle, 0);
        }
    }

    private void BackButtonAction()
    {
        if (forwardButton.activeInHierarchy == false)
        {
            forwardButton.SetActive(true);
        }
        if (_index - 1 == -1)
        {
            backButton.SetActive(false);
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
            rotate = true;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            timeElapsed += Time.deltaTime;
            _pages[_index].rotation = Quaternion.Lerp(_pages[_index].rotation, targetRotation, timeElapsed * _pageSpeed);
            angle1 = Quaternion.Angle(_pages[_index].rotation, targetRotation);

            yield return null;
        }
        rotate = false;

        if (direction == 0)
        {
            _index--;
        }
    }
}
