using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractTut : MonoBehaviour
{
    [SerializeField] private float interactRange = 5;
    [SerializeField] private GameObject lastTarget;
    [SerializeField] private GameObject currentTarget;


    private int highlightMask;
    private int actorMask;

    private void Awake()
    {
        highlightMask = LayerMask.NameToLayer("Highlight");
        actorMask = LayerMask.NameToLayer("Actor");
    }


    void Update()
    {
        HandleRaycast();
    }

    void HandleRaycast()
    {
        RaycastHit info;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height) * 0.5f), out info, interactRange, LayerMask.GetMask("Highlight", "Actor")))
        {
            currentTarget = info.collider.gameObject;
            if (lastTarget != null)
            {
                lastTarget.layer = actorMask;
                lastTarget = null;
            }
            if (currentTarget != null)
            {
                currentTarget.layer = highlightMask;
                lastTarget = currentTarget;
            }
        }
        else
        {
            currentTarget = null;
            if (lastTarget)
                lastTarget.layer = actorMask;
        }
    }
   

}
