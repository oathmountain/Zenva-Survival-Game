using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IInteractible curInteractible;

    public TextMeshProUGUI promtText;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractible = hit.collider.GetComponent<IInteractible>();
                    setPromtText();
                }
            }
            else
            {
                curInteractible = null;
                curInteractGameObject = null;
                promtText.gameObject.SetActive(false);
            }
        }
    }

    void setPromtText()
    {
        promtText.gameObject.SetActive(true);
        promtText.text = string.Format("<b>[E]</b> {0}", curInteractible.GetInteractPromt());
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractible != null)
        {
            curInteractible.OnInteract();
            curInteractGameObject = null;
            curInteractible = null;
            promtText.gameObject.SetActive(false);
        }
    }
}

public interface IInteractible
{
    string GetInteractPromt();
    void OnInteract();
}
