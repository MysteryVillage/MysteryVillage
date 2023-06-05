using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOutline : MonoBehaviour
{
    [Header("Das Outline Material muss im Renderer ganz unten stehen!")]
    //assign all renderers
    [SerializeField]
    private List<Renderer> renderers;
    [SerializeField]
    private float thickness = 0.01f;
    [SerializeField]
    private Color outlineColor = Color.white;

    private List<Material> materials;

    //Get materials from each renderer
    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            if (renderer.materials.Length > 0)
            {
                //get last material slot
                Material lastMaterial = renderer.materials[renderer.materials.Length - 1];
                materials.Add(lastMaterial);
            }
        }
    }


    public void ToggleOutline(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //Enable thickness and change color
                material.SetFloat("_Thickness", thickness);
                material.SetColor("_OutlineColor", outlineColor);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                //disable thickness
                material.SetFloat("_Thickness", 0.0f);
            }
        }
    }
}