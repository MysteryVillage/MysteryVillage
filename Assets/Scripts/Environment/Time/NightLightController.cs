using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLightController : MonoBehaviour
{
    private GameObject moon;
    private Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
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
        if(light != null && !light.enabled)
        {
            light.enabled = true;
        }
    }

    private void TurnOffNightLight()
    {
        if(light != null && light.enabled)
        {
            light.enabled = false;
        }
    }
}
