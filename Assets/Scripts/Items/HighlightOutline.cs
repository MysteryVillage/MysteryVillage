using System.Collections.Generic;
using UnityEngine;

namespace Items
{
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
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

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
                    material.SetFloat(Thickness, thickness);
                    material.SetColor(OutlineColor, outlineColor);
                    Debug.Log("Highlight " + gameObject.name);
                }
            }
            else
            {
                foreach (var material in materials)
                {
                    //disable thickness
                    material.SetFloat(Thickness, 0.0f);
                }
            }
        }
    }
}