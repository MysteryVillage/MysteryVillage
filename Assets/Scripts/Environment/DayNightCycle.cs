using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Einstellungen SkyBox")]
    public bool AktivateCycle = true;
    public bool useDefaultSkyBox = false;


    [Range(0.0f,1.0f)]
    public float time;
    public float fullDayLength; // in Seconds 
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;


    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lighthingIntensityMultiplier;
    public AnimationCurve reflectionsIntensityMultipler;
    [SerializeField] private Material skyBoxDefault;
    [SerializeField] private Material skyBox;
    private int skyboxExposure = Shader.PropertyToID("_Exposure");
    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        if(useDefaultSkyBox && skyBoxDefault != null)
        {
            RenderSettings.skybox = skyBoxDefault;
        }else if(!useDefaultSkyBox && skyBox != null)
        {
            RenderSettings.skybox = skyBox;
        }
    }

    private void Update()
    {
        if (AktivateCycle)
        {
            time += timeRate * Time.deltaTime;
        }
        

        if(time >= 1.0f)
        {
            time = 0.0f;
        }

        // Light Rotation

        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        // Light intensity 

        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        // change colors 

        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        // enable / disable sun

        if(sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
        }else if(sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
        }
        // enable / disable moon

        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
        }

        // lighting and reflections intensity 
        RenderSettings.ambientIntensity = lighthingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultipler.Evaluate(time);
        skyBox.SetFloat(skyboxExposure, Mathf.Lerp(0.15f, 0.85f, lighthingIntensityMultiplier.Evaluate(time)));
    }
}
