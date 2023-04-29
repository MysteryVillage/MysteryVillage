using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] float _pageSpeed = 0.5f;
    [SerializeField] List<Transform> _pages;
    int _index = -1;

    public void RotateNext()
    {
        _index++;
        _pages[_index].SetAsLastSibling();
        float angle = 180;
        Rotate(angle, -1);
    }

    public void RotateBack()
    {
        _pages[_index].SetAsLastSibling();
        float angle = 0;
        Rotate(angle, 0);
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
