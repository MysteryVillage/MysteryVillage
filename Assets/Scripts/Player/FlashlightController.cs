using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    public GameObject lightSource;
    public AudioClip audioClip;

    private RigBuilder rigBuilder;
    private InputAction flashlight;
    private Animator animator;
    private AudioSource audioSource;
    private bool flashlightOn = false;


    // Start is called before the first frame update
    void Start()
    {
        rigBuilder = GetComponent<RigBuilder>();
        animator = GetComponent<Animator>();
        rigBuilder.layers[1].active = false;
        animator.SetLayerWeight(1, 0.0f);
        lightSource.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;

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
            audioSource.Play();

            Debug.Log("Taschenlampe an");
        }
        else
        {
            rigBuilder.layers[1].active = false;
            animator.SetLayerWeight(1, 0.0f);
            lightSource.SetActive(false);
            flashlightOn = false;
            audioSource.Play();

            Debug.Log("Taschenlampe aus");
        }
    }
}
