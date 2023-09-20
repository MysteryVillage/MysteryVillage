using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLightController : MonoBehaviour
{
    private GameObject moon;
    public GameObject light;
    public Renderer glassRenderer;

    private bool enabled = false;

    private void Awake()
    {
        moon = GameObject.FindGameObjectWithTag("moon");
    }
    void Update()
    {
        if(moon)
        {
            if (moon.activeSelf)
            { 
                TurnOnNightLight(); 
            }
            else
            {
                TurnOffNightLight();
            }
        } 
    }

    private void TurnOnNightLight()
    {
        if(light != null && !enabled)
        {
            light.SetActive(true);
            enabled = true;
            glassRenderer.material.EnableKeyword("_EMISSION");
        }
    }

    private void TurnOffNightLight()
    {
        if(light != null && enabled)
        {
            light.SetActive(false);
            enabled = false;
            glassRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
