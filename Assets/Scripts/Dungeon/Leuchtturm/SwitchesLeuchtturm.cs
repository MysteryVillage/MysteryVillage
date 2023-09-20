using Mirror;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesLeuchtturm : NetworkBehaviour, IIinteractable
{
    [SerializeField] Renderer _lamp;
    [SerializeField] Material _red;
    [SerializeField] Material _green;
    [SerializeField] private Animator levlerAnimation = null;

    private float speed = 80f;
    private bool switchState = true;
    private bool toggle = false;

    private bool _hasAnimator = false;
    private int _animID;

    private void Start()
    {
        if (levlerAnimation != null) _hasAnimator = true;
        _animID = Animator.StringToHash("Schalter");
        changeLamps(false);
    }

    public string GetInteractPrompt()
    {
        return "Schalter umlegen";
    }

    public void OnInteract(uint networkIdentifier)
    {
        if (switchState == true && !toggle)
        {
            AnimateSwitchDown(); // Schalter Oben -> Unten
            changeLamps(true);
        }
        else if (!switchState && toggle)
        {
            AnimateSwitchUp(); // Schalter Unten -> Oben 
            changeLamps(false);
        }
    }
    private void changeLamps(bool lampState)
    {
        if (isServer)
        {
            toggleLamp(lampState);
        }
        
    }

    private void AnimateSwitchDown()
    {

        if (levlerAnimation == null) return;

        switchState = false; // Schalter ist unten 

        if (_hasAnimator)
        {
            setSwitchAnimation(true);

        }
    }

    private void AnimateSwitchUp()
    {
        if (levlerAnimation == null) return;

        switchState = true; // Schalter ist oben 
        if (_hasAnimator)
        {
            setSwitchAnimation(false); // Animator Updaten


        }
    }

    [ClientRpc]
    private void setSwitchAnimation(bool SwitchDown)
    {
        if (SwitchDown)
        {
            StartCoroutine(SwitchDelayDown(levlerAnimation.GetCurrentAnimatorStateInfo(0).length));
            levlerAnimation.Play("Switch_Down", 0, 0.0f);
        }
        else
        {
            StartCoroutine(SwitchDelayUp(levlerAnimation.GetCurrentAnimatorStateInfo(0).length));
            levlerAnimation.Play("Switch_Up", 0, 0.0f);
        }

    }

    [ClientRpc]
    private void toggleLamp(bool lampState)
    {
        if (lampState)
        {
            _lamp.material = _green;
        }
        else
        {
            _lamp.material = _red;
        }
    }

    IEnumerator SwitchDelayUp(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        toggle = false;
    }

    IEnumerator SwitchDelayDown(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        toggle = true;
    }
}
