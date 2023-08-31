using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    public GameObject lightSource;
    private RigBuilder rigBuilder;
    private InputAction flashlight;
    private Animator animator;
    private bool flashlightOn = false;
   

    // Start is called before the first frame update
    void Start()
    {
        rigBuilder = GetComponent<RigBuilder>();
        animator = GetComponent<Animator>();
        rigBuilder.layers[1].active = false;
        animator.SetLayerWeight(1, 0.0f);
        lightSource.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        if (!flashlightOn)
        {
            rigBuilder.layers[1].active = true;
            animator.SetLayerWeight(1, 0.8f);
            lightSource.SetActive(true);
            flashlightOn = true;

            Debug.Log("Taschenlampe an");
        }
        else
        {
            rigBuilder.layers[1].active = false;
            animator.SetLayerWeight(1, 0.0f);
            lightSource.SetActive(false);
            flashlightOn = false;

            Debug.Log("Taschenlampe aus");
        }
    }
}
