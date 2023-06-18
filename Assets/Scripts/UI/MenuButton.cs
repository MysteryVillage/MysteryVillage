using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private RectTransform button_RectTransform;
    private TMP_Text tmp;


    // Start is called before the first frame update
    void Start()
    {
        button_RectTransform = GetComponent<RectTransform>();
        tmp = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        //transform.localScale = new Vector3(370, 300, 0);
        button_RectTransform.sizeDelta = new Vector2(380, 350);
        tmp.fontSize = 75; 
        Debug.Log("kello");
    }

    public void OnPointerExit()
    {
        button_RectTransform.sizeDelta = new Vector2(350, 300);
        tmp.fontSize = 70;
    }
}
