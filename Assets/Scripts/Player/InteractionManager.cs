using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : NetworkBehaviour
{
    public float checkRate = .05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IIinteractable curInteractable;
    public TextMeshProUGUI promptText;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IIinteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        Debug.Log(curInteractable.GetInteractPrompt());
        Debug.Log(promptText.text);
        promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt());
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    public void OnInteract()
    {
        OnInteractCmd(curInteractable.GetHashCode());
    }

    [Command]
    public void OnInteractCmd(int interactable)
    {
        GameObject go = GameObject.Find(interactable.ToString());
        Debug.Log(go.GetHashCode());
        go.GetComponent<IIinteractable>().OnInteract();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Gizmos.DrawRay(ray.origin, ray.direction);
    }}

public interface IIinteractable
{
    string GetInteractPrompt();
    void OnInteract();
}