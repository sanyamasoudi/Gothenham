using System;
using Development.Scripts.InteractableSystem.Interactable;
using UnityEngine;

namespace Development.Scripts.InteractableSystem
{
    public class InteractionController : MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] private InteractionInputData interactionInputData;
        [SerializeField] private InteractionData interactionData;
        
        [Header("UI")]
        [SerializeField] private InteractionUIPanel interactionUIPanel;

        [Space] [Header("Raycast Settings")] 
        [SerializeField] private LayerMask raycastLayerMask;
        [SerializeField] private float raycastDistance;
        [SerializeField] private float raycastSphereRadius;

        private Camera mainCamera;
        private bool isInteracting = false;
        private float holdTimer = 0;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            CheckForInteractable();
            CheckForInteractableInput();
        }

        private void CheckForInteractable()
        {
            var ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hitInfo;
            var isHit = Physics.SphereCast(ray, raycastSphereRadius, out hitInfo, raycastDistance, raycastLayerMask);
            if (isHit)
            {
                var interactable = hitInfo.transform.GetComponent<InteractableBase>();
                if (interactable)
                {
                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = interactable;
                        interactionUIPanel.SetTooltipText("hold R to interact");
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(interactable))
                        {
                            interactionData.Interactable = interactable;
                            interactionUIPanel.SetTooltipText("hold R to interact");
                        }
                    }
                }
            }
            else
            {
                interactionUIPanel.Reset();
                interactionData.ResetData();
            }

            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, isHit ? Color.green : Color.red);
        }
        
        private void CheckForInteractableInput()
        {
            if(interactionData.IsEmpty()) return;
            if (interactionInputData.InteractedClicked)
            {
                isInteracting = true;
                holdTimer = 0f;
            }

            if (interactionInputData.InteractedReleased)
            {
                isInteracting = false;
                holdTimer = 0f;
                interactionUIPanel.UpdateProgress(holdTimer);
            }

            if (isInteracting)
            {
                if(!interactionData.Interactable.IsInteractable) return;
                if (interactionData.Interactable.HoldInteract)
                {
                    holdTimer += Time.deltaTime;
                    var holdPercent = holdTimer / interactionData.Interactable.HoldDuration;
                    interactionUIPanel.UpdateProgress(holdPercent);
                    
                    if (holdPercent > 1f)
                    {
                        interactionData.Interact();
                        isInteracting = false;
                    }
                }
                else
                {
                    interactionData.Interact();
                    isInteracting = false;
                }
            }
        }
    }
}